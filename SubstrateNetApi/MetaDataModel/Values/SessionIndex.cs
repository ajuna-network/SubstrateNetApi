using System;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class SessionIndex : UInt32S
    {
        public SessionIndex(uint value) : base(value) { }
    }

    public class UInt32S
    {
        public uint Value;

        public UInt32S(uint value)
        {
            Value = value;
        }

        public static UInt32S Decode(Memory<byte> byteArray, ref int p)
        {
            var u32 = BitConverter
                .ToUInt32(byteArray.Span.Slice(p, 4).ToArray(), 0);
            p += 4;
            return new UInt32S(u32);
        }
    }
}