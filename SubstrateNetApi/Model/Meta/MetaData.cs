using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SubstrateNetApi.Model.Types.Metadata.V14;
using SubstrateNetApi.Model.Types.Struct;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubstrateNetApi.Model.Meta
{
    public class MetaData
    {
        public MetaData(RuntimeMetadata rtmd, string origin = "unknown")
        {
            Origin = origin;
            Magic = Utils.Bytes2HexString(rtmd.MetaDataInfo.Magic.Bytes);
            Version = rtmd.MetaDataInfo.Version.Value;
            NodeMetadata = new NodeMetadataV14()
            {
                Types = CreateNodeTypeDict(rtmd.RuntimeMetadataData.Types.Value),
                Modules = CreateModuleDict(rtmd.RuntimeMetadataData.Modules.Value),
                Extrinsic = CreateExtrinsic(rtmd.RuntimeMetadataData.Extrinsic),
                TypeId = (uint)rtmd.RuntimeMetadataData.TypeId.Value
            };
        }
        public string Origin { get; set; }
        public string Magic { get; set; }
        public byte Version { get; set; }
        public NodeMetadataV14 NodeMetadata { get; set; }
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, new StringEnumConverter());
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
                                    var fDocs = p.Docs == null || p.Docs.Value.Length == 0 ? null : p.Docs.Value.Select(q => q.Value).ToArray();
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
                                    var vDocs = p.Docs == null || p.Docs.Value.Length == 0 ? null : p.Docs.Value.Select(q => q.Value).ToArray();
                                    return new TypeVariant()
                                    {
                                        Name = p.VariantName.Value,
                                        TypeFields = p.VariantFields.Value.Length == 0 ? null : p.VariantFields.Value.Select(q =>
                                        {
                                            var fDocs = q.Docs == null || q.Docs.Value.Length == 0 ? null : q.Docs.Value.Select(r => r.Value).ToArray();
                                            return new NodeTypeField()
                                            {
                                                Name = q.FieldName.Value?.Value,
                                                TypeName = q.FieldTypeName.Value?.Value,
                                                TypeId = q.FieldTy.Value,
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
                        palletModule.Storage.Entries[i] = new Entry()
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
    }
}