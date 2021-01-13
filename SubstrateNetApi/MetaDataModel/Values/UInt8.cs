using Newtonsoft.Json;
using System;
using System.Numerics;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class UInt8 : IEncodable
    {
        public byte Value { get; }

        [JsonIgnore]
        public byte[] Bytes { get; }

        public UInt8(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal UInt8(Memory<byte> memory)
        {
            Bytes = memory.ToArray();
            Value = memory.ToArray()[0];
        }

        public UInt8(byte value)
        {
            Bytes = BitConverter.GetBytes(value);
            Value = value;
        }

        public byte[] Encode()
        {
            byte[] reversed = Bytes;
            Array.Reverse(reversed);
            return reversed;
        }

        override
        public string ToString()
        {
            return Value.ToString();
        }
    }
}
