using System.Collections.Generic;
using System.Numerics;

namespace SubstrateNetApi
{
    public struct CompactInteger
    {
        public CompactInteger(BigInteger value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is CompactInteger i)
            {
                return Value.Equals(i.Value);
            }
            return Value.Equals(obj);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static CompactInteger operator /(CompactInteger self, CompactInteger value) => self.Value / value.Value;
        public static CompactInteger operator -(CompactInteger self, CompactInteger value) => self.Value - value.Value;
        public static CompactInteger operator +(CompactInteger self, CompactInteger value) => self.Value + value.Value;
        public static CompactInteger operator *(CompactInteger self, CompactInteger value) => self.Value * value.Value;
        public static bool operator <(CompactInteger self, CompactInteger value) => self.Value < value.Value;
        public static bool operator >(CompactInteger self, CompactInteger value) => self.Value > value.Value;
        public static bool operator <=(CompactInteger self, CompactInteger value) => self.Value <= value.Value;
        public static bool operator >=(CompactInteger self, CompactInteger value) => self.Value >= value.Value;
        public static CompactInteger operator <<(CompactInteger self, int value) => self.Value << value;
        public static CompactInteger operator >>(CompactInteger self, int value) => self.Value >> value;
        public static CompactInteger operator &(CompactInteger self, CompactInteger value) => self.Value & value.Value;
        public static CompactInteger operator |(CompactInteger self, CompactInteger value) => self.Value | value.Value;
        public static bool operator ==(CompactInteger self, CompactInteger value) => self.Value == value.Value;
        public static bool operator !=(CompactInteger self, CompactInteger value) => self.Value != value.Value;

        public static explicit operator BigInteger(CompactInteger c) => c.Value;
        public static implicit operator sbyte(CompactInteger c) => (sbyte)c.Value;
        public static implicit operator byte(CompactInteger c) => (byte)c.Value;
        public static implicit operator short(CompactInteger c) => (short)c.Value;
        public static implicit operator ushort(CompactInteger c) => (ushort)c.Value;
        public static implicit operator int(CompactInteger c) => (int)c.Value;
        public static implicit operator uint(CompactInteger c) => (uint)c.Value;
        public static implicit operator long(CompactInteger c) => (long)c.Value;
        public static implicit operator ulong(CompactInteger c) => (ulong)c.Value;

        public static implicit operator CompactInteger(BigInteger b) => new CompactInteger(b);
        public static implicit operator CompactInteger(sbyte i) => new CompactInteger(i);
        public static implicit operator CompactInteger(byte i) => new CompactInteger(i);
        public static implicit operator CompactInteger(short i) => new CompactInteger(i);
        public static implicit operator CompactInteger(ushort i) => new CompactInteger(i);
        public static implicit operator CompactInteger(int i) => new CompactInteger(i);
        public static implicit operator CompactInteger(uint i) => new CompactInteger(i);
        public static implicit operator CompactInteger(long i) => new CompactInteger(i);
        public static implicit operator CompactInteger(ulong i) => new CompactInteger(i);

        public BigInteger Value { get; }

        public static CompactInteger Decode(byte[] m)
        {
            int p = 0;
            return Decode(m, ref p);
        }

        public static CompactInteger Decode(byte[] m, ref int p)
        {
            uint firstByte = m[p++];
            uint flag = (firstByte) & 0b00000011u;
            CompactInteger number = 0u;

            switch (flag)
            {
                case 0b00u:
                    {
                        number = firstByte >> 2;
                        break;
                    }

                case 0b01u:
                    {
                        uint secondByte = m[p++];

                        number = ((uint)((firstByte) & 0b11111100u) + (uint)(secondByte) * 256u) >> 2;
                        break;
                    }

                case 0b10u:
                    {
                        number = firstByte;
                        uint multiplier = 256u;

                        for (var i = 0; i < 3; ++i)
                        {
                            number += m[p++] * multiplier;
                            multiplier = multiplier << 8;
                        }
                        number = number >> 2;
                        break;
                    }

                case 0b11:
                    {
                        uint bytesCount = ((firstByte) >> 2) + 4u;
                        CompactInteger multiplier = 1u;
                        CompactInteger value = 0;

                        // we assured that there are m more bytes,
                        // no need to make checks in a loop
                        for (var i = 0; i < bytesCount; ++i)
                        {
                            value += multiplier * m[p++];
                            multiplier *= 256u;
                        }

                        return value;
                    }
            }

            return number;
        }

        public byte[] Encode()
        {
            if (this <= 63)
            {
                return new byte[] { (byte)(this << 2) };
            }

            if (this <= 0x3FFF)
            {
                return new byte[] { (byte)(((this & 0x3F) << 2) | 0x01), (byte)((this & 0xFC0) >> 6) };
            }

            if (this <= 0x3FFFFFFF)
            {
                var result = new byte[4];
                result[0] = (byte)(((this & 0x3F) << 2) | 0x02);
                this >>= 6;
                for (int i = 1; i < 4; ++i)
                {
                    result[i] = (byte)(this & 0xFF);
                    this >>= 8;
                }
                return result;
            }
            else
            {
                var b0 = new List<byte>();
                while (this > 0)
                {
                    b0.Add((byte)(this & 0xFF));
                    this >>= 8;
                }
                var result = new List<byte>
                {
                    (byte)(((b0.Count - 4) << 2) | 0x03)
                };
                result.AddRange(b0);
                return result.ToArray();
            }
        }
    }
}
