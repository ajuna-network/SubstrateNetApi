using Newtonsoft.Json;
using SubstrateNetApi;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Metadata.V14;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RuntimeMetadata
{
    partial class Program
    {
        private const string Websocketurl = "ws://127.0.0.1:9944";

        static async Task Main(string[] args)
        {
            string result;
            if (false)
            {
                using var client = new SubstrateClient(new Uri(Websocketurl));
                await client.ConnectLightAsync(CancellationToken.None);
                result = await client.State.GetMetaDataAsync(CancellationToken.None);
                File.WriteAllText("metadata_20210923.txt", result);
            }
            else
            {
                result = File.ReadAllText("metadata_20210923.txt");
            }
            var mdv14 = new SubstrateNetApi.Model.Types.Struct.RuntimeMetadata();
            mdv14.Create(result);

            //Console.WriteLine(mdv14);

            NodeMetadataV14 metadata = new()
            {
                Types = CreateNodeTypeDict(mdv14.RuntimeMetadataData.Types.Value),
                Modules = CreateModuleDict(mdv14.RuntimeMetadataData.Modules.Value),
                Extrinsic = CreateExtrinsic(mdv14.RuntimeMetadataData.Extrinsic),
                TypeId = (uint)mdv14.RuntimeMetadataData.TypeId.Value
            };

            // generate types
            var typeDict = GenerateTypes(metadata.Types);
            
            // generate modules
            GenerateModules(metadata.Modules, typeDict, metadata.Types);

            Console.WriteLine(JsonConvert.SerializeObject(typeDict, Formatting.Indented));

            Console.WriteLine($"Map: {typeDict.Count} vs. Tot: {metadata.Types.Count} {((double)typeDict.Count / metadata.Types.Count).ToString("P")}");

            //for (uint i = 0; i < nodeTypes.Keys.Max(); i++)
            //{
            //    if (nodeTypes.ContainsKey(i) && !typeDict.ContainsKey(i))
            //    {
            //        Console.WriteLine($"### {i} -------------------------------------------------");
            //        Console.WriteLine(JsonConvert.SerializeObject(nodeTypes[i], Formatting.Indented));
            //    }
            //}

            WriteJsonFile("metadata.json", metadata);
            //GenerateCode(nodeTypes);

        }

        private static void GenerateModules(Dictionary<uint, PalletModule> modules, Dictionary<uint, (string, List<string>)> typeDict, Dictionary<uint, NodeType> nodeTypes)
        {
            List<(string, List<string>)> moduleNames = new();
            foreach (var module in modules.Values) 
            {
                var moduleNameTuple = ModuleGenBuilder.Init(module.Index, module, typeDict, nodeTypes).Create().Build(write: true, out bool _);
                moduleNames.Add(moduleNameTuple);
            }

            ClientGenBuilder.Init(0, moduleNames, typeDict).Create().Build(write: true, out bool _);
        }

        private static ExtrinsicMetadata CreateExtrinsic(ExtrinsicMetadataStruct extrinsic)
        {
            return new ExtrinsicMetadata()
            {
                TypeId = (uint)extrinsic.ExtrinsicType.Value,
                Version = (int)extrinsic.Version.Value,
                SignedExtensions = extrinsic.SignedExtensions.Value.Select(p => new SignedExtensionMetadata()
                {
                    SignedIdentifier = p.SignedIdentifier.Value,
                    SignedExtType = (uint)p.SignedExtType.Value,
                    AddSignedExtType = (uint)p.AddSignedExtType.Value,
                }).ToArray()
            };
        }

        private static Dictionary<uint, (string, List<string>)> GenerateTypes(Dictionary<uint, NodeType> nodeTypes)
        {
            var typeDict = new Dictionary<uint, (string, List<string>)>();

            Dictionary<string, int> eventIndex = GetRuntimeIndex(nodeTypes, "node_runtime", "Event");

            Dictionary<string, int> callIndex = GetRuntimeIndex(nodeTypes, "node_runtime", "Call");

            var iterations = 10;

            for (int i = 0; i < iterations; i++)
            {

                for (uint id = 0; id < nodeTypes.Keys.Max(); id++)
                {
                    if (!nodeTypes.TryGetValue(id, out NodeType nodeType) || typeDict.ContainsKey(id))
                    {
                        continue;
                    }

                    switch (nodeType.TypeDef)
                    {
                        case TypeDefEnum.Composite:
                            {
                                var type = nodeType as NodeTypeComposite;
                                var fullItem = StructGenBuilder.Init(type.Id, type, typeDict)
                                    .Create()
                                    .Build(write: true, out bool success);
                                if (success)
                                {
                                    typeDict.Add(type.Id, fullItem);
                                }
                                break;
                            }
                        case TypeDefEnum.Variant:
                            {
                                var type = nodeType as NodeTypeVariant;
                                var variantType = GetVariantType(String.Join('.', nodeType.Path));
                                CallVariant(variantType, type, ref typeDict);
                                break;
                            }
                        case TypeDefEnum.Sequence:
                            {
                                var type = nodeType as NodeTypeSequence;
                                if (typeDict.TryGetValue(type.TypeId, out (string, List<string>) fullItem))
                                {
                                    var typeName = $"BaseVec<{fullItem.Item1}>";
                                    typeDict.Add(type.Id, (typeName, fullItem.Item2));
                                }
                                break;
                            }
                        case TypeDefEnum.Array:
                            {
                                var type = nodeType as NodeTypeArray;
                                var fullItem = ArrayGenBuilder.Create(type.Id, type, typeDict)
                                    .Create()
                                    .Build(write: true, out bool success);
                                if (success)
                                {
                                    typeDict.Add(type.Id, fullItem);
                                }
                                break;
                            }
                        case TypeDefEnum.Tuple:
                            {
                                var type = nodeType as NodeTypeTuple;
                                CallTuple(type, ref typeDict);
                                break;
                            }
                        case TypeDefEnum.Primitive:
                            {
                                var type = nodeType as NodeTypePrimitive;
                                CallPrimitive(type, ref typeDict);
                                break;
                            }
                        case TypeDefEnum.Compact:
                            {
                                var type = nodeType as NodeTypeCompact;
                                if (typeDict.TryGetValue(type.TypeId, out (string, List<string>) fullItem))
                                {
                                    var typeName = $"BaseCom<{fullItem.Item1}>";
                                    typeDict.Add(type.Id, (typeName, fullItem.Item2));
                                }
                                break;
                            }
                        case TypeDefEnum.BitSequence:
                        default:
                            throw new NotImplementedException($"Unimplemented enumeration of node type {nodeType.TypeDef}");
                    }
                }
            }
            
            return typeDict;
        }

        private static Dictionary<string, int> GetRuntimeIndex(Dictionary<uint, NodeType> nodeTypes, string runtime, string runtimeType)
        {
            var nodeType = nodeTypes.Select(p => p.Value).Where(p => p.Path != null && p.Path.Length == 2 && p.Path[0] == runtime && p.Path[1] == runtimeType).FirstOrDefault();
            if (nodeType is null or not NodeTypeVariant)
            {
                throw new Exception($"Node Index changed for {runtime}.{runtimeType} and {nodeType.GetType().Name}");
            }
            Dictionary<string, int> result = new();
            foreach (var variant in (nodeType as NodeTypeVariant).Variants)
            {
                result.Add(variant.Name, variant.Index);
            }
            return result;
        }

        private static void CallPrimitive(NodeTypePrimitive nodeType, ref Dictionary<uint, (string, List<string>)> typeDict)
        {
            List<string> spaces = new() { "SubstrateNetApi.Model.Types.Primitive" };
            var path = "SubstrateNetApi.Model.Types.Primitive.";
            switch (nodeType.Primitive)
            {
                case TypeDefPrimitive.Bool:
                    typeDict.Add(nodeType.Id, (path + nameof(Bool), spaces));
                    break;
                case TypeDefPrimitive.Char:
                    typeDict.Add(nodeType.Id, (path + nameof(PrimChar), spaces));
                    break;
                case TypeDefPrimitive.Str:
                    typeDict.Add(nodeType.Id, (path + nameof(Str), spaces));
                    break;
                case TypeDefPrimitive.U8:
                    typeDict.Add(nodeType.Id, (path + nameof(U8), spaces));
                    break;
                case TypeDefPrimitive.U16:
                    typeDict.Add(nodeType.Id, (path + nameof(U16), spaces));
                    break;
                case TypeDefPrimitive.U32:
                    typeDict.Add(nodeType.Id, (path + nameof(U32), spaces));
                    break;
                case TypeDefPrimitive.U64:
                    typeDict.Add(nodeType.Id, (path + nameof(U64), spaces));
                    break;
                case TypeDefPrimitive.U128:
                    typeDict.Add(nodeType.Id, (path + nameof(U128), spaces));
                    break;
                case TypeDefPrimitive.U256:
                    typeDict.Add(nodeType.Id, (path + nameof(U256), spaces));
                    break;
                case TypeDefPrimitive.I8:
                    typeDict.Add(nodeType.Id, (path + nameof(I8), spaces));
                    break;
                case TypeDefPrimitive.I16:
                    typeDict.Add(nodeType.Id, (path + nameof(I16), spaces));
                    break;
                case TypeDefPrimitive.I32:
                    typeDict.Add(nodeType.Id, (path + nameof(I32), spaces));
                    break;
                case TypeDefPrimitive.I64:
                    typeDict.Add(nodeType.Id, (path + nameof(I64), spaces));
                    break;
                case TypeDefPrimitive.I128:
                    typeDict.Add(nodeType.Id, (path + nameof(I128), spaces));
                    break;
                case TypeDefPrimitive.I256:
                    typeDict.Add(nodeType.Id, (path + nameof(I256), spaces));
                    break;
                default:
                    throw new NotImplementedException($"Please implement {nodeType.Primitive}, in SubstrateNetApi.");
            }
        }

        private static void CallTuple(NodeTypeTuple nodeType, ref Dictionary<uint, (string, List<string>)> typeDict)
        {
            var typeIds = new List<string>();
            var imports = new List<string>();
            for (int j = 0; j < nodeType.TypeIds.Length; j++)
            {
                var typeId = nodeType.TypeIds[j];
                if (!typeDict.TryGetValue(typeId, out (string, List<string>) fullItem))
                {
                    typeIds = null;
                    break;
                }
                imports.AddRange(fullItem.Item2);
                typeIds.Add(fullItem.Item1);
            }
            // all types found
            if (typeIds != null)
            {
                var typeName = $"BaseTuple{(typeIds.Count > 0 ? "<" + String.Join(',', typeIds.ToArray()) + ">" : "")}";
                typeDict.Add(nodeType.Id, (typeName, imports.Distinct().ToList()));
            }
        }

        private static void CallBitSequence(Dictionary<uint, NodeType> nodeTypes, ref Dictionary<uint, (string, List<string>)> typeDict)
        {
            for (uint i = 0; i < nodeTypes.Keys.Max(); i++)
            {
                if (!nodeTypes.TryGetValue(i, out NodeType nodeType) || typeDict.ContainsKey(i))
                {
                    continue;
                }

                if (nodeType.TypeDef == TypeDefEnum.BitSequence)
                {
                    var typeDef = nodeType as NodeTypeBitSequence;

                    throw new NotImplementedException("CallBitSequence");
                }
            }
        }

        private static void CallVariant(string variantType, NodeTypeVariant nodeType, ref Dictionary<uint, (string, List<string>)> typeDict)
        {
            switch (variantType)
            {
                case "Option":
                    {
                        if (typeDict.TryGetValue(nodeType.Variants[1].TypeFields[0].TypeId, out (string, List<string>) fullItem))
                        {
                            typeDict.Add(nodeType.Id, ($"BaseOpt<{fullItem.Item1}>", fullItem.Item2));
                        }
                        break;
                    }

                case "Result":
                    {
                        var spaces = new List<string>() { "SubstrateNetApi.Model.Types.Base" };
                        typeDict.Add(nodeType.Id, ($"BaseTuple<BaseTuple,  SubstrateNetApi.Model.SpRuntime.EnumDispatchError>", spaces));
                        break;
                    }

                case "Call":
                    {
                        var fullItem = CallGenBuilder.Init(nodeType.Id, nodeType, typeDict)
                            .Create()
                            // we don't write here only at the end in the modules ....
                            .Build(write: false, out bool success);
                        if (success)
                        {
                            typeDict.Add(nodeType.Id, fullItem);
                        }
                        break;
                    }

                case "Event":
                    {
                        var fullItem = EventGenBuilder.Init(nodeType.Id, nodeType, typeDict)
                            .Create()
                            // we don't write here only at the end in the modules ....
                            .Build(write: false, out bool success);
                        if (success)
                        {
                            typeDict.Add(nodeType.Id, fullItem);
                        }
                        break;
                    }

                case "Error":
                    {
                        var fullItem = ErrorGenBuilder.Init(nodeType.Id, nodeType, typeDict)
                            .Create()
                            // we don't write here only at the end in the modules ....
                            .Build(write: false, out bool success);
                        if (success)
                        {
                            typeDict.Add(nodeType.Id, fullItem);
                        }
                        break;
                    }

                case "Runtime":
                    {
                        var fullItem = RuntimeGenBuilder.Init(nodeType.Id, nodeType, typeDict)
                            .Create().Build(write: true, out bool success);
                        if (success)
                        {
                            typeDict.Add(nodeType.Id, fullItem);
                        }
                        break;
                    }

                case "Void":
                    {
                        var spaces = new List<string>() { "SubstrateNetApi.Model.Types.Base" };
                        typeDict.Add(nodeType.Id, ("SubstrateNetApi.Model.Types.Base.BaseVoid", spaces));
                        break;
                    }

                case "Enum":
                    {
                        var fullItem = EnumGenBuilder.Init(nodeType.Id, nodeType, typeDict)
                            .Create().Build(write: true, out bool success);
                        if (success)
                        {
                            typeDict.Add(nodeType.Id, fullItem);
                        }
                        break;
                    }

                default:
                        throw new NotImplementedException();
            }
        }

        private static string GetVariantType(string path)
        {
            if (path == "Option")
            {
                return path;
            }
            else if (path == "Result")
            {
                return path;
            }
            else if ((path.Contains("pallet_") || path.Contains(".pallet.")) && path.Contains(".Call"))
            {
                return "Call";
            }
            else if ((path.Contains("pallet_") || path.Contains(".pallet.")) && (path.Contains(".Event") || path.Contains(".RawEvent")))
            {
                return "Event";
            }
            else if ((path.Contains("pallet_") || path.Contains(".pallet.")) && path.Contains(".Error"))
            {
                return "Error";
            }
            else if (path.Contains("node_runtime.Event") || path.Contains("node_runtime.Call"))
            {
                return "Runtime";
            }
            else if (path.Contains(".Void"))
            {
                return "Void";
            }
            else
            {
                return "Enum";
            }
        }

        private static void WriteJsonFile(string fileName, NodeMetadataV14 runtimeMetadata)
        {
            var jsonFile = JsonConvert.SerializeObject(runtimeMetadata, Formatting.Indented);
            File.WriteAllText(fileName, jsonFile);
        }

        public static Dictionary<uint, NodeType> CreateNodeTypeDict(PortableType[] types)
        {
            var result = new Dictionary<uint, NodeType>();

            foreach (var type in types)
            {
                var path = type.Ty.Path.Value.Length == 0 ? null : type.Ty.Path.Value.Select(p => p.Value).ToArray();
                var typeParams = type.Ty.TypeParams.Value.Length == 0 ? null : type.Ty.TypeParams.Value.Select(p =>
                {
                    return new NodeTypeParam()
                    {
                        Name = p.TypeParameterName.Value,
                        TypeId = p.TypeParameterType.Value?.Value
                    };
                }).ToArray();
                var typeDefValue = type.Ty.TypeDef.Value;
                var docs = type.Ty.Docs == null || type.Ty.Docs.Value.Length == 0 ? null : type.Ty.Docs.Value.Select(p => p.Value).ToArray();

                NodeType nodeType = null;
                switch (typeDefValue)
                {
                    case TypeDefEnum.Composite:
                        {
                            var typeDef = type.Ty.TypeDef.Value2 as TypeDefComposite;
                            nodeType = new NodeTypeComposite()
                            {
                                Id = type.Id.Value,
                                Path = path,
                                TypeParams = typeParams,
                                TypeDef = typeDefValue,
                                TypeFields = typeDef.Fields.Value.Length == 0 ? null : typeDef.Fields.Value.Select(p =>
                                {
                                    var fDocs = p.Docs == null || p.Docs.Value.Length == 0 ? null : p.Docs.Value.Select(p => p.Value).ToArray();
                                    return new NodeTypeField()
                                    {
                                        Name = p.FieldName.Value?.Value,
                                        TypeName = p.FieldTypeName.Value?.Value,
                                        TypeId = p.FieldTy.Value,
                                        Docs = fDocs
                                    };
                                }).ToArray(),
                                Docs = docs
                            };
                        }
                        break;
                    case TypeDefEnum.Variant:
                        {
                            var typeDef = type.Ty.TypeDef.Value2 as TypeDefVariant;
                            nodeType = new NodeTypeVariant()
                            {
                                Id = type.Id.Value,
                                Path = path,
                                TypeParams = typeParams,
                                TypeDef = typeDefValue,
                                Variants = typeDef.TypeParam.Value.Length == 0 ? null : typeDef.TypeParam.Value.Select(p =>
                                {
                                    var vDocs = p.Docs == null || p.Docs.Value.Length == 0 ? null : p.Docs.Value.Select(p => p.Value).ToArray();
                                    return new TypeVariant()
                                    {
                                        Name = p.VariantName.Value,
                                        TypeFields = p.VariantFields.Value.Length == 0 ? null : p.VariantFields.Value.Select(p =>
                                        {
                                            var fDocs = p.Docs == null || p.Docs.Value.Length == 0 ? null : p.Docs.Value.Select(p => p.Value).ToArray();
                                            return new NodeTypeField()
                                            {
                                                Name = p.FieldName.Value?.Value,
                                                TypeName = p.FieldTypeName.Value?.Value,
                                                TypeId = p.FieldTy.Value,
                                                Docs = fDocs
                                            };
                                        }).ToArray(),
                                        Index = p.Index.Value,
                                        Docs = vDocs
                                    };
                                }).ToArray(),
                                Docs = docs
                            };
                        }
                        break;
                    case TypeDefEnum.Sequence:
                        {
                            var typeDef = type.Ty.TypeDef.Value2 as TypeDefSequence;
                            nodeType = new NodeTypeSequence()
                            {
                                Id = type.Id.Value,
                                Path = path,
                                TypeParams = typeParams,
                                TypeDef = typeDefValue,
                                TypeId = typeDef.TypeParam.Value,
                                Docs = docs
                            };
                        }
                        break;
                    case TypeDefEnum.Array:
                        {
                            var typeDef = type.Ty.TypeDef.Value2 as TypeDefArray;
                            nodeType = new NodeTypeArray()
                            {
                                Id = type.Id.Value,
                                Path = path,
                                TypeParams = typeParams,
                                TypeDef = typeDefValue,
                                TypeId = typeDef.TypeParam.Value,
                                Length = typeDef.Len.Value,
                                Docs = docs
                            };
                        }
                        break;
                    case TypeDefEnum.Tuple:
                        {
                            var typeDef = type.Ty.TypeDef.Value2 as TypeDefTuple;
                            nodeType = new NodeTypeTuple()
                            {
                                Id = type.Id.Value,
                                Path = path,
                                TypeParams = typeParams,
                                TypeDef = typeDefValue,
                                TypeIds = typeDef.Fields.Value.Select(p => (uint)p.Value).ToArray(),
                                Docs = docs
                            };
                        }
                        break;
                    case TypeDefEnum.Primitive:
                        {
                            var typeDef = (TypeDefPrimitive)Enum.Parse(typeof(TypeDefPrimitive), type.Ty.TypeDef.Value2.ToString());
                            nodeType = new NodeTypePrimitive()
                            {
                                Id = type.Id.Value,
                                Path = path,
                                TypeParams = typeParams,
                                TypeDef = typeDefValue,
                                Primitive = typeDef,
                                Docs = docs
                            };
                        }
                        break;
                    case TypeDefEnum.Compact:
                        {
                            var typeDef = type.Ty.TypeDef.Value2 as TypeDefCompact;
                            nodeType = new NodeTypeCompact()
                            {
                                Id = type.Id.Value,
                                Path = path,
                                TypeParams = typeParams,
                                TypeDef = typeDefValue,
                                TypeId = typeDef.TypeParam.Value,
                                Docs = docs
                            };
                        }
                        break;
                    case TypeDefEnum.BitSequence:
                        {
                            var typeDef = type.Ty.TypeDef.Value2 as TypeDefBitSequence;
                            nodeType = new NodeTypeBitSequence()
                            {
                                Id = type.Id.Value,
                                Path = path,
                                TypeParams = typeParams,
                                TypeDef = typeDefValue,
                                TypeIdOrder = typeDef.BitOrderType.Value,
                                TypeIdStore = typeDef.BitStoreType.Value,
                                Docs = docs
                            };
                        }
                        break;
                }

                if (nodeType != null)
                {
                    result.Add(nodeType.Id, nodeType);
                }
            }

            return result;
        }

        public static Dictionary<uint, PalletModule> CreateModuleDict(PalletMetadata[] modules)
        {
            var result = new Dictionary<uint, PalletModule>();

            foreach (var module in modules)
            {

                var palletModule = new PalletModule()
                {
                    Name = module.PalletName.Value,
                    Index = module.Index.Value,
                };
                result.Add(module.Index.Value, palletModule);

                if (module.PalletStorage.OptionFlag)
                {
                    var storage = module.PalletStorage.Value;
                    palletModule.Storage = new PalletStorage()
                    {
                        Prefix = storage.Prefix.Value,
                    };

                    palletModule.Storage.Entries = new Entry[storage.Entries.Value.Length];
                    for (int i = 0; i < storage.Entries.Value.Length; i++)
                    {
                        var entry = storage.Entries.Value[i];
                        palletModule.Storage.Entries[i] = new()
                        {
                            Name = entry.StorageName.Value,
                            Modifier = entry.StorageModifier.Value,
                            StorageType = entry.StorageType.Value,
                            Default = entry.StorageDefault.Value.Select(p => p.Value).ToArray(),
                            Docs = entry.Documentation.Value.Select(p => p.Value).ToArray(),
                        };

                        switch (entry.StorageType.Value)
                        {
                            case Storage.Type.Plain:
                                palletModule.Storage.Entries[i].TypeMap = (((TType)entry.StorageType.Value2).Value, null);
                                break;
                            case Storage.Type.Map:
                                var typeMap = ((StorageEntryTypeMap)entry.StorageType.Value2);
                                palletModule.Storage.Entries[i].TypeMap = (0, new TypeMap()
                                {
                                    Hashers = typeMap.Hashers.Value.Select(p => p.Value).ToArray(),
                                    Key = (uint)typeMap.Key.Value,
                                    Value = (uint)typeMap.Value.Value
                                });
                                break;
                            default:
                                throw new NotImplementedException();
                        }

                    }
                }

                if (module.PalletCalls.OptionFlag)
                {
                    var calls = module.PalletCalls.Value;
                    palletModule.Calls = new PalletCalls()
                    {
                        TypeId = (uint)calls.CallType.Value
                    };
                }

                if (module.PalletEvents.OptionFlag)
                {
                    var events = module.PalletEvents.Value;
                    palletModule.Events = new PalletEvents()
                    {
                        TypeId = (uint)events.EventType.Value
                    };
                }

                var constants = module.PalletConstants.Value;
                palletModule.Constants = new PalletConstant[constants.Length];
                for (int i = 0; i < constants.Length; i++)
                {
                    PalletConstantMetadata constant = constants[i];
                    palletModule.Constants[i] = new PalletConstant()
                    {
                        Name = constant.ConstantName.Value,
                        TypeId = (uint)constant.ConstantType.Value,
                        Value = constant.ConstantValue.Value.Select(p => p.Value).ToArray(),
                        Docs = constant.Documentation.Value.Select(p => p.Value).ToArray()

                    };
                }

                if (module.PalletErrors.OptionFlag)
                {
                    var errors = module.PalletErrors.Value;
                    palletModule.Errors = new PalletErrors()
                    {
                        TypeId = (uint)errors.ErrorType.Value
                    };
                }
            }

            return result;
        }

    }

}
