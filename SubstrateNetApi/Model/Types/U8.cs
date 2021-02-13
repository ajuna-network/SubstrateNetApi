using System;

namespace SubstrateNetApi.Model.Types
{
    public class U8 : BaseType
    {
        public override string Name() => "u8";

        public override int Size() => 1;

        public byte Value { get; internal set; }

        public override byte[] Encode()
        {
            var reversed = Bytes;
            Array.Reverse(reversed);
            return reversed;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = byteArray[0];
        }
    }
}
