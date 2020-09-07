using System;
using Newtonsoft.Json;

namespace SubstrateNetApi
{
    internal class Hash
    {
        public const int HEXSIZE = 32;

        public string HexString { get; }

        [JsonIgnore]
        public byte[] Bytes { get; }

        public Hash(string resultString)
        {
            Bytes = Utils.HexToByteArray(resultString);
            HexString = Utils.Bytes2HexString(Bytes, Utils.HexStringFormat.PREFIXED);
        }

        override
        public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}