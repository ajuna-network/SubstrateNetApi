using System;

namespace SubstrateNetApi.Model.Types
{
    public class U16 : BaseType
    {
        public override string Name() => "u16";

        public override int Size() => 2;

        public ushort Value { get; internal set; }

        public override byte[] Encode()
        {
            var reversed = Bytes;
            Array.Reverse(reversed);
            return reversed;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = BitConverter.ToUInt16(byteArray, 0);
        }
    }
}