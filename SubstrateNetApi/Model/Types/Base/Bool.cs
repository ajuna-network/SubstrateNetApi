using System;

namespace SubstrateNetApi.Model.Types.Base
{
    public class Bool : BasePrim<bool>
    {
        public override string TypeName() => "bool";

        public override int TypeSize() => 1;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = byteArray[0] > 0;
        }

        public void Create(bool value)
        {
            Bytes = new byte[] { (byte)(value ? 0x01 : 0x00) };
            Value = value;
        }
    }
}