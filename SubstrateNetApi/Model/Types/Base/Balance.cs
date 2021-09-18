using System.Numerics;

namespace SubstrateNetApi.Model.Types.Base
{
    public class Balance : BaseType<BigInteger>
    {
        public override string TypeName() => "T::Balance";

        public override int TypeSize() => 16;

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