using System;
using System.Numerics;

namespace SubstrateNetApi.Model.Types
{
    public partial class Balance : IEncodable
    {
        public BigInteger Value { get; }

        public Balance(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal Balance(Memory<byte> memory)
        {
            Value = new BigInteger(memory.ToArray());
        }

        public Balance(BigInteger value)
        {
            Value = value;
        }

        public byte[] Encode()
        {
            return new CompactInteger(Value).Encode();
        }

        override
        public string ToString()
        {
            return Value.ToString();
        }
    }
}
