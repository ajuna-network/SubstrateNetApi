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
    public class EnumGenBuilder : BaseBuilder
    {
        private EnumGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            Success = true;
            Id = id;

            TargetUnit = new CodeCompileUnit();
            TargetUnit.Namespaces.Add(ImportsNamespace);

            var enumName = $"{typeDef.Path.Last()}";

            if (typeDef.Path[0].Contains("_"))
            {
                NameSpace = "SubstrateNetApi.Model." + typeDef.Path[0].MakeMethod();
            }
            else
            {
                NameSpace = "SubstrateNetApi.Model." + "Base";
            }

            ClassName = $"Enum{enumName}";
            ReferenzName = $"{NameSpace}.{ClassName}";
            CodeNamespace typeNamespace = new(NameSpace);
            TargetUnit.Namespaces.Add(typeNamespace);

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

            if (typeDef.Variants != null)
            {
                TargetClass = new CodeTypeDeclaration(ClassName)
                {
                    IsClass = true,
                    TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                };
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

                if (typeDef.Variants.All(p => p.TypeFields == null))
                {
                    TargetClass.BaseTypes.Add(new CodeTypeReference($"BaseEnum<{enumName}>"));
                    typeNamespace.Types.Add(TargetClass);
                }
                else
                {
                    var codeTypeRef = new CodeTypeReference("BaseEnumExt");
                    codeTypeRef.TypeArguments.Add(new CodeTypeReference(enumName));
                    foreach (TypeVariant variant in typeDef.Variants)
                    {
                        if (variant.TypeFields == null)
                        {
                            codeTypeRef.TypeArguments.Add(new CodeTypeReference("BaseVoid"));
                            //ImportsNamespace.Imports.Add(new CodeNamespaceImport("SubstrateNetApi.Model.Types"));
                        }
                        else
                        {
                            if (variant.TypeFields.Length == 1)
                            {
                                if (typeDict.TryGetValue(variant.TypeFields[0].TypeId, out (string, List<string>) fullItem))
                                {
                                    codeTypeRef.TypeArguments.Add(new CodeTypeReference(fullItem.Item1));
                                    //fullItem.Item2.ForEach(p => ImportsNamespace.Imports.Add(new CodeNamespaceImport(p)));
                                }
                                else
                                {
                                    Success = false;
                                }
                            }
                            else
                            {
                                var baseTuple = new CodeTypeReference("BaseTuple");
                                foreach (var field in variant.TypeFields)
                                {
                                    if (typeDict.TryGetValue(field.TypeId, out (string, List<string>) fullItem))
                                    {
                                        baseTuple.TypeArguments.Add(new CodeTypeReference(fullItem.Item1));
                                        //fullItem.Item2.ForEach(p => ImportsNamespace.Imports.Add(new CodeNamespaceImport(p)));
                                    }
                                    else
                                    {
                                        Success = false;
                                    }
                                }
                                codeTypeRef.TypeArguments.Add(baseTuple);
                            }

                        }
                    }
                    TargetClass.BaseTypes.Add(codeTypeRef);
                    typeNamespace.Types.Add(TargetClass);
                }
            }
        }

        public static EnumGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new EnumGenBuilder(id, typeDef, typeDict);
        }
    }
}
