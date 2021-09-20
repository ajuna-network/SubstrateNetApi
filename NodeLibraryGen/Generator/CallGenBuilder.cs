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
        private CallGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            Success = true;
            Id = id;

            TargetUnit = new CodeCompileUnit();
            ImportsNamespace.Imports.Add(new CodeNamespaceImport("SubstrateNetApi.Model.Calls"));
            TargetUnit.Namespaces.Add(ImportsNamespace);

            if (typeDef.Path[0].Contains("_"))
            {
                NameSpace = "SubstrateNetApi.Model." + typeDef.Path[0].MakeMethod();
            }
            else
            {
                NameSpace = "SubstrateNetApi.Model." + "Base";
            }

            ClassName = typeDef.Path[0].MakeMethod() + "Call";
            ReferenzName = $"{NameSpace}.{ClassName}";
            CodeNamespace typeNamespace = new(NameSpace);
            TargetUnit.Namespaces.Add(typeNamespace);

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
            TargetClass.Comments.Add(new CodeCommentStatement($">> Path: {String.Join('.', typeDef.Path)}", true));
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
                            if (!typeDict.TryGetValue(field.TypeId, out (string, List<string>) fullItem))
                            {
                                Success = false;
                                fullItem = ("Unknown", new List<string>() { "Unknown" });
                            }
                            //fullItem.Item2.ForEach(p => ImportsNamespace.Imports.Add(new CodeNamespaceImport(p)));
                            CodeParameterDeclarationExpression param = new()
                            {
                                Type = new CodeTypeReference(fullItem.Item1),
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

        public static CallGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new CallGenBuilder(id, typeDef, typeDict);
        }
    }
}
