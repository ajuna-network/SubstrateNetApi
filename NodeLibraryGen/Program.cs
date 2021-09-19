using Newtonsoft.Json;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Metadata.V14;
using SubstrateNetApi.Model.Types.TypeDefPrimitive;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NodeLibraryGen
{

    class Program
    {
        private const string Websocketurl = "ws://127.0.0.1:9944";

        static async Task Main(string[] args)
        {
            //using var client = new SubstrateClient(new Uri(Websocketurl));
            //await client.ConnectLightAsync(CancellationToken.None);
            //var result = await client.State.GetMetaDataAsync(CancellationToken.None);

            string result = File.ReadAllText("metadata.txt");

            var mdv14 = new RuntimeMetadata();
            mdv14.Create(result);

            //Console.WriteLine(mdv14);

            var nodeTypes = CreateNodeTypeDict(mdv14.RuntimeMetadataData.Types.Value);

            var typeDict = CreateMapping(nodeTypes);

            Console.WriteLine(JsonConvert.SerializeObject(typeDict, Formatting.Indented));

            Console.WriteLine($"{((double)typeDict.Count / nodeTypes.Count).ToString("P")}");

            //WriteJsonFile("metadata.json", nodeTypes);
            //GenerateCode(nodeTypes);

        }

        private static Dictionary<uint, string> CreateMapping(Dictionary<uint, NodeType> nodeTypes)
        {
            var typeDict = new Dictionary<uint, string>();

            CallPrimitive(nodeTypes, ref typeDict);

            for (int i = 0; i < 10; i++)
            {
                CallArray(nodeTypes, ref typeDict);
                CallTuple(nodeTypes, ref typeDict);
                CallSequence(nodeTypes, ref typeDict);
                CallCompact(nodeTypes, ref typeDict);
                CallComposite(nodeTypes, ref typeDict);
                CallVariant(nodeTypes, ref typeDict);
            }

            return typeDict;
        }

        private static void CallPrimitive(Dictionary<uint, NodeType> nodeTypes, ref Dictionary<uint, string> typeDict)
        {
            for (uint i = 0; i < nodeTypes.Keys.Max(); i++)
            {
                if (!nodeTypes.TryGetValue(i, out NodeType nodeType) || typeDict.ContainsKey(i))
                {
                    continue;
                }

                if (nodeType.TypeDef == TypeDefEnum.Primitive)
                {
                    var typeDef = nodeType as NodeTypePrimitive;
                    switch (typeDef.Primitive)
                    {
                        case TypeDefPrimitive.Bool:
                            typeDict.Add(i, typeof(PrimBool).Name);
                            break;
                        case TypeDefPrimitive.Char:
                            typeDict.Add(i, typeof(PrimChar).Name);
                            break;
                        case TypeDefPrimitive.Str:
                            typeDict.Add(i, typeof(PrimStr).Name);
                            break;
                        case TypeDefPrimitive.U8:
                            typeDict.Add(i, typeof(PrimU8).Name);
                            break;
                        case TypeDefPrimitive.U16:
                            typeDict.Add(i, typeof(PrimU16).Name);
                            break;
                        case TypeDefPrimitive.U32:
                            typeDict.Add(i, typeof(PrimU32).Name);
                            break;
                        case TypeDefPrimitive.U64:
                            typeDict.Add(i, typeof(PrimU64).Name);
                            break;
                        case TypeDefPrimitive.U128:
                            typeDict.Add(i, typeof(PrimU128).Name);
                            break;
                        case TypeDefPrimitive.U256:
                            typeDict.Add(i, typeof(PrimU256).Name);
                            break;
                        case TypeDefPrimitive.I8:
                            typeDict.Add(i, typeof(PrimI8).Name);
                            break;
                        case TypeDefPrimitive.I16:
                            typeDict.Add(i, typeof(PrimI16).Name);
                            break;
                        case TypeDefPrimitive.I32:
                            typeDict.Add(i, typeof(PrimI32).Name);
                            break;
                        case TypeDefPrimitive.I64:
                            typeDict.Add(i, typeof(PrimI64).Name);
                            break;
                        case TypeDefPrimitive.I128:
                            typeDict.Add(i, typeof(PrimI128).Name);
                            break;
                        case TypeDefPrimitive.I256:
                            typeDict.Add(i, typeof(PrimI128).Name);
                            break;
                        default:
                            throw new NotImplementedException($"Please implement {typeDef.Primitive}, in SubstrateNetApi.");
                    }
                }
            }
        }

        private static void CallArray(Dictionary<uint, NodeType> nodeTypes, ref Dictionary<uint, string> typeDict)
        {
            for (uint i = 0; i < nodeTypes.Keys.Max(); i++)
            {
                if (!nodeTypes.TryGetValue(i, out NodeType nodeType) || typeDict.ContainsKey(i))
                {
                    continue;
                }

                if (nodeType.TypeDef == TypeDefEnum.Array)
                {
                    var typeDef = nodeType as NodeTypeArray;

                    if (typeDict.TryGetValue(typeDef.TypeId, out string baseType))
                    {
                        if (nodeType.Path != null && nodeType.Path.Length > 0)
                        {
                            Console.WriteLine($"{nodeType.GetType().Name} has path {String.Join('.', nodeType.Path)}, please check!");
                        }

                        if (nodeType.TypeParams != null && nodeType.TypeParams.Length > 0)
                        {
                            Console.WriteLine($"{nodeType.GetType().Name} has TypeParams, please check!");
                        }

                        var typeName = ArrayGenBuilder.Create(i, baseType, typeDef.Length).Build();
                        typeDict.Add(i, typeName);
                    }
                }
            }
        }

        private static void CallTuple(Dictionary<uint, NodeType> nodeTypes, ref Dictionary<uint, string> typeDict)
        {
            for (uint i = 0; i < nodeTypes.Keys.Max(); i++)
            {
                if (!nodeTypes.TryGetValue(i, out NodeType nodeType) || typeDict.ContainsKey(i))
                {
                    continue;
                }

                if (nodeType.TypeDef == TypeDefEnum.Tuple)
                {
                    var typeDef = nodeType as NodeTypeTuple;

                    if (nodeType.Path != null && nodeType.Path.Length > 0)
                    {
                        Console.WriteLine($"{nodeType.GetType().Name} has path {String.Join('.', nodeType.Path)}, please check!");
                    }

                    if (nodeType.TypeParams != null && nodeType.TypeParams.Length > 0)
                    {
                        Console.WriteLine($"{nodeType.GetType().Name} has TypeParams, please check!");
                    }

                    if (typeDef.TypeIds.Length > 7)
                    {
                        Console.WriteLine($"{nodeType.GetType().Name} has more {typeDef.TypeIds.Length}, parts!");
                    }

                    var typeIds = new List<string>();
                    for (int j = 0; j < typeDef.TypeIds.Length; j++)
                    {
                        var typeId = typeDef.TypeIds[j];
                        if (!typeDict.TryGetValue(typeId, out string value))
                        {
                            typeIds = null;
                            break;
                        }

                        typeIds.Add(value);
                    }
                    // all types found
                    if (typeIds != null)
                    {
                        var typeName = $"BaseTuple{(typeIds.Count > 0 ? "<" + String.Join(',', typeIds.ToArray()) + ">" : "")}";
                        typeDict.Add(i, typeName);
                    }
                }
            }
        }

        private static void CallSequence(Dictionary<uint, NodeType> nodeTypes, ref Dictionary<uint, string> typeDict)
        {
            for (uint i = 0; i < nodeTypes.Keys.Max(); i++)
            {
                if (!nodeTypes.TryGetValue(i, out NodeType nodeType) || typeDict.ContainsKey(i))
                {
                    continue;
                }

                if (nodeType.TypeDef == TypeDefEnum.Sequence)
                {
                    var typeDef = nodeType as NodeTypeSequence;

                    if (nodeType.Path != null && nodeType.Path.Length > 0)
                    {
                        Console.WriteLine($"{nodeType.GetType().Name} has path {String.Join('.', nodeType.Path)}, please check!");
                    }

                    if (nodeType.TypeParams != null && nodeType.TypeParams.Length > 0)
                    {
                        Console.WriteLine($"{nodeType.GetType().Name} has TypeParams, please check!");
                    }

                    if (typeDict.TryGetValue(typeDef.TypeId, out string value))
                    {
                        var typeName = $"BaseVec<{value}>";
                        typeDict.Add(i, typeName);
                    }
                }
            }
        }

        private static void CallCompact(Dictionary<uint, NodeType> nodeTypes, ref Dictionary<uint, string> typeDict)
        {
            for (uint i = 0; i < nodeTypes.Keys.Max(); i++)
            {
                if (!nodeTypes.TryGetValue(i, out NodeType nodeType) || typeDict.ContainsKey(i))
                {
                    continue;
                }

                if (nodeType.TypeDef == TypeDefEnum.Compact)
                {
                    var typeDef = nodeType as NodeTypeCompact;

                    if (nodeType.Path != null && nodeType.Path.Length > 0)
                    {
                        Console.WriteLine($"{nodeType.GetType().Name} has path {String.Join('.', nodeType.Path)}, please check!");
                    }

                    if (nodeType.TypeParams != null && nodeType.TypeParams.Length > 0)
                    {
                        Console.WriteLine($"{nodeType.GetType().Name} has TypeParams, please check!");
                    }

                    if (typeDict.TryGetValue(typeDef.TypeId, out string value))
                    {
                        var typeName = $"BaseCom<{value}>";
                        typeDict.Add(i, typeName);
                    }
                }
            }
        }

        private static void CallComposite(Dictionary<uint, NodeType> nodeTypes, ref Dictionary<uint, string> typeDict)
        {
            for (uint i = 0; i < nodeTypes.Keys.Max(); i++)
            {
                if (!nodeTypes.TryGetValue(i, out NodeType nodeType) || typeDict.ContainsKey(i))
                {
                    continue;
                }

                if (nodeType.TypeDef == TypeDefEnum.Composite)
                {
                    var typeDef = nodeType as NodeTypeComposite;

                    if (nodeType.Path == null || nodeType.Path.Length == 0)
                    {
                        Console.WriteLine($"{nodeType.GetType().Name} has no path, please check!");
                    }

                    bool satisfied = true;
                    List<string> types = new();
                    if (typeDef.TypeFields != null)
                    {
                        foreach (var field in typeDef.TypeFields)
                        {
                            if (!typeDict.TryGetValue(field.TypeId, out string value))
                            {
                                satisfied = false;
                                break;
                            }

                            types.Add(value);
                        }
                    }
                    if (satisfied)
                    {
                        var typeName = StructGenBuilder.Create(i, typeDef, types).Build();
                        typeDict.Add(i, typeName);
                    }
                }
            }
        }

        private static void CallBitSequence(Dictionary<uint, NodeType> nodeTypes, ref Dictionary<uint, string> typeDict)
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

        private static void CallVariant(Dictionary<uint, NodeType> nodeTypes, ref Dictionary<uint, string> typeDict)
        {
            for (uint i = 0; i < nodeTypes.Keys.Max(); i++)
            {
                if (!nodeTypes.TryGetValue(i, out NodeType nodeType) || typeDict.ContainsKey(i))
                {
                    continue;
                }

                if (nodeType.TypeDef == TypeDefEnum.Variant)
                {
                    var typeDef = nodeType as NodeTypeVariant;

                    var path = String.Join('.', typeDef.Path);
                    //typeDict.Add(i, $"{String.Join('.', typeDef.Path)}");

                    if (path == "Option")
                    {
                        if (typeDict.TryGetValue(typeDef.Variants[1].TypeFields[0].TypeId, out string optType))
                        {
                            typeDict.Add(i, $"BaseOpt<{optType}>");
                        }
                    }
                    else if (path == "Result")
                    {
                        //Console.WriteLine($"{i} --> {String.Join('.', typeDef.Path)}");
                    }
                    else if (path.Contains("pallet_") && path.Contains(".Call"))
                    {
                        var typeName = CallGenBuilder.Create(i, typeDef, typeDict).Build(out bool success);
                        if (success)
                        {
                            typeDict.Add(i, typeName);
                        }

                    }
                    else if (path.Contains("node_runtime.Call") || path.Contains(".pallet.Call"))
                    {
                        Console.WriteLine($"{i} --> {String.Join('.', typeDef.Path)}");
                    }
                    else if (path.Contains("node_runtime.Event") || path.Contains(".pallet.Event") || path.Contains("pallet_") && path.Contains(".Event"))
                    {
                        //Console.WriteLine($"{i} --> {String.Join('.', typeDef.Path)}");
                    }
                    else if (path.Contains(".pallet.Error") || path.Contains("pallet_") && path.Contains(".Error"))
                    {
                        //Console.WriteLine($"{i} --> {String.Join('.', typeDef.Path)}");
                    }
                    else if (path.Contains("pallet_"))
                    {
                        //Console.WriteLine($"{i} --> {String.Join('.', typeDef.Path)}");
                        var typeName = EnumGenBuilder.Create(i, typeDef, typeDict).Build(out bool success);
                        if (success)
                        {
                            typeDict.Add(i, typeName);
                        }
                    }
                    else if (path.Contains(".Void"))
                    {
                        //Console.WriteLine($"{i} --> {String.Join('.', typeDef.Path)}");
                        var typeName = EnumGenBuilder.Create(i, typeDef, typeDict).Build(out bool success);
                        if (success)
                        {
                            typeDict.Add(i, typeName);
                        }
                    }
                    else
                    {
                        //Console.WriteLine($"{i} --> {String.Join('.', typeDef.Path)}");
                        var typeName = EnumGenBuilder.Create(i, typeDef, typeDict).Build(out bool success);
                        if (success)
                        {
                            typeDict.Add(i, typeName);
                        }
                    }

                }
            }
        }

        private static void WriteJsonFile(string fileName, Dictionary<uint, NodeType> nodeTypes)
        {
            var jsonFile = JsonConvert.SerializeObject(nodeTypes, Formatting.Indented);
            File.WriteAllText(fileName, jsonFile);
        }

        public static Dictionary<uint, NodeType> CreateNodeTypeDict(List<PortableType> types)
        {
            var result = new Dictionary<uint, NodeType>();

            foreach (var type in types)
            {
                var path = type.Ty.Path.Value.Count == 0 ? null : type.Ty.Path.Value.Select(p => p.Value).ToArray();
                var typeParams = type.Ty.TypeParams.Value.Count == 0 ? null : type.Ty.TypeParams.Value.Select(p =>
                {
                    return new NodeTypeParam()
                    {
                        Name = p.TypeParameterName.Value,
                        TypeId = p.TypeParameterType.Value?.Value
                    };
                }).ToArray();
                var typeDefValue = type.Ty.TypeDef.Value;
                var docs = type.Ty.Docs == null || type.Ty.Docs.Value.Count == 0 ? null : type.Ty.Docs.Value.Select(p => p.Value).ToArray();

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
                                TypeFields = typeDef.Fields.Value.Count == 0 ? null : typeDef.Fields.Value.Select(p =>
                                {
                                    return new NodeTypeField()
                                    {
                                        Name = p.FieldName.Value?.Value,
                                        TypeName = p.FieldTypeName.Value?.Value,
                                        TypeId = p.FieldTy.Value
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
                                Variants = typeDef.TypeParam.Value.Count == 0 ? null : typeDef.TypeParam.Value.Select(p =>
                                {
                                    return new TypeVariant()
                                    {
                                        Name = p.VariantName.Value,
                                        TypeFields = p.VariantFields.Value.Count == 0 ? null : p.VariantFields.Value.Select(p =>
                                        {
                                            return new NodeTypeField()
                                            {
                                                Name = p.FieldName.Value?.Value,
                                                TypeName = p.FieldTypeName.Value?.Value,
                                                TypeId = p.FieldTy.Value
                                            };
                                        }).ToArray(),
                                        Index = p.Index.Value
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

        public void Print()
        {
            //foreach(var type in mdv14.RuntimeMetadataData.Types.Value)
            //{
            //    //var value = type.Ty.Path.Value;
            //    Console.WriteLine($"{type.Id}[{type.Ty.TypeDef.Value}] --> {string.Join('.', type.Ty.Path.Value.Select(p => p.Value))}");
            //    for (int i = 0; i < type.Ty.TypeParams.Value.Count; i++)
            //    {
            //        var typeParam = type.Ty.TypeParams.Value[i];
            //        Console.WriteLine($"          +-> Param: {typeParam.TypeParameterName.Value}{(typeParam.TypeParameterType.OptionFlag? "[" + typeParam.TypeParameterType.Value.Value.ToString() + "]":"")}");

            //    }

            //    switch (type.Ty.TypeDef.Value)
            //    {
            //        case TypeDefEnum.Composite:
            //            {
            //                var typeDef = type.Ty.TypeDef.Value2 as TypeDefComposite;
            //                Console.WriteLine($"          ### {typeDef.GetType().Name}");
            //                for (int i = 0; i < typeDef.Fields.Value.Count; i++)
            //                {
            //                    Field field = typeDef.Fields.Value[i];
            //                    Console.WriteLine($"          +-> Field{(field.FieldName.OptionFlag ? "(" + field.FieldName.Value.Value + ")" : "")}: {(field.FieldTypeName.OptionFlag ? field.FieldTypeName.Value.Value : "")}[{field.FieldTy.Value.Value}]");

            //                }
            //            }
            //            break;
            //        case TypeDefEnum.Variant:
            //            break;
            //        case TypeDefEnum.Sequence:
            //            break;
            //        case TypeDefEnum.Array:
            //            {
            //                var typeDef = type.Ty.TypeDef.Value2 as TypeDefArray;
            //                Console.WriteLine($"          ### {typeDef.GetType().Name}");
            //                Console.WriteLine($"          +-> [{typeDef.TypeParam.Value}] Length: {typeDef.Len.Value}");
            //            }
            //            break;
            //        case TypeDefEnum.Tuple:
            //            break;
            //        case TypeDefEnum.Primitive:
            //            {
            //                var typeDef = (TypeDefPrimitive)Enum.Parse(typeof(TypeDefPrimitive), type.Ty.TypeDef.Value2.ToString());
            //                Console.WriteLine($"          ### TypeDefPrimitive");
            //                Console.WriteLine($"          +-> {typeDef}");
            //            }
            //            break;
            //        case TypeDefEnum.Compact:
            //            {
            //                var typeDef = type.Ty.TypeDef.Value2 as TypeDefCompact;
            //            }
            //            break;
            //        case TypeDefEnum.BitSequence:
            //            break;
            //    }
            //}
        }

    }
}
