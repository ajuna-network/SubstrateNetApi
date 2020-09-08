using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.MetaDataModel.Values
{
    internal class MogwaiStruct
    {
        private string resultString;

        public Hash Id { get; }

        public Hash Dna { get; }

        public Balance Price { get; }

        public ulong Gen { get; }

        public MogwaiStruct(string resultString)
        {
            var str = resultString;
            if (resultString.StartsWith("0x"))
            {
                str = resultString.Substring(2);
            }

            Id = new Hash(str.Substring(0, 64));
            Dna = new Hash(str.Substring(64, 64));
            Price = new Balance(str.Substring(128, 32));
            Gen = BitConverter.ToUInt64(Utils.HexToByteArray(str.Substring(160, 16)), 0);
        }

        override
        public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}