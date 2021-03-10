/*
 * Copyright (C) 2020 Usetech Professional
 * Modifications: copyright (C) 2021 DOT Mog
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Schnorrkel.Scalars
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    public class Scalar52
    {
        private ulong[] _data;

        public Scalar52(byte[] data)
        {
            var dt = new ulong[] { 0, 0, 0, 0 };
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    dt[i] |= ((ulong)data[(i * 8) + j]) << (j * 8);
                }
            }

            var mask = (1UL << 52) - 1;
            var topMask = (1UL << 48) - 1;
            var s = Zero();

            s[0] = dt[0] & mask;
            s[1] = ((dt[0] >> 52) | (dt[1] << 12)) & mask;
            s[2] = ((dt[1] >> 40) | (dt[2] << 24)) & mask;
            s[3] = ((dt[2] >> 28) | (dt[3] << 36)) & mask;
            s[4] = (dt[3] >> 16) & topMask;

            _data = s._data;
        }

        public static Scalar52 operator *(Scalar52 self, Scalar52 other)
        {
            return Mul(self, other);
        }

        public static Scalar52 operator +(Scalar52 self, Scalar52 other)
        {
            return Add(self, other);
        }

        public static Scalar52 operator -(Scalar52 self, Scalar52 other)
        {
            return Sub(self, other);
        }

        public static ulong AsU64(BigInteger bi)
        {
            var ba = bi.ToByteArray();
            return ba.Length >= 8 ? BitConverter.ToUInt64(ba.AsSpan(0, 8).ToArray(), 0) : (ulong)(long)bi;
        }

        private static byte AsU8(ulong num)
        {
            return (byte)(num & 0xffUL);
        }

        private static byte AsU8(BigInteger bi)
        {
            return bi.ToByteArray()[0];
        }

        private static BigInteger M(BigInteger x, BigInteger y) => x * y;

        private static (BigInteger Carry, ulong N) Part1(BigInteger sum)
        {
            var p = WrappingMul(AsU64(sum), Consts.LFACTOR) & ((1UL << 52) - 1);
            return ((sum + M(p, Consts.L[0])) >> 52, p);
        }

        private static (BigInteger Carry, ulong R) Part2(BigInteger sum)
        {
            var w = AsU64(sum) & ((1UL << 52) - 1);
            return (sum >> 52, w);
        }

        public Scalar52(ulong[] data)
        {
            _data = data;
        }

        public ulong this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        public static ulong[] GetU64Data(byte[] data)
        {
            var dataSpan = data.AsSpan();
            var result = new List<ulong>
            {
                BitConverter.ToUInt64(dataSpan[..8].ToArray(), 0),
                BitConverter.ToUInt64(dataSpan[8..16].ToArray(), 0),
                BitConverter.ToUInt64(dataSpan[16..24].ToArray(), 0),
                BitConverter.ToUInt64(dataSpan[^8..].ToArray(), 0),
                0
            };

            return result.ToArray();
        }

        public static Scalar52 Zero()
        {
            return new Scalar52(new ulong[] { 0, 0, 0, 0, 0 });
        }

        public static Scalar52 MontgomeryMul(Scalar52 a, Scalar52 b)
        {
            return MontgomeryReduce(MulInternal(a, b));
        }

        public static BigInteger[] MulInternal(Scalar52 a, Scalar52 b)
        {
            BigInteger[] z = new BigInteger[9];

            z[0] = M(a[0], b[0]);
            z[1] = M(a[0], b[1]) + M(a[1], b[0]);
            z[2] = M(a[0], b[2]) + M(a[1], b[1]) + M(a[2], b[0]);
            z[3] = M(a[0], b[3]) + M(a[1], b[2]) + M(a[2], b[1]) + M(a[3], b[0]);
            z[4] = M(a[0], b[4]) + M(a[1], b[3]) + M(a[2], b[2]) + M(a[3], b[1]) + M(a[4], b[0]);
            z[5] =                  M(a[1], b[4]) + M(a[2], b[3]) + M(a[3], b[2]) + M(a[4], b[1]);
            z[6] =                                   M(a[2], b[4]) + M(a[3], b[3]) + M(a[4], b[2]);
            z[7] =                                                    M(a[3], b[4]) + M(a[4], b[3]);
            z[8] =                                                                     M(a[4], b[4]);

            return z;
        }

        public static Scalar52 Add(Scalar52 a, Scalar52 b)
        {
            var sum = Zero();
            ulong mask = (1UL << 52) - 1;

            ulong carry = 0;
            for (var i = 0; i < 5; i++)
            {
                carry = a[i] + b[i] + (carry >> 52);
                sum[i] = carry & mask;
            }

            // subtract l if the sum is >= l
            return Sub(sum, Consts.L);
        }

        public static ulong WrappingSub(ulong a, ulong b)
        {
            return AsU64(new BigInteger(a) - new BigInteger(b));
        }

        public static ulong WrappingMul(ulong a, ulong b)
        {
            return AsU64(new BigInteger(a) * new BigInteger(b));
        }

        public static Scalar52 Sub(Scalar52 a, Scalar52 b)
        {
            var difference = Zero();
            ulong mask = (1UL << 52) - 1;

            // a - b
            ulong borrow = 0;
            for (var i = 0; i < 5; i++)
            {
                borrow = WrappingSub(a[i], b[i] + (borrow >> 63));
                difference[i] = borrow & mask;
            }

            // conditionally add l if the difference is negative
            var underflow_mask = WrappingSub((borrow >> 63) ^ 1, 1);
            ulong carry = 0;
            for (var i = 0; i < 5; i++)
            {
                carry = (carry >> 52) + difference[i] + (Consts.L[i] & underflow_mask);
                difference[i] = carry & mask;
            }

            return difference;
        }

        public static Scalar52 Mul(Scalar52 a, Scalar52 b)
        {
            var ab = MontgomeryReduce(MulInternal(a, b));
            return MontgomeryReduce(MulInternal(ab, Consts.RR));
        }

        public static Scalar52 MontgomeryReduce(BigInteger[] limbs)
        {
            var l = Consts.L;

            // the first half computes the Montgomery adjustment factor n, and begins adding n*l to make limbs divisible by R
            var (carry0, n0) = Part1(limbs[0]);
            var (carry1, n1) = Part1(carry0 + limbs[1] + M(n0, l[1]));
            var (carry2, n2) = Part1(carry1 + limbs[2] + M(n0,l[2]) + M(n1,l[1]));
            var (carry3, n3) = Part1(carry2 + limbs[3]              + M(n1,l[2]) + M(n2,l[1]));
            var (carry4, n4) = Part1(carry3 + limbs[4] + M(n0,l[4])              + M(n2,l[2]) + M(n3,l[1]));

            // limbs is divisible by R now, so we can divide by R by simply storing the upper half as the result
            var (carry5, r0) = Part2(carry4 + limbs[5]              + M(n1,l[4])              + M(n3,l[2]) + M(n4,l[1]));
            var (carry6, r1) = Part2(carry5 + limbs[6]                           + M(n2,l[4])              + M(n4,l[2]));
            var (carry7, r2) = Part2(carry6 + limbs[7]                                        + M(n3,l[4])             );
            var (carry8, r3) = Part2(carry7 + limbs[8]                                                     + M(n4,l[4]));
            var         r4 = AsU64(carry8);

            return Sub(new Scalar52(new ulong[] { r0, r1, r2, r3, r4 }), l);
        }

        public byte[] ToBytes()
        {
            return ToBytes(this);
        }

        public static byte[] ToBytes(Scalar52 scalar)
        {
            byte[] s = new byte[32];

            s[0] = AsU8(scalar[0] >> 0);
            s[1] = AsU8(scalar[0] >> 8);
            s[2] = AsU8(scalar[0] >> 16);
            s[3] = AsU8(scalar[0] >> 24);
            s[4] = AsU8(scalar[0] >> 32);
            s[5] = AsU8(scalar[0] >> 40);
            s[6] = AsU8((scalar[0] >> 48) | (scalar[1] << 4));
            s[7] = AsU8(scalar[1] >> 4);
            s[8] = AsU8(scalar[1] >> 12);
            s[9] = AsU8(scalar[1] >> 20);
            s[10] = AsU8(scalar[1] >> 28);
            s[11] = AsU8(scalar[1] >> 36);
            s[12] = AsU8(scalar[1] >> 44);
            s[13] = AsU8(scalar[2] >> 0);
            s[14] = AsU8(scalar[2] >> 8);
            s[15] = AsU8(scalar[2] >> 16);
            s[16] = AsU8(scalar[2] >> 24);
            s[17] = AsU8(scalar[2] >> 32);
            s[18] = AsU8(scalar[2] >> 40);
            s[19] = AsU8((scalar[2] >> 48) | (scalar[3] << 4));
            s[20] = AsU8(scalar[3] >> 4);
            s[21] = AsU8(scalar[3] >> 12);
            s[22] = AsU8(scalar[3] >> 20);
            s[23] = AsU8(scalar[3] >> 28);
            s[24] = AsU8(scalar[3] >> 36);
            s[25] = AsU8(scalar[3] >> 44);
            s[26] = AsU8(scalar[4] >> 0);
            s[27] = AsU8(scalar[4] >> 8);
            s[28] = AsU8(scalar[4] >> 16);
            s[29] = AsU8(scalar[4] >> 24);
            s[30] = AsU8(scalar[4] >> 32);
            s[31] = AsU8(scalar[4] >> 40);

            return s;
        }
    }
}
