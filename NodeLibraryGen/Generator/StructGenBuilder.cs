using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RuntimeMetadata
{
    public class StructGenBuilder : TypeBuilder
    {
        private StructGenBuilder(uint id, NodeTypeComposite typeDef, Dictionary<uint, (string, List<string>)> typeDict) 
            : base(id, typeDef, typeDict)
        {
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

        private CodeMemberMethod GetDecode(NodeTypeField[] typeFields)
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
                for (uint i = 0; i < typeFields.Length; i++)
                {
                    NodeTypeField typeField = typeFields[i];

                    string fieldName = typeField.Name;
                    if (typeField.Name == null)
                    {
                        fieldName = typeFields.Length > 1 ? typeField.TypeName : "value";
                    }
                    var fullItem = GetFullItemPath(typeField.TypeId);

                    decodeMethod.Statements.Add(new CodeSnippetExpression($"{fieldName.MakeMethod()} = new {fullItem.Item1}()"));
                    decodeMethod.Statements.Add(new CodeSnippetExpression($"{fieldName.MakeMethod()}.Decode(byteArray, ref p)"));
                }
            }

            decodeMethod.Statements.Add(new CodeSnippetExpression("TypeSize = p - start"));
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
                    }

                    encodeMethod.Statements.Add(new CodeSnippetExpression($"result.AddRange({fieldName.MakeMethod()}.Encode())"));
                }
            }

            encodeMethod.Statements.Add(new CodeSnippetExpression("return result.ToArray()"));
            return encodeMethod;
        }

        public static BaseBuilder Init(uint id, NodeTypeComposite typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new StructGenBuilder(id, typeDef, typeDict);
        }

        public override TypeBuilder Create()
        {
            var typeDef = TypeDef as NodeTypeComposite;

            #region CREATE

            ClassName = $"{typeDef.Path.Last()}";

            ReferenzName = $"{NameSpace}.{typeDef.Path.Last()}";

            CodeNamespace typeNamespace = new(NameSpace);
            TargetUnit.Namespaces.Add(typeNamespace);

            TargetClass = new CodeTypeDeclaration(ClassName)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };
            TargetClass.BaseTypes.Add(new CodeTypeReference("BaseType"));

            // add comment to class if exists
            TargetClass.Comments.AddRange(GetComments(typeDef.Docs, typeDef));

            typeNamespace.Types.Add(TargetClass);

            var nameMethod = SimpleMethod("TypeName", "System.String", ClassName);
            TargetClass.Members.Add(nameMethod);

            if (typeDef.TypeFields != null)
            {
                for (uint i = 0; i < typeDef.TypeFields.Length; i++)
                {

                    NodeTypeField typeField = typeDef.TypeFields[i];
                    string fieldName = typeField.Name;
                    if (typeField.Name == null)
                    {
                        fieldName = typeDef.TypeFields.Length > 1 ? typeField.TypeName : "value";
                    }

                    var fullItem = GetFullItemPath(typeField.TypeId);

                    var field = GetPropertyField(fieldName, fullItem.Item1);
                    
                    // add comment to field if exists
                    field.Comments.AddRange(GetComments(typeField.Docs, null, fieldName));

                    TargetClass.Members.Add(field);
                    TargetClass.Members.Add(GetProperty(fieldName, field));
                }
            }

            CodeMemberMethod encodeMethod = GetEncode(typeDef.TypeFields);
            TargetClass.Members.Add(encodeMethod);

            CodeMemberMethod decodeMethod = GetDecode(typeDef.TypeFields);
            TargetClass.Members.Add(decodeMethod);

            #endregion

            return this;
        }
    }
}
