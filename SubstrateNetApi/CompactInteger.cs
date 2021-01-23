/// <file> SubstrateNetApi\CompactInteger.cs </file>
/// <copyright file="CompactInteger.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the compact integer class. </summary>
using SubstrateNetApi.Model.Types;
using System.Collections.Generic;
using System.Numerics;

namespace SubstrateNetApi
{
    /// <summary> A compact integer. </summary>
    /// <remarks> 19.09.2020. </remarks>
    public struct CompactInteger : IEncodable
    {
        /// <summary> Constructor. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="value"> The value. </param>
        public CompactInteger(BigInteger value)
        {
            Value = value;
        }

        /// <summary> Indicates whether this instance and a specified object are equal. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="obj"> The object to compare with the current instance. </param>
        /// <returns>
        /// true if <paramref name="obj">obj</paramref> and this instance are the same type and represent
        /// the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is CompactInteger i)
            {
                return Value.Equals(i.Value);
            }
            return Value.Equals(obj);
        }

        /// <summary> Returns the hash code for this instance. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <returns> A 32-bit signed integer that is the hash code for this instance. </returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary> Returns the fully qualified type name of this instance. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <returns> The fully qualified type name. </returns>
        public override string ToString() => Value.ToString();

        /// <summary> Division operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The numerator. </param>
        /// <param name="value"> The denominator. </param>
        /// <returns> The result of the operation. </returns>
        public static CompactInteger operator /(CompactInteger self, CompactInteger value) => self.Value / value.Value;

        /// <summary> Subtraction operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The first value. </param>
        /// <param name="value"> A value to subtract from it. </param>
        /// <returns> The result of the operation. </returns>
        public static CompactInteger operator -(CompactInteger self, CompactInteger value) => self.Value - value.Value;

        /// <summary> Addition operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The first value. </param>
        /// <param name="value"> A value to add to it. </param>
        /// <returns> The result of the operation. </returns>
        public static CompactInteger operator +(CompactInteger self, CompactInteger value) => self.Value + value.Value;

        /// <summary> Multiplication operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The first value to multiply. </param>
        /// <param name="value"> The second value to multiply. </param>
        /// <returns> The result of the operation. </returns>
        public static CompactInteger operator *(CompactInteger self, CompactInteger value) => self.Value * value.Value;

        /// <summary> Less-than comparison operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The first instance to compare. </param>
        /// <param name="value"> The second instance to compare. </param>
        /// <returns> The result of the operation. </returns>
        public static bool operator <(CompactInteger self, CompactInteger value) => self.Value < value.Value;

        /// <summary> Greater-than comparison operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The first instance to compare. </param>
        /// <param name="value"> The second instance to compare. </param>
        /// <returns> The result of the operation. </returns>
        public static bool operator >(CompactInteger self, CompactInteger value) => self.Value > value.Value;

        /// <summary> Less-than-or-equal comparison operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The first instance to compare. </param>
        /// <param name="value"> The second instance to compare. </param>
        /// <returns> The result of the operation. </returns>
        public static bool operator <=(CompactInteger self, CompactInteger value) => self.Value <= value.Value;

        /// <summary> Greater-than-or-equal comparison operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The first instance to compare. </param>
        /// <param name="value"> The second instance to compare. </param>
        /// <returns> The result of the operation. </returns>
        public static bool operator >=(CompactInteger self, CompactInteger value) => self.Value >= value.Value;

        /// <summary> . </summary>
        public static CompactInteger operator <<(CompactInteger self, int value) => self.Value << value;

        /// <summary> Bitwise right shift operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The class instance that this method operates on. </param>
        /// <param name="value"> The value. </param>
        /// <returns> The result of the operation. </returns>
        public static CompactInteger operator >>(CompactInteger self, int value) => self.Value >> value;

        /// <summary> Bitwise 'and' operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  A bit-field to process. </param>
        /// <param name="value"> A mask of bits to apply to the bit-field. </param>
        /// <returns> The result of the operation. </returns>
        public static CompactInteger operator &(CompactInteger self, CompactInteger value) => self.Value & value.Value;

        /// <summary> Bitwise 'or' operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  A bit-field to process. </param>
        /// <param name="value"> One or more bits to OR into the bit-field. </param>
        /// <returns> The result of the operation. </returns>
        public static CompactInteger operator |(CompactInteger self, CompactInteger value) => self.Value | value.Value;

        /// <summary> Equality operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The first instance to compare. </param>
        /// <param name="value"> The second instance to compare. </param>
        /// <returns> The result of the operation. </returns>
        public static bool operator ==(CompactInteger self, CompactInteger value) => self.Value == value.Value;

