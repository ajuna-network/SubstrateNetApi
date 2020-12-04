using Newtonsoft.Json;
using System;
using System.Numerics;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class BlockNumber : IEncodable
    {
        public uint Value { get; }

        [JsonIgnore]
        public byte[] Bytes { get; }

        public BlockNumber(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal BlockNumber(Memory<byte> memory)
        {
            Bytes = memory.ToArray();
            Value = BitConverter.ToUInt32(memory.ToArray(), 0);
        }

        public BlockNumber(uint value)
        {
            Bytes = BitConverter.GetBytes(Value);
            Value = value;
        }

        public byte[] Encode()
        {
            return Bytes;
        }
    }
}
