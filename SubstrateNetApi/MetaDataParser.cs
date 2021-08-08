using System;
using System.Text;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Struct;

namespace SubstrateNetApi
{
    public class MetaDataParser
    {
        public string MetaDateString { get;  set; }

        public MetaDataParser(string origin, string metaData)
        {
            MetaDateString = metaData;
            Parse(origin, Utils.HexToByteArray(metaData));
        }

        public MetaData MetaData { get; private set; }

        internal MetaData Parse(string origin, byte[] m)
        {
            var p = 0;

            MetaData = new MetaData(origin)
            {
                Magic = Encoding.ASCII.GetString(new[] {m[p++], m[p++], m[p++], m[p++]}),
                Version = "v" + BitConverter.ToInt16(new byte[] {m[p++], 0x00}, 0)
            };

            // pub types: PortableRegistry,
            // pub struct PortableRegistry
            // {
            //   types: Vec<PortableType>,
            // }
            var tlen = CompactInteger.Decode(m, ref p);

            for (var typeIndex = 0; typeIndex < tlen; typeIndex++) {
                
                var typeId = CompactInteger.Decode(m, ref p);

                // The unique path to the type. Can be empty for built-in types
                var pathCount = CompactInteger.Decode(m, ref p);
                for (var pathIndex = 0; pathIndex < pathCount; pathIndex++)
                {
                    var pathName = ExtractString(m, ref p);
                }

                // The generic type parameters of the type in use. Empty for non generic types
                var paramsCount = CompactInteger.Decode(m, ref p);
                for (var paramsIndex = 0; paramsIndex < paramsCount; paramsIndex++)
                {
                    var paramsName = ExtractString(m, ref p);

                    var optionTypeParam = m[p++];
                    if (optionTypeParam != 0)
                    {
                        var typeParam = CompactInteger.Decode(m, ref p);
                    }
                }

                // The actual type definition
                // https://github.com/paritytech/scale-info/blob/b8cb9177a2ce85d8e156b604a6d0dd796ed8bdaa/src/ty/mod.rs#L237
                //var typDef = CompactInteger.Decode(m, ref p);
                var typDef = (Types.TypeDef)BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0);
                switch (typDef)
                {
                    case Types.TypeDef.Composite:
                        var fieldsCount = CompactInteger.Decode(m, ref p);
                        for (var fieldIndex = 0; fieldIndex < fieldsCount; fieldIndex++)
                        {
                            var optionName = m[p++];
                            if (optionName != 0)
                            {
                                var name = ExtractString(m, ref p);
                            }
                            var compositeType = CompactInteger.Decode(m, ref p);
                            var optionTypeName = m[p++];
                            if (optionTypeName != 0)
                            {
                                var typeName = ExtractString(m, ref p);
                            }
                            var docsCount = CompactInteger.Decode(m, ref p);
                            for (var docsIndex = 0; docsIndex < docsCount; docsIndex++)
                            {
                                var docs = ExtractString(m, ref p);
                            }
                        }
                        break;

                    case Types.TypeDef.Variant:
                        var variantsCount = CompactInteger.Decode(m, ref p);
                        for (var variantsIndex = 0; variantsIndex < variantsCount; variantsIndex++)
                        {
                            var name = ExtractString(m, ref p);
                            var variantFieldsCount = CompactInteger.Decode(m, ref p);
                            for (var variantFieldIndex = 0; variantFieldIndex < variantFieldsCount; variantFieldIndex++)
                            {
                                var optionName = m[p++];
                                if (optionName != 0)
                                {
                                    var variantName = ExtractString(m, ref p);
                                }
                                var compositeType = CompactInteger.Decode(m, ref p);
                                var optionTypeName = m[p++];
                                if (optionTypeName != 0)
                                {
                                    var typeName = ExtractString(m, ref p);
                                }
                                var variantDocsCount = CompactInteger.Decode(m, ref p);
                                for (var variantDocsIndex = 0; variantDocsIndex < variantDocsCount; variantDocsIndex++)
                                {
                                    var docs = ExtractString(m, ref p);
                                }
                            }
                            
                            U8 variantIndex = new U8();
                            variantIndex.Decode(m, ref p);

                            var docsCount = CompactInteger.Decode(m, ref p);
                            for (var docsIndex = 0; docsIndex < docsCount; docsIndex++)
                            {
                                var docs = ExtractString(m, ref p);
                            }

                        }
                        break;

                    case Types.TypeDef.Sequence:
                        var seqTypDefPrimitive = CompactInteger.Decode(m, ref p);
                        break;
                    
                    case Types.TypeDef.Array:
                        U32 arrayLen = new U32();
                        arrayLen.Decode(m, ref p);
                        //var arrayLen = CompactInteger.Decode(m, ref p);
                        var arrayType = CompactInteger.Decode(m, ref p);
                        break;
                    
                    case Types.TypeDef.Tuple:
                        var tupleFieldsCount = CompactInteger.Decode(m, ref p);
                        for (var fieldIndex = 0; fieldIndex < tupleFieldsCount; fieldIndex++)
                        {
                            var tupleTypDefPrimitive = CompactInteger.Decode(m, ref p);
                        }
                        break;

                    case Types.TypeDef.Primitive:
                        var typDefPrimitive = (Types.TypeDefPrimitive)BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0);
                        break;

                    case Types.TypeDef.Compact:
                        var compactType = CompactInteger.Decode(m, ref p);
                        break;

                    case Types.TypeDef.BitSequence:
                        var bitStoreType = CompactInteger.Decode(m, ref p);
                        var bitOrderType = CompactInteger.Decode(m, ref p);
                        break;

                    default:
                        throw new Exception($"Unknown {typDef}");
                }

                // Documentation
                var docCount = CompactInteger.Decode(m, ref p);
                for (var docIndex = 0; docIndex < docCount; docIndex++)
                {
                    var docs = ExtractString(m, ref p);
                }
            }

            // Metadata of all the pallets.
            // pub pallets: Vec<PalletMetadata<PortableForm>>,
            var mlen = CompactInteger.Decode(m, ref p);

            MetaData.Modules = new Module[mlen];
            for (var modIndex = 0; modIndex < mlen; modIndex++)
            {
                var module = new Module
                {
                    Name = ExtractString(m, ref p)
                };

                var hasStorage = m[p++];
                if (hasStorage != 0)
                {
                    module.Storage = new Storage();
                    module.Storage.Prefix = ExtractString(m, ref p);

                    var storageLen = CompactInteger.Decode(m, ref p);
                    module.Storage.Items = new Item[storageLen];

                    for (var i = 0; i < storageLen; i++)
                    {
                        var item = new Item();
                        item.Name = ExtractString(m, ref p);
                        item.Modifier = (Storage.Modifier) BitConverter.ToInt16(new byte[] {m[p++], 0x00}, 0);

                        item.Type = (Storage.Type) BitConverter.ToInt16(new byte[] {m[p++], 0x00}, 0);

                        byte[] look = new byte[100];
                        Buffer.BlockCopy(m, p, look, 0, look.Length);

                        item.Function = new Function();

                        item.Function.Hasher = Storage.Hasher.None;

                        // default
                        item.Function.Key1 = null;
                        item.Function.Key2 = null;
                        item.Function.IsLinked = null;
                        item.Function.Key2Hasher = Storage.Hasher.None;

                        switch (item.Type)
                        {
                            case Storage.Type.Plain:
                                item.Function.Value = CompactInteger.Decode(m, ref p).Value.ToString();
                                break;
                            case Storage.Type.Map:
                                var shizzle1 = CompactInteger.Decode(m, ref p);
                                item.Function.Hasher = (Storage.Hasher)BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0);
                                item.Function.Key1 = CompactInteger.Decode(m, ref p).Value.ToString();
                                item.Function.Value = CompactInteger.Decode(m, ref p).Value.ToString();
                                //item.Function.IsLinked = m[p++] != 0;
                                break;
                            case Storage.Type.DoubleMap:
                                item.Function.Hasher = (Storage.Hasher)BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0);
                                item.Function.Key1 = CompactInteger.Decode(m, ref p).Value.ToString();
                                item.Function.Key2 = CompactInteger.Decode(m, ref p).Value.ToString();
                                item.Function.Value = CompactInteger.Decode(m, ref p).Value.ToString();
                                item.Function.Key2Hasher = (Storage.Hasher)(item.Type != Storage.Type.Plain
                                ? BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0)
                                : -1);
                                break;
                            default:
                                // NMap
                                //NMap {
                                //keys: DecodeDifferentArray < &'static str, StringBuf>,
                                //
                                //    hashers: DecodeDifferentArray<StorageHasher>,
		                        //    value: DecodeDifferentStr,
	                            //},
                                throw new Exception($"Unhandled storage type '{item.Type}', please create an issue on github!");
                        }

                        var vector = new Vec<U8>();
                        vector.Decode(m, ref p);

                        //item.FallBack = Utils.Bytes2HexString(ExtractBytes(m, ref p));

                        var docLen = CompactInteger.Decode(m, ref p);
                        item.Documentations = new string[docLen];
                        for (var j = 0; j < docLen; j++) item.Documentations[j] = ExtractString(m, ref p);

