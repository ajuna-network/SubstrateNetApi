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
    public class RuntimeGenBuilder : BaseBuilder
    {
        private RuntimeGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
        {
            Success = true;
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

            CodeNamespace typeNamespace = new("SubstrateNetApi.Model.Custom.Runtime");
            TargetUnit.Namespaces.Add(importsNamespace);
            TargetUnit.Namespaces.Add(typeNamespace);

            var fullPath = $"{String.Join('.', typeDef.Path)}";

            var runtimeType = $"{typeDef.Path.Last()}";
            var enumName = $"Node{runtimeType}";
            ClassName = $"Enum{enumName}";

            CodeTypeDeclaration TargetType = new(enumName)
            {
                IsEnum = true
            };

            if (typeDef.Variants != null) 
            { 
                foreach (var enumFieldName in typeDef.Variants.Select(p => p.Name))
                {
                    TargetType.Members.Add(new CodeMemberField(ClassName, enumFieldName));
                }
            }
            typeNamespace.Types.Add(TargetType);

            TargetClass = new CodeTypeDeclaration(ClassName)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };
            TargetClass.BaseTypes.Add(new CodeTypeReference($"BaseEnum<{enumName}>"));
            TargetClass.Comments.Add(new CodeCommentStatement("<summary>", true));
            TargetClass.Comments.Add(new CodeCommentStatement($">> Enum", true));
            if (typeDef.Docs != null)
            {
                foreach (var doc in typeDef.Docs)
                {
                    TargetClass.Comments.Add(new CodeCommentStatement(doc, true));
                }
            }
            TargetClass.Comments.Add(new CodeCommentStatement("</summary>", true));

            typeNamespace.Types.Add(TargetClass);

        }

        public static RuntimeGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
        {
            return new RuntimeGenBuilder(id, typeDef, typeDict);
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
                var path = Path.Combine("Model", "Custom", "Runtime", ClassName + ".cs");
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
