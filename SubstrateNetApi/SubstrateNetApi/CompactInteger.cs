using System.Numerics;

namespace SubstrateNetApi
{
    public struct CompactInteger
    {
        public CompactInteger(BigInteger value)
        {
            Value = value;
        }

        public static CompactInteger operator +(CompactInteger self, int value)
        {
            self.Value += value;
            return self;
        }

        public static CompactInteger operator *(CompactInteger self, int value)
        {
            self.Value *= value;
            return self;
        }

        public static CompactInteger operator +(CompactInteger self, uint value)
        {
            self.Value += value;
            return self;
        }

        public static CompactInteger operator *(CompactInteger self, uint value)
        {
            self.Value *= value;
            return self;
        }

        public static CompactInteger operator +(CompactInteger self, byte value)
        {
            self.Value += value;
            return self;
        }

        public static CompactInteger operator *(CompactInteger self, byte value)
        {
            self.Value *= value;
            return self;
        }

        public static CompactInteger operator +(CompactInteger self, CompactInteger value)
        {
            self.Value += value.Value;
            return self;
        }

        public static CompactInteger operator *(CompactInteger self, CompactInteger value)
        {
            self.Value *= value.Value;
            return self;
        }

        public static implicit operator BigInteger(CompactInteger c) => c.Value;
        public static implicit operator CompactInteger(BigInteger b) => new CompactInteger(b);

        public BigInteger Value { get; set; }
    }

}
