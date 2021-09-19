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
    public class StructGenBuilder : BaseBuilder
    {
        private StructGenBuilder(uint id, NodeTypeComposite typeDef, List<string> types)
        {
            Id = id;

            TargetUnit = new CodeCompileUnit();
            CodeNamespace importsNamespace = new() {
                Imports = {
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Base"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Primitive"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Sequence"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Composite"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Enum"),
                    new CodeNamespaceImport("System.Collections.Generic"),
                    new CodeNamespaceImport("System")
                }
            };

            CodeNamespace typeNamespace = new("SubstrateNetApi.Model.Types.Composite");
            TargetUnit.Namespaces.Add(importsNamespace);
            TargetUnit.Namespaces.Add(typeNamespace);

            var fullPath = $"{String.Join('.', typeDef.Path)}";

            ClassName = $"{typeDef.Path.Last()}";

            TargetClass = new CodeTypeDeclaration(ClassName)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };
            TargetClass.BaseTypes.Add(new CodeTypeReference("BaseType"));
            TargetClass.Comments.Add(new CodeCommentStatement("<summary>", true));
            TargetClass.Comments.Add(new CodeCommentStatement($">> Path: {fullPath}", true));
            if (typeDef.Docs != null) {
                foreach (var doc in typeDef.Docs)
                {
                    TargetClass.Comments.Add(new CodeCommentStatement(doc, true));
                }
            }
            TargetClass.Comments.Add(new CodeCommentStatement("</summary>", true));
            typeNamespace.Types.Add(TargetClass);

            var nameMethod = SimpleMethod("TypeName", "System.String", ClassName);
            TargetClass.Members.Add(nameMethod);

            if (typeDef.TypeFields != null)
            {
                for (int i = 0; i < typeDef.TypeFields.Length; i++) {
                    
                    NodeTypeField typeField = typeDef.TypeFields[i];
                    string fieldName = typeField.Name;
                    if (typeField.Name == null)
                    {
                        fieldName = typeDef.TypeFields.Length > 1 ? typeField.TypeName : "value";
                    }

                    string baseType = types[i];
                    var field = GetPropertyField(fieldName, baseType);
                    TargetClass.Members.Add(field);
                    TargetClass.Members.Add(GetProperty(fieldName, field)); }
            }

            CodeMemberMethod encodeMethod = GetEncode(typeDef.TypeFields);
            TargetClass.Members.Add(encodeMethod);

            CodeMemberMethod decodeMethod = GetDecode(typeDef.TypeFields, types);
            TargetClass.Members.Add(decodeMethod);
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

        public static StructGenBuilder Create(uint id, NodeTypeComposite typeDef, List<string> types)
        {
            return new StructGenBuilder(id, typeDef, types);
        }

        public string Build()
        {

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new()
            {
                BracingStyle = "C"
            };
            var path = Path.Combine("Model", "Types", "Composite", ClassName + ".cs");
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (StreamWriter sourceWriter = new(path))
            {
                provider.GenerateCodeFromCompileUnit(
                    TargetUnit, sourceWriter, options);
            }

            return ClassName;
        }
    }
}