        /// <summary> Inequality operator. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="self">  The first instance to compare. </param>
        /// <param name="value"> The second instance to compare. </param>
        /// <returns> The result of the operation. </returns>
        public static bool operator !=(CompactInteger self, CompactInteger value) => self.Value != value.Value;

        /// <summary> Explicit cast that converts the given CompactInteger to a BigInteger. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="c"> A CompactInteger to process. </param>
        /// <returns> The result of the operation. </returns>
        public static explicit operator BigInteger(CompactInteger c) => c.Value;

        /// <summary> Implicit cast that converts the given CompactInteger to a sbyte. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="c"> A CompactInteger to process. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator sbyte(CompactInteger c) => (sbyte)c.Value;

        /// <summary> Implicit cast that converts the given CompactInteger to a byte. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="c"> A CompactInteger to process. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator byte(CompactInteger c) => (byte)c.Value;

        /// <summary> Implicit cast that converts the given CompactInteger to a short. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="c"> A CompactInteger to process. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator short(CompactInteger c) => (short)c.Value;

        /// <summary> Implicit cast that converts the given CompactInteger to an ushort. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="c"> A CompactInteger to process. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator ushort(CompactInteger c) => (ushort)c.Value;

        /// <summary> Implicit cast that converts the given CompactInteger to an int. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="c"> A CompactInteger to process. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator int(CompactInteger c) => (int)c.Value;

        /// <summary> Implicit cast that converts the given CompactInteger to an uint. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="c"> A CompactInteger to process. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator uint(CompactInteger c) => (uint)c.Value;

        /// <summary> Implicit cast that converts the given CompactInteger to a long. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="c"> A CompactInteger to process. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator long(CompactInteger c) => (long)c.Value;

        /// <summary> Implicit cast that converts the given CompactInteger to an ulong. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="c"> A CompactInteger to process. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator ulong(CompactInteger c) => (ulong)c.Value;

        /// <summary> Implicit cast that converts the given BigInteger to a CompactInteger. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="b"> A BigInteger to process. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator CompactInteger(BigInteger b) => new CompactInteger(b);

        /// <summary> Implicit cast that converts the given sbyte to a CompactInteger. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="i"> Zero-based index of the. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator CompactInteger(sbyte i) => new CompactInteger(i);

        /// <summary> Implicit cast that converts the given byte to a CompactInteger. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="i"> Zero-based index of the. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator CompactInteger(byte i) => new CompactInteger(i);

        /// <summary> Implicit cast that converts the given short to a CompactInteger. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="i"> Zero-based index of the. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator CompactInteger(short i) => new CompactInteger(i);

        /// <summary> Implicit cast that converts the given ushort to a CompactInteger. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="i"> Zero-based index of the. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator CompactInteger(ushort i) => new CompactInteger(i);

        /// <summary> Implicit cast that converts the given int to a CompactInteger. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="i"> Zero-based index of the. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator CompactInteger(int i) => new CompactInteger(i);

        /// <summary> Implicit cast that converts the given uint to a CompactInteger. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="i"> Zero-based index of the. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator CompactInteger(uint i) => new CompactInteger(i);

        /// <summary> Implicit cast that converts the given long to a CompactInteger. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="i"> Zero-based index of the. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator CompactInteger(long i) => new CompactInteger(i);

        /// <summary> Implicit cast that converts the given ulong to a CompactInteger. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="i"> Zero-based index of the. </param>
        /// <returns> The result of the operation. </returns>
        public static implicit operator CompactInteger(ulong i) => new CompactInteger(i);

        /// <summary> Gets the value. </summary>
        /// <value> The value. </value>
        public BigInteger Value { get; }

        /// <summary> Decodes. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="m"> A byte[] to process. </param>
        /// <returns> A CompactInteger. </returns>
        public static CompactInteger Decode(byte[] m)
        {
            int p = 0;
            return Decode(m, ref p);
        }

        /// <summary> Decodes. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="m"> A byte[] to process. </param>
        /// <param name="p"> [in,out] an int to process. </param>
        /// <returns> A CompactInteger. </returns>
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

        /// <summary> Gets the encode. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <returns> A byte[]. </returns>
        public byte[] Encode()
        {
            if (this <= 63)
            {
                return new byte[] { this << 2 };
            }

            if (this <= 0x3FFF)
            {
                return new byte[] {
                    ((this & 0x3F) << 2) | 0x01,
                    (this & 0xFFC0) >> 6
                };
            }

            if (this <= 0x3FFFFFFF)
            {
                var result = new byte[4];
                result[0] = ((this & 0x3F) << 2) | 0x02;
                this >>= 6;
                for (int i = 1; i < 4; ++i)
                {
                    result[i] = this & 0xFF;
                    this >>= 8;
                }
                return result;
            }
            else
            {
                var b0 = new List<byte>();
                while (this > 0)
                {
                    b0.Add(this & 0xFF);
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
