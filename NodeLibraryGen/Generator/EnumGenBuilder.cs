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
        private bool _success = true;

        private EnumGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, string> typeDict)
        {
            Id = id;

            TargetUnit = new CodeCompileUnit();
            CodeNamespace importsNamespace = new() {
                Imports = {
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.TypeDefBase"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Primitive"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.TypeDefArray"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.TypeDefComposite"),
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.TypeDefVariant"),
                    new CodeNamespaceImport("System.Collections.Generic"),
                    new CodeNamespaceImport("System")
                }
            };

            CodeNamespace typeNamespace = new("SubstrateNetApi.Model.Types.TypeDefVariant");
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
                            codeTypeRef.TypeArguments.Add(new CodeTypeReference("NullType"));
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
                                    _success = false;
                                }

                            }
                            else
                            {
                                //Console.WriteLine($"fucking {Id} extenum shizzle {variant.TypeFields.Length}");
                                _success = false;
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
            success = _success;
            if (success)
            {
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CodeGeneratorOptions options = new()
                {
                    BracingStyle = "C"
                };
                var path = Path.Combine("Model", "Types", "TypeDefVariant", ClassName + ".cs");
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
