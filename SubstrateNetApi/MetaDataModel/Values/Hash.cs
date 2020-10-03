using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class Hash : IEncodable
    {
        public const int HEXSIZE = 32;

        public string HexString { get; }

        [JsonIgnore]
        public byte[] Bytes { get; }

        public Hash(byte[] bytes)
        {
            Bytes = bytes;
            HexString = Utils.Bytes2HexString(bytes, Utils.HexStringFormat.PREFIXED);
        }

        public Hash(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal Hash(Memory<byte> memory)
        {
            Bytes = memory.ToArray();
            HexString = Utils.Bytes2HexString(Bytes, Utils.HexStringFormat.PREFIXED);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public byte[] Encode()
        {
            return Bytes;
        }
    }
}