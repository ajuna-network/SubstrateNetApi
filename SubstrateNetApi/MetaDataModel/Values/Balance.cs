using System;
using System.Numerics;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class Balance : IEncodable
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

        public static Balance Decode(Memory<byte> byteArray, ref int p)
        {
            var balance = new Balance(byteArray.Span.Slice(p, 16).ToArray());
            p += 16;
            return balance;
        }
    }
}
