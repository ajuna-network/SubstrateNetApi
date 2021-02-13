using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using StreamJsonRpc;

namespace SubstrateNetApi.Model.Types
{
    public partial class VectorU8 : IEncodable
    {
        public List<U8> Value { get; }

        [JsonIgnore]
        public byte[] Bytes { get; }

        public VectorU8(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal VectorU8(Memory<byte> memory)
        {
            var byteArray = memory.ToArray();

            var list = new List<U8>();

            var p = 0;
            var length = CompactInteger.Decode(byteArray, ref p);
            for (var i = 0; i < length; i++)
            {
                list.Add(U8.Decode(byteArray, ref p));
            }

            Bytes = memory.ToArray();
            Value = list;
        }

        override
        public string ToString()
        {
            return JsonConvert.SerializeObject(Value.ToString());
        }

        public byte[] Encode()
        {
            throw new NotImplementedException();
        }
    }
}
