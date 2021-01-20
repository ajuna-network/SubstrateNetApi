using SubstrateNetApi.Model.Meta;
using System;
using System.Text;

namespace SubstrateNetApi
{
    public class MetaDataParser
    {
        private MetaData _md11;

        public MetaData MetaData => _md11;

        public MetaDataParser(string origin, string metaData)
        {
            Parse(origin, Utils.HexToByteArray(metaData));
        }

        internal MetaData Parse(string origin, byte[] m)
        {
            var p = 0;

            _md11 = new MetaData(origin)
            {
                Magic = Encoding.ASCII.GetString(new byte[] { m[p++], m[p++], m[p++], m[p++] }),
                Version = "v" + BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0)
            };

            var mlen = CompactInteger.Decode(m, ref p);

            _md11.Modules = new Module[(int)mlen];
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
                    module.Storage.Items = new Item[(int)storageLen];

                    for (int i = 0; i < storageLen; i++)
                    {
                        var item = new Item();
                        item.Name = ExtractString(m, ref p);
                        item.Modifier = (Storage.Modifier)BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0);

                        item.Type = (Storage.Type)BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0);

                        item.Function = new Function
                        {
                            Hasher = (Storage.Hasher)(item.Type != Storage.Type.Plain ? BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0) : -1),

                        };

                        switch (item.Type)
                        {
                            case Storage.Type.Plain:
                                item.Function.Value = ExtractString(m, ref p);
                                break;
                            case Storage.Type.Map:
                                item.Function.Key1 = ExtractString(m, ref p);
                                item.Function.Value = ExtractString(m, ref p);
                                item.Function.IsLinked = m[p++] != 0;
                                break;
                            case Storage.Type.DoubleMap:
                                item.Function.Key1 = ExtractString(m, ref p);
                                item.Function.Key2 = ExtractString(m, ref p);
                                item.Function.Value = ExtractString(m, ref p);
                                item.Function.IsLinked = m[p++] != 0;
                                break;
                        }

                        item.FallBack = Utils.Bytes2HexString(ExtractBytes(m, ref p));

                        var docLen = CompactInteger.Decode(m, ref p);
                        item.Documentations = new string[(int)docLen];
                        for (int j = 0; j < docLen; j++)
                        {
                            item.Documentations[j] = ExtractString(m, ref p);
                        }

                        module.Storage.Items[i] = item;
                    }
                }

                var hasCalls = m[p++];
                if (hasCalls != 0)
                {
                    var callsLen = CompactInteger.Decode(m, ref p);
                    module.Calls = new Call[(int)callsLen];

                    for (int i = 0; i < callsLen; i++)
                    {
                        var call = new Call();
                        call.Name = ExtractString(m, ref p);

                        var argsLen = CompactInteger.Decode(m, ref p);
                        call.Arguments = new Argument[(int)argsLen];

                        for (var j = 0; j < argsLen; j++)
                        {
                            var argument = new Argument();
                            argument.Name = ExtractString(m, ref p);
                            argument.Type = ExtractString(m, ref p);

                            call.Arguments[j] = argument;
                        }

                        var docLen = CompactInteger.Decode(m, ref p);
                        call.Documentations = new string[(int)docLen];
                        for (int j = 0; j < docLen; j++)
                        {
                            call.Documentations[j] = ExtractString(m, ref p);
                        }

                        module.Calls[i] = call;
                    }

                }

                var hasEvents = m[p++];
                if (hasEvents != 0)
                {
                    var eventsLen = CompactInteger.Decode(m, ref p);
                    module.Events = new Event[(int)eventsLen];
                    for (int i = 0; i < eventsLen; i++)
                    {
                        var evnt = new Event
                        {
                            Name = ExtractString(m, ref p)
                        };

                        var argsLen = CompactInteger.Decode(m, ref p);
                        evnt.EventArgs = new string[(int)argsLen];

                        for (var j = 0; j < argsLen; j++)
                        {
                            evnt.EventArgs[j] = ExtractString(m, ref p);
                        }

                        var docLen = CompactInteger.Decode(m, ref p);
                        evnt.Documentations = new string[(int)docLen];
                        for (int j = 0; j < docLen; j++)
                        {
                            evnt.Documentations[j] = ExtractString(m, ref p);
                        }

                        module.Events[i] = evnt;
                    }
                }

                var conLen = CompactInteger.Decode(m, ref p);
                module.Consts = new Const[(int)conLen];
                for (int i = 0; i < conLen; i++)
                {
                    var cons = new Const
                    {
                        Name = ExtractString(m, ref p),
                        Type = ExtractString(m, ref p),
                        Value = Utils.Bytes2HexString(ExtractBytes(m, ref p))
                    };
                    ;

                    var docLen = CompactInteger.Decode(m, ref p);
                    cons.Documentations = new string[(int)docLen];
                    for (int j = 0; j < docLen; j++)
                    {
                        cons.Documentations[j] = ExtractString(m, ref p);
                    }

                    module.Consts[i] = cons;
                }

                var errLen = CompactInteger.Decode(m, ref p);
                module.Errors = new Error[(int)errLen];
                for (int i = 0; i < errLen; i++)
                {
                    var err = new Error
                    {
                        Name = ExtractString(m, ref p)
                    };

                    var docLen = CompactInteger.Decode(m, ref p);
                    err.Documentations = new string[(int)docLen];
                    for (int j = 0; j < docLen; j++)
                    {
                        err.Documentations[j] = ExtractString(m, ref p);
                    }

                    module.Errors[i] = err;
                }

                module.Index = m[p++];

                _md11.Modules[modIndex] = module;
            }

            var eLen = CompactInteger.Decode(m, ref p);
            for (var i = 0; i < eLen; i++)
            {
                var itmLen = CompactInteger.Decode(m, ref p);
                _md11.ExtrinsicExtensions = new string[(int)itmLen];
                for (var j = 0; j < itmLen; j++)
                {
                    _md11.ExtrinsicExtensions[j] = ExtractString(m, ref p);
                }

            }

            return _md11;
        }

        private byte[] ExtractBytes(byte[] m, ref int p, Utils.HexStringFormat format = Utils.HexStringFormat.PREFIXED)
        {
            var value = CompactInteger.Decode(m, ref p);
            byte[] bytes = new byte[(int)value];
            for (int i = 0; i < value; i++)
            {
                bytes[i] = m[p++];
            }
            return bytes;
        }

        private string ExtractString(byte[] m, ref int p)
        {
            var value = CompactInteger.Decode(m, ref p);

            string s = string.Empty;
            for (int i = 0; i < value; i++)
            {
                s += (char)m[p++];
            }
            return s;
        }
    }
}
