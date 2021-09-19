using Newtonsoft.Json;
using SubstrateNetApi.Model.Calls;
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
    public class CallGenBuilder : BaseBuilder
    {
        private CallGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
        {
            Success = true;
            Id = id;

            TargetUnit = new CodeCompileUnit();
            CodeNamespace importsNamespace = new() {
                Imports = {
                    new CodeNamespaceImport("SubstrateNetApi.Model.Calls"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Base"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Primitive"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.TypeDefArray"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.TypeDefComposite"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.TypeDefVariant"),
                    new CodeNamespaceImport("System.Collections.Generic"),
                    new CodeNamespaceImport("System")
                }
            };

            CodeNamespace typeNamespace = new("SubstrateNetApi.Model.Custom.Calls");
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

            typeNamespace.Types.Add(TargetClass);

            if (typeDef.Variants != null)
            {
                foreach (var variant in typeDef.Variants)
                {
                    CodeMemberMethod callMethod = new()
                    {
                        Attributes = MemberAttributes.Public | MemberAttributes.Final,
                        Name = variant.Name.MakeMethod(),
                        ReturnType = new CodeTypeReference(typeof(GenericExtrinsicCall).Name)
                    };

                    callMethod.Comments.Add(new CodeCommentStatement("<summary>", true));
                    callMethod.Comments.Add(new CodeCommentStatement($">> Extrinsic: {variant.Name}", true));
                    foreach(var doc in variant.Docs)
                    {
                        callMethod.Comments.Add(new CodeCommentStatement(doc, true));
                    }
                    //callMethod.Comments.Add(new CodeCommentStatement(
                    //    @"<para>Add a new paragraph to the description.</para>", true));
                    callMethod.Comments.Add(new CodeCommentStatement("</summary>", true));

                    var create = new CodeObjectCreateExpression(typeof(GenericExtrinsicCall).Name, Array.Empty<CodeExpression>());
                    create.Parameters.Add(new CodePrimitiveExpression(palletName));
                    create.Parameters.Add(new CodePrimitiveExpression(variant.Name));
                    
                    if (variant.TypeFields != null)
                    {
                        foreach (var field in variant.TypeFields)
                        {
                            if (!typeDict.TryGetValue(field.TypeId, out string baseType))
                            {
                                Success = false;
                                baseType = "Unknown";
                            }
                            CodeParameterDeclarationExpression param = new()
                            {
                                Type = new CodeTypeReference(baseType),
                                Name = field.Name
                            };
                            callMethod.Parameters.Add(param);

                            create.Parameters.Add(new CodeVariableReferenceExpression(field.Name));
                        }
                    }

                    CodeMethodReturnStatement returnStatement = new()
                    {
                        Expression = create
                    };

                    callMethod.Statements.Add(returnStatement);
                    TargetClass.Members.Add(callMethod);
                }
            }
        }

        public static CallGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
        {
            return new CallGenBuilder(id, typeDef, typeDict);
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
                var path = Path.Combine("Model", "Custom", "Calls", ClassName + ".cs");
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
