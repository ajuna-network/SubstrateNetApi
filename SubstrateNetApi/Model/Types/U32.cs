using System;

namespace SubstrateNetApi.Model.Types
{
    public class U32 : BaseType
    {
        public override string Name() => "u32";

        public override int Size() => 4;

        public uint Value { get; internal set; }

        public override byte[] Encode()
        {
            var reversed = Bytes;
            Array.Reverse(reversed);
            return reversed;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = BitConverter.ToUInt32(byteArray, 0);
        }
    }
}