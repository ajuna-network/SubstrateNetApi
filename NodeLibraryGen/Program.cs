using Newtonsoft.Json;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types.Metadata.V14;
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

            var nodeTypes = new Dictionary<uint, NodeType>();

            foreach (var type in mdv14.RuntimeMetadataData.Types.Value)
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
                    nodeTypes.Add(nodeType.Id, nodeType);
                }
            }

            var jsonFile = JsonConvert.SerializeObject(nodeTypes, Formatting.Indented);

            File.WriteAllText("metadata.json", jsonFile);

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
