using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.Model.Types
{
    public class BlockNumber : BaseType
    {
        public override string Name() => "T::BlockNumber";

        public override int Size() => 1;

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

        public void Create(uint value)
        {
            Bytes = BitConverter.GetBytes(value);
            Value = value;
        }
    }
}