                        module.Storage.Items[i] = item;
                    }
                }

                byte[] look1 = new byte[50];
                Buffer.BlockCopy(m, p, look1, 0, look1.Length);

                var hasCalls = m[p++];
                if (hasCalls != 0)
                {
                    var callType = CompactInteger.Decode(m, ref p);

                    //var callsLen = CompactInteger.Decode(m, ref p);
                    //module.Calls = new Call[callsLen];

                    //for (var i = 0; i < callsLen; i++)
                    //{
                    //    var call = new Call();
                    //    call.Name = ExtractString(m, ref p);

                    //    var argsLen = CompactInteger.Decode(m, ref p);
                    //    call.Arguments = new Argument[argsLen];

                    //    for (var j = 0; j < argsLen; j++)
                    //    {
                    //        var argument = new Argument();
                    //        argument.Name = ExtractString(m, ref p);
                    //        argument.Type = CompactInteger.Decode(m, ref p).Value.ToString(); ;

                    //        call.Arguments[j] = argument;
                    //    }

                    //    var docLen = CompactInteger.Decode(m, ref p);
                    //    call.Documentations = new string[docLen];
                    //    for (var j = 0; j < docLen; j++) call.Documentations[j] = ExtractString(m, ref p);

                    //    module.Calls[i] = call;
                    //}
                }

                var hasEvents = m[p++];
                if (hasEvents != 0)
                {
                    var eventType = CompactInteger.Decode(m, ref p);

                    //var eventsLen = CompactInteger.Decode(m, ref p);
                    //module.Events = new Event[eventsLen];
                    //for (var i = 0; i < eventsLen; i++)
                    //{
                    //    var evnt = new Event
                    //    {
                    //        Name = ExtractString(m, ref p)
                    //    };

                    //    var argsLen = CompactInteger.Decode(m, ref p);
                    //    evnt.EventArgs = new string[argsLen];

                    //    for (var j = 0; j < argsLen; j++) evnt.EventArgs[j] = ExtractString(m, ref p);

                    //    var docLen = CompactInteger.Decode(m, ref p);
                    //    evnt.Documentations = new string[docLen];
                    //    for (var j = 0; j < docLen; j++) evnt.Documentations[j] = ExtractString(m, ref p);

                    //    module.Events[i] = evnt;
                    //}
                }

                var conLen = CompactInteger.Decode(m, ref p);
                module.Consts = new Const[conLen];
                for (var i = 0; i < conLen; i++)
                {
                    
                    var cons = new Const
                    {
                        Name = ExtractString(m, ref p),
                        Type = CompactInteger.Decode(m, ref p).Value.ToString(),
                        //Value = Utils.Bytes2HexString(ExtractBytes(m, ref p))
                    };
                    // TODO
                    var vector = new Vec<U8>();
                    vector.Decode(m, ref p);

                    var docLen = CompactInteger.Decode(m, ref p);
                    cons.Documentations = new string[docLen];
                    for (var j = 0; j < docLen; j++) cons.Documentations[j] = ExtractString(m, ref p);

                    module.Consts[i] = cons;
                }

                byte[] look2 = new byte[50];
                Buffer.BlockCopy(m, p, look2, 0, look2.Length);

                var hasErrors = m[p++];
                if (hasErrors != 0)
                {
                    var errorType = CompactInteger.Decode(m, ref p);
                }

                //var errLen = CompactInteger.Decode(m, ref p);
                //module.Errors = new Error[errLen];
                //for (var i = 0; i < errLen; i++)
                //{
                //    var err = new Error
                //    {
                //        Name = ExtractString(m, ref p)
                //    };

                //    var docLen = CompactInteger.Decode(m, ref p);
                //    err.Documentations = new string[docLen];
                //    for (var j = 0; j < docLen; j++) err.Documentations[j] = ExtractString(m, ref p);

                //    module.Errors[i] = err;
                //}

                module.Index = m[p++];

                MetaData.Modules[modIndex] = module;
            }

            byte[] look3 = new byte[50];
            Buffer.BlockCopy(m, p, look3, 0, look3.Length);

            // Metadata of the extrinsic.
            // pub extrinsic: ExtrinsicMetadata<PortableForm>,
            var extrinsicType = CompactInteger.Decode(m, ref p);
            var extrinsicVersion = CompactInteger.Decode(m, ref p);
            var eLen = CompactInteger.Decode(m, ref p);
            for (var i = 0; i < eLen; i++)
            {
                var identifier = ExtractString(m, ref p);
                var identifierType = CompactInteger.Decode(m, ref p);
                var boeh = CompactInteger.Decode(m, ref p);
            }

            //var eLen = CompactInteger.Decode(m, ref p);
            //for (var i = 0; i < eLen; i++)
            //{
            //    var itmLen = CompactInteger.Decode(m, ref p);
            //    MetaData.ExtrinsicExtensions = new string[itmLen];
            //    for (var j = 0; j < itmLen; j++) MetaData.ExtrinsicExtensions[j] = ExtractString(m, ref p);
            //}

            return MetaData;
        }

        private byte[] ExtractBytes(byte[] m, ref int p)
        {
            var value = CompactInteger.Decode(m, ref p);
            var bytes = new byte[value];
            for (var i = 0; i < value; i++) bytes[i] = m[p++];
            return bytes;
        }

        private string ExtractString(byte[] m, ref int p)
        {
            var value = CompactInteger.Decode(m, ref p);

            var s = string.Empty;
            for (var i = 0; i < value; i++) s += (char) m[p++];
            return s;
        }
    }
}