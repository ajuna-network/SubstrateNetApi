using System.Numerics;

namespace SubstrateNetApi.Model.Types.Base
{
    public class Balance : BaseType<BigInteger>
    {
        public override string Name() => "T::Balance";

        public override int Size() => 16;

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
            Bytes = value.ToByteArray();
            Value = value;
        }
    }
}