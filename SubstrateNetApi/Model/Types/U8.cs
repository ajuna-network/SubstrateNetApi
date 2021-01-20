using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.Model.Types
{
    public partial class U8 : IEncodable
    {
        public byte Value { get; }

        [JsonIgnore]
        public byte[] Bytes { get; }

        public U8(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal U8(Memory<byte> memory)
        {
            Bytes = memory.ToArray();
            Value = memory.ToArray()[0];
        }

        public U8(byte value)
        {
            Bytes = BitConverter.GetBytes(value);
            Value = value;
        }

        override
        public string ToString()
        {
            return Value.ToString();
        }

        public byte[] Encode()
        {
            byte[] reversed = Bytes;
            Array.Reverse(reversed);
            return reversed;
        }
    }
}
