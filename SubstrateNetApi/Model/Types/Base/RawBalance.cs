using System;
using System.Numerics;

namespace SubstrateNetApi.Model.Types.Base
{
    public class RawBalance : BaseType<BigInteger>
    {
        public override string Name() => "T::Balance";

        public override int Size() => 16;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = new BigInteger(byteArray);
        }

        public void Create(BigInteger value)
        {
            var byteArray = new byte[16];
            Array.Copy(value.ToByteArray(), 0,  byteArray, 0, value.ToByteArray().Length);
            Bytes = byteArray;
            Value = value;
        }
    }
}