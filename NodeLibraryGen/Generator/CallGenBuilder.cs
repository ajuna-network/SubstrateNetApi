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

namespace RuntimeMetadata
{
    public class CallGenBuilder : TypeBuilder
    {
        private CallGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict) 
            : base(id, typeDef, typeDict)
        {
        }

        public static CallGenBuilder Init(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new CallGenBuilder(id, typeDef, typeDict);
        }

        public override TypeBuilder Create()
        {
            var typeDef = TypeDef as NodeTypeVariant;

            #region CREATE

            ImportsNamespace.Imports.Add(new CodeNamespaceImport("SubstrateNetApi.Model.Calls"));
            
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

            // add comment to class if exists
            TargetClass.Comments.AddRange(GetComments(typeDef.Docs, typeDef));

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

                    // add comment to class if exists
                    callMethod.Comments.AddRange(GetComments(typeDef.Docs, null, variant.Name));

                    var create = new CodeObjectCreateExpression(typeof(GenericExtrinsicCall).Name, Array.Empty<CodeExpression>());
                    create.Parameters.Add(new CodePrimitiveExpression(palletName));
                    create.Parameters.Add(new CodePrimitiveExpression(variant.Name));

                    if (variant.TypeFields != null)
                    {
                        foreach (var field in variant.TypeFields)
                        {
                            var fullItem = GetFullItemPath(field.TypeId);

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

            #endregion

            return this;
        }

    }
}
