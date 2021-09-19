using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NodeLibraryGen
{
    public class EventGenBuilder : BaseBuilder
    {
        private EventGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
        {
            Success = true;
            Id = id;

            TargetUnit = new CodeCompileUnit();
            CodeNamespace importsNamespace = new()
            {
                Imports = {
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Base"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Primitive"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.TypeDefArray"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.TypeDefComposite"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.TypeDefVariant"),
                    new CodeNamespaceImport("System.Collections.Generic"),
                    new CodeNamespaceImport("System")
                }
            };

            CodeNamespace typeNamespace = new("SubstrateNetApi.Model.Custom.Events");
            TargetUnit.Namespaces.Add(importsNamespace);
            TargetUnit.Namespaces.Add(typeNamespace);

            var fullPath = $"{String.Join('.', typeDef.Path)}";

            ClassName = typeDef.Path[0].MakeMethod();

            var palletName = typeDef.Path[0]
                .Replace("pallet_", "")
                .Replace("frame_", "")
                .MakeMethod();

            TargetClass = new CodeTypeDeclaration(ClassName)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };

            TargetClass.Comments.Add(new CodeCommentStatement("<summary>", true));
            TargetClass.Comments.Add(new CodeCommentStatement($">> Path: {fullPath}", true));
            foreach (var doc in typeDef.Docs)
            {
                TargetClass.Comments.Add(new CodeCommentStatement(doc, true));
            }
            TargetClass.Comments.Add(new CodeCommentStatement("</summary>", true));

            if (typeDef.Variants != null)
            {
                foreach (var variant in typeDef.Variants)
                {
                    var eventClass = new CodeTypeDeclaration(variant.Name.MakeMethod())
                    {
                        IsClass = true,
                        TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                    };

                    eventClass.Comments.Add(new CodeCommentStatement("<summary>", true));
                    eventClass.Comments.Add(new CodeCommentStatement($">> Event: {variant.Name}", true));
                    foreach (var doc in variant.Docs)
                    {
                        eventClass.Comments.Add(new CodeCommentStatement(doc, true));
                    }
                    eventClass.Comments.Add(new CodeCommentStatement("</summary>", true));

                    var codeTypeRef = new CodeTypeReference("BaseTuple");
                    if (variant.TypeFields != null)
                    {
                        foreach (var field in variant.TypeFields)
                        {
                            if (typeDict.TryGetValue(field.TypeId, out string baseType))
                            {
                                codeTypeRef.TypeArguments.Add(new CodeTypeReference(baseType));
                            }
                            else
                            {
                                Success = false;
                            }
                        }
                    }
                    eventClass.BaseTypes.Add(codeTypeRef);

                    TargetClass.Members.Add(eventClass);
                }
            }

            typeNamespace.Types.Add(TargetClass);

        }
        private CodeMemberField GetPropertyField(string name, string baseType)
        {
            CodeMemberField field = new()
            {
                Attributes = MemberAttributes.Private,
                Name = name.MakePrivateField(),
                Type = new CodeTypeReference($"{baseType}")
            };
            return field;
        }

        private CodeMemberProperty GetProperty(string name, CodeMemberField propertyField)
        {
            CodeMemberProperty prop = new()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = name.MakeMethod(),
                HasGet = true,
                HasSet = true,
                Type = propertyField.Type
            };
            prop.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), propertyField.Name)));
            prop.SetStatements.Add(new CodeAssignStatement(
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), propertyField.Name),
                    new CodePropertySetValueReferenceExpression()));
            return prop;
        }

        private CodeMemberMethod GetDecode(NodeTypeField[] typeFields, List<string> types)
        {
            var decodeMethod = SimpleMethod("Decode");
            CodeParameterDeclarationExpression param1 = new()
            {
                Type = new CodeTypeReference("System.Byte[]"),
                Name = "byteArray"
            };
            decodeMethod.Parameters.Add(param1);
            CodeParameterDeclarationExpression param2 = new()
            {
                Type = new CodeTypeReference("System.Int32"),
                Name = "p",
                Direction = FieldDirection.Ref
            };
            decodeMethod.Parameters.Add(param2);
            decodeMethod.Statements.Add(new CodeSnippetExpression("var start = p"));

            if (typeFields != null)
            {
                for (int i = 0; i < typeFields.Length; i++)
                {
                    NodeTypeField typeField = typeFields[i];

                    string fieldName = typeField.Name;
                    if (typeField.Name == null)
                    {
                        fieldName = typeFields.Length > 1 ? typeField.TypeName : "value";
                        //Console.WriteLine(ClassName + ": " + JsonConvert.SerializeObject(typeField) + " -- >" + types[i]);
                    }

                    decodeMethod.Statements.Add(new CodeSnippetExpression($"{fieldName.MakeMethod()} = new {types[i]}()"));
                    decodeMethod.Statements.Add(new CodeSnippetExpression($"{fieldName.MakeMethod()}.Decode(byteArray, ref p)"));
                }
            }

            decodeMethod.Statements.Add(new CodeSnippetExpression("_typeSize = p - start"));
            return decodeMethod;
        }

        private CodeMemberMethod GetEncode(NodeTypeField[] typeFields)
        {
            CodeMemberMethod encodeMethod = new()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
                Name = "Encode",
                ReturnType = new CodeTypeReference("System.Byte[]")
            };
            encodeMethod.Statements.Add(new CodeSnippetExpression("var result = new List<byte>()"));

            if (typeFields != null)
            {
                foreach (var typeField in typeFields)
                {
                    string fieldName = typeField.Name;
                    if (typeField.Name == null)
                    {
                        fieldName = typeFields.Length > 1 ? typeField.TypeName : "value";
                        //Console.WriteLine(ClassName + ": " + JsonConvert.SerializeObject(typeField) + " -- >" + types[i]);
                    }

                    encodeMethod.Statements.Add(new CodeSnippetExpression($"result.AddRange({fieldName.MakeMethod()}.Encode())"));
                }
            }

            encodeMethod.Statements.Add(new CodeSnippetExpression("return result.ToArray()"));
            return encodeMethod;
        }

        public static EventGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
        {
            return new EventGenBuilder(id, typeDef, typeDict);
        }

        public string Build(out bool success)
        {
            success = Success;
            if (Success)
            {
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CodeGeneratorOptions options = new()
                {
                    BracingStyle = "C"
                };
                var path = Path.Combine("Model", "Custom", "Events", ClassName + ".cs");
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (StreamWriter sourceWriter = new(path))
                {
                    provider.GenerateCodeFromCompileUnit(
                        TargetUnit, sourceWriter, options);
                }
            }
            return ClassName;
        }
    }
}
