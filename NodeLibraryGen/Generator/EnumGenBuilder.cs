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
            : base(id, typeDef, typeDict)
        {
        }

        public static EnumGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new EnumGenBuilder(id, typeDef, typeDict);
        }

        public override BaseBuilder Create()
        {
            var typeDef = TypeDef as NodeTypeVariant;

            #region CREATE

            var enumName = $"{typeDef.Path.Last()}";

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
                TargetClass.Comments.AddRange(GetComments(typeDef.Docs, typeDef));

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
                            // add void type
                            codeTypeRef.TypeArguments.Add(new CodeTypeReference("BaseVoid"));
                        }
                        else
                        {
                            if (variant.TypeFields.Length == 1)
                            {
                                var fullItem = GetFullItemPath(variant.TypeFields[0].TypeId);
                                codeTypeRef.TypeArguments.Add(new CodeTypeReference(fullItem.Item1));
                            }
                            else
                            {
                                var baseTuple = new CodeTypeReference("BaseTuple");
                                foreach (var field in variant.TypeFields)
                                {
                                    var fullItem = GetFullItemPath(field.TypeId);
                                    baseTuple.TypeArguments.Add(new CodeTypeReference(fullItem.Item1));
                                }
                                codeTypeRef.TypeArguments.Add(baseTuple);
                            }
                        }
                    }
                    TargetClass.BaseTypes.Add(codeTypeRef);
                    typeNamespace.Types.Add(TargetClass);
                }
            }
            #endregion

            return this;
        }
    }
}
