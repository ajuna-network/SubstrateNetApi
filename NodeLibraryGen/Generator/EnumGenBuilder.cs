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
        private EnumGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
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

            CodeNamespace typeNamespace = new("SubstrateNetApi.Model.Types.Enum");
            TargetUnit.Namespaces.Add(importsNamespace);
            TargetUnit.Namespaces.Add(typeNamespace);

            var enumName = $"{typeDef.Path.Last()}";

            ClassName = $"Enum{enumName}";

            CodeTypeDeclaration TargetType = new CodeTypeDeclaration(enumName)
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
                if (typeDef.Variants.All(p => p.TypeFields == null))
                {
                    TargetClass = new CodeTypeDeclaration(ClassName)
                    {
                        IsClass = true,
                        TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                    };
                    TargetClass.BaseTypes.Add(new CodeTypeReference($"BaseEnum<{enumName}>"));
                    typeNamespace.Types.Add(TargetClass);
                }
                else
                {
                    TargetClass = new CodeTypeDeclaration(ClassName)
                    {
                        IsClass = true,
                        TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                    };

                    var codeTypeRef = new CodeTypeReference("BaseEnumExt");
                    codeTypeRef.TypeArguments.Add(new CodeTypeReference(enumName));
                    foreach (TypeVariant variant in typeDef.Variants)
                    {
                        if (variant.TypeFields == null)
                        {
                            codeTypeRef.TypeArguments.Add(new CodeTypeReference("Void"));
                        }
                        else
                        {
                            if (variant.TypeFields.Length == 1)
                            {
                                if (typeDict.TryGetValue(variant.TypeFields[0].TypeId, out string baseType))
                                {
                                    codeTypeRef.TypeArguments.Add(new CodeTypeReference(baseType));
                                }
                                else
                                {
                                    //codeTypeRef.TypeArguments.Add(new CodeTypeReference("FuckYou"));
                                    Success = false;
                                }

                            }
                            else
                            {
                                var baseTuple = new CodeTypeReference("BaseTuple");
                                foreach (var field in variant.TypeFields)
                                {
                                    if (typeDict.TryGetValue(field.TypeId, out string baseType))
                                    {
                                        baseTuple.TypeArguments.Add(new CodeTypeReference(baseType));
                                    }
                                    else
                                    {
                                        //codeTypeRef.TypeArguments.Add(new CodeTypeReference("FuckYou"));
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

        public static EnumGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
        {
            return new EnumGenBuilder(id, typeDef, typeDict);
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
                var path = Path.Combine("Model", "Types", "Enum", ClassName + ".cs");
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
