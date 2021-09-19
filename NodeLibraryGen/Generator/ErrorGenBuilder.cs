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
    public class ErrorGenBuilder : BaseBuilder
    {
        private ErrorGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
        {
            Success = true;
            Id = id;

            TargetUnit = new CodeCompileUnit();
            CodeNamespace importsNamespace = new()
            {
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

            CodeNamespace typeNamespace = new("SubstrateNetApi.Model.Custom.Errors");
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
                IsEnum = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };

            TargetClass.Comments.Add(new CodeCommentStatement("<summary>", true));
            TargetClass.Comments.Add(new CodeCommentStatement($">> Path: {fullPath}", true));
            foreach (var doc in typeDef.Docs)
            {
                TargetClass.Comments.Add(new CodeCommentStatement(doc, true));
            }
            TargetClass.Comments.Add(new CodeCommentStatement("</summary>", true));
            typeNamespace.Types.Add(TargetClass);


            if (typeDef.Variants != null)
            {
                foreach (var variant in typeDef.Variants)
                {
                    var enumField = new CodeMemberField(ClassName, variant.Name);
                    enumField.Comments.Add(new CodeCommentStatement("<summary>", true));
                    enumField.Comments.Add(new CodeCommentStatement($">> Event: {variant.Name}", true));
                    foreach (var doc in variant.Docs)
                    {
                        enumField.Comments.Add(new CodeCommentStatement(doc, true));
                    }
                    enumField.Comments.Add(new CodeCommentStatement("</summary>", true));
                    TargetClass.Members.Add(enumField);
                }
            }



        }

        public static ErrorGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
        {
            return new ErrorGenBuilder(id, typeDef, typeDict);
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
                var path = Path.Combine("Model", "Custom", "Errors", ClassName + ".cs");
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (StreamWriter sourceWriter = new(path))
                {
                    provider.GenerateCodeFromCompileUnit(
                        TargetUnit, sourceWriter, options);
                }
            }
            return ClassName + "Error";
        }
    }
}
