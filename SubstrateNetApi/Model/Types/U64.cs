using System;

namespace SubstrateNetApi.Model.Types
{
    public class U64 : BaseType
    {
        public override string Name() => "u64";

        public override int Size() => 8;

        public ulong Value { get; internal set; }

        public override byte[] Encode()
        {
            var reversed = Bytes;
            Array.Reverse(reversed);
            return reversed;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = BitConverter.ToUInt64(byteArray, 0);
        }
    }
}