using System;

namespace SubstrateNetApi.Model.Types.TypeDefPrimitive
{
    public class PrimU8 : BaseType<byte>
    {
        public override string TypeName() => "u8";

        public override int TypeSize() => 1;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = byteArray[0];
        }

        public void Create(byte value)
        {
            Bytes = new byte[] { value };
            Value = value;
        }
    }
}