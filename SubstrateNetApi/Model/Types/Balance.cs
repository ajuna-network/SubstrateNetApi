using System;
using System.Numerics;

namespace SubstrateNetApi.Model.Types
{
    public class Balance : BaseType
    {
        public override string Name() => "T::Balance";

        public override int Size() => 16;

        public BigInteger Value { get; internal set; }

        public override byte[] Encode()
        {
            return new CompactInteger(Value).Encode();
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = new BigInteger(byteArray);
        }

        public void Create(BigInteger value)
        {
            Value = value;
        }
    }
}
