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

            var mlen = DecodeCompactInteger(m, ref p);

            _md11.Modules = new Module[(int)mlen.Value];
            for (var modIndex = 0; modIndex < mlen.Value; modIndex++)
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

                    var storageLen = DecodeCompactInteger(m, ref p);
                    module.Storage.Items = new Item[(int)storageLen.Value];

                    for (int i = 0; i < storageLen.Value; i++)
                    {
                        var item = new Item();
                        item.Name = ExtractString(m, ref p);
                        item.Modifier = (Storage.Modifier) BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0);

                        item.Type = (Storage.Type) BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0);

                        item.Function = new Function
                        {
                            Hasher = (Storage.Hasher) (item.Type != Storage.Type.Plain ? BitConverter.ToInt16(new byte[] { m[p++], 0x00 }, 0) : -1),

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

                        var docLen = DecodeCompactInteger(m, ref p);
                        item.Documentations = new string[(int)docLen.Value];
                        for (int j = 0; j < docLen.Value; j++)
                        {
                            item.Documentations[j] = ExtractString(m, ref p);
                        }

                        module.Storage.Items[i] = item;
                    }
                }

                var hasCalls = m[p++];
                if (hasCalls != 0)
                {
                    var callsLen = DecodeCompactInteger(m, ref p);
                    module.Calls = new Call[(int)callsLen.Value];

                    for (int i = 0; i < callsLen.Value; i++)
                    {
                        var call = new Call();
                        call.Name = ExtractString(m, ref p);

                        var argsLen = DecodeCompactInteger(m, ref p);
                        call.Arguments = new Argument[(int)argsLen.Value];

                        for (var j = 0; j < argsLen.Value; j++)
                        {
                            var argument = new Argument();
                            argument.Name = ExtractString(m, ref p);
                            argument.Type = ExtractString(m, ref p);

                            call.Arguments[j] = argument;
                        }

                        var docLen = DecodeCompactInteger(m, ref p);
                        call.Documentations = new string[(int)docLen.Value];
                        for (int j = 0; j < docLen.Value; j++)
                        {
                            call.Documentations[j] = ExtractString(m, ref p);
                        }

                        module.Calls[i] = call;
                    }

                }

                var hasEvents = m[p++];
                if (hasEvents != 0)
                {
                    var eventsLen = DecodeCompactInteger(m, ref p);
                    module.Events = new Event[(int)eventsLen.Value];
                    for (int i = 0; i < eventsLen.Value; i++)
                    {
                        var evnt = new Event
                        {
                            Name = ExtractString(m, ref p)
                        };

                        var argsLen = DecodeCompactInteger(m, ref p);
                        evnt.EventArgs = new string[(int)argsLen.Value];

                        for (var j = 0; j < argsLen.Value; j++)
                        {
                            evnt.EventArgs[j] = ExtractString(m, ref p);
                        }

                        var docLen = DecodeCompactInteger(m, ref p);
                        evnt.Documentations = new string[(int)docLen.Value];
                        for (int j = 0; j < docLen.Value; j++)
                        {
                            evnt.Documentations[j] = ExtractString(m, ref p);
                        }

                        module.Events[i] = evnt;
                    }
                }

                var conLen = DecodeCompactInteger(m, ref p);
                module.Consts = new Const[(int)conLen.Value];
                for (int i = 0; i < conLen.Value; i++)
                {
                    var cons = new Const
                    {
                        Name = ExtractString(m, ref p),
                        Type = ExtractString(m, ref p),
                        Value = Utils.Bytes2HexString(ExtractBytes(m, ref p))
                    };
                    ;

                    var docLen = DecodeCompactInteger(m, ref p);
                    cons.Documentations = new string[(int)docLen.Value];
                    for (int j = 0; j < docLen.Value; j++)
                    {
                        cons.Documentations[j] = ExtractString(m, ref p);
                    }

                    module.Consts[i] = cons;
                }

                var errLen = DecodeCompactInteger(m, ref p);
                module.Errors = new Error[(int)errLen.Value];
                for (int i = 0; i < errLen.Value; i++)
                {
                    var err = new Error
                    {
                        Name = ExtractString(m, ref p)
                    };

                    var docLen = DecodeCompactInteger(m, ref p);
                    err.Documentations = new string[(int)docLen.Value];
                    for (int j = 0; j < docLen.Value; j++)
                    {
                        err.Documentations[j] = ExtractString(m, ref p);
                    }

                    module.Errors[i] = err;
                }

                _md11.Modules[modIndex] = module;
            }

            var eLen = DecodeCompactInteger(m, ref p);
            for (var i = 0; i < eLen.Value; i++)
            {
                var itmLen = DecodeCompactInteger(m, ref p);
                _md11.ExtrinsicExtensions = new string[(int)itmLen.Value];
                for (var j = 0; j < itmLen.Value; j++)
                {
                    _md11.ExtrinsicExtensions[j] = ExtractString(m, ref p);
                }

            }

            return _md11;
        }

        private byte[] ExtractBytes(byte[] m, ref int p, Utils.HexStringFormat format = Utils.HexStringFormat.PREFIXED)
        {
            var value = DecodeCompactInteger(m, ref p).Value;
            byte[] bytes = new byte[(int)value];
            for (int i = 0; i < value; i++)
            {
                bytes[i] = m[p++];
            }
            return bytes;
        }

        private string ExtractString(byte[] m, ref int p)
        {
            var value = DecodeCompactInteger(m, ref p).Value;

            string s = string.Empty;
            for (int i = 0; i < value; i++)
            {
                s += (char)m[p++];
            }
            return s;
        }

        private CompactInteger DecodeCompactInteger(byte[] m, ref int p)
        {
            uint first_byte = m[p++];
            uint flag = (first_byte) & 0b00000011u;
            uint number = 0u;

            switch (flag)
            {
                case 0b00u:
                    {
                        number = first_byte >> 2;
                        break;
                    }

                case 0b01u:
                    {
                        uint second_byte = m[p++];

                        number = ((uint)((first_byte) & 0b11111100u) + (uint)(second_byte) * 256u) >> 2;
                        break;
                    }

                case 0b10u:
                    {
                        number = first_byte;
                        uint multiplier = 256u;

                        for (var i = 0; i < 3; ++i)
                        {
                            number += m[p++] * multiplier;
                            multiplier = multiplier << 8;
                        }
                        number = 0u >> 2;
                        break;
                    }

                case 0b11:
                    {
                        uint bytes_count = ((first_byte) >> 2) + 4u;
                        CompactInteger multiplier = new CompactInteger { Value = 1u };
                        CompactInteger value = new CompactInteger { Value = 0 };

                        // we assured that there are m more bytes,
                        // no need to make checks in a loop
                        for (var i = 0; i < bytes_count; ++i)
                        {
                            value += multiplier * m[p++];
                            multiplier *= 256u;
                        }

                        return value;
                    }

                default:
                    throw new Exception("CompactInteger decode error: unknown flag");
            }

            return new CompactInteger { Value = number };
        }

    }
}
