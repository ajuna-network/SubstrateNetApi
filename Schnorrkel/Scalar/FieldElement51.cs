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
    using System.Numerics;

    public class FieldElement51
    {
        public ulong[] _data = { 0, 0, 0, 0, 0 };

        public FieldElement51()
        {
        }

        public FieldElement51(ulong[] data)
        {
            _data = data;
        }

        public FieldElement51(ulong p1, ulong p2, ulong p3, ulong p4, ulong p5)
        {
            _data = new ulong[] { p1, p2, p3, p4, p5 };
        }

        public static FieldElement51 operator *(FieldElement51 self, FieldElement51 other)
        {
            return self.Mul(other);
        }

        public static FieldElement51 operator +(FieldElement51 self, FieldElement51 other)
        {
            return self.Add(other);
        }

        public static FieldElement51 operator -(FieldElement51 self, FieldElement51 other)
        {
            return self.Sub(other);
        }

        public ulong this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        public static ulong AsU64(BigInteger bi)
        {
            var ba = bi.ToByteArray();
            return ba.Length >= 8 ? BitConverter.ToUInt64(ba.AsSpan(0, 8).ToArray(), 0) : (ulong)(long)bi;
        }

        public static FieldElement51 FromBytes(byte[] bytes)
        {
            var load8 = new Func<byte[], ulong>((input) =>
            {
                return ((ulong)input[0])
                    | (((ulong)input[1]) << 8)
                    | (((ulong)input[2]) << 16)
                    | (((ulong)input[3]) << 24)
                    | (((ulong)input[4]) << 32)
                    | (((ulong)input[5]) << 40)
                    | (((ulong)input[6]) << 48)
                    | (((ulong)input[7]) << 56);
            });

            var low51BitMask = ((ulong)1 << 51) - 1;

            return new
                FieldElement51(new ulong[]
                {
                     // load bits [  0, 64), no shift
                    load8(bytes.AsMemory(0, 8).ToArray()) & low51BitMask,
                    // load bits [ 48,112), shift to [ 51,112)
                    (load8(bytes.AsMemory(6, 8).ToArray()) >> 3) & low51BitMask,
                    // load bits [ 96,160), shift to [102,160)
                    (load8(bytes.AsMemory(12, 8).ToArray()) >> 6) & low51BitMask,
                    // load bits [152,216), shift to [153,216)
                    (load8(bytes.AsMemory(19, 8).ToArray()) >> 1) & low51BitMask,
                    // load bits [192,256), shift to [204,112)
                    (load8(bytes.AsMemory(24, 8).ToArray()) >> 12) & low51BitMask
                });
        }

        public FieldElement51 Clone()
        {
            return new FieldElement51(_data[0], _data[1], _data[2], _data[3], _data[4]);
        }

        public bool CtEq(FieldElement51 a)
        {
            var b1 = ToBytes();
            var b2 = a.ToBytes();

            // Short-circuit on the *lengths* of the slices, not their
            // contents.
            if (b1.Length != b2.Length)
                return false;

            for (var i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i])
                    return false;
            }

            return true;
        }

        public FieldElement51 Negate()
        {
            return Reduce(new ulong[] {
                36028797018963664 - _data[0],
                36028797018963952 - _data[1],
                36028797018963952 - _data[2],
                36028797018963952 - _data[3],
                36028797018963952 - _data[4]
            });
        }

        public static (bool, FieldElement51) SqrtRatioI(FieldElement51 u, FieldElement51 v)
        {
            var pow22501 = new Func<FieldElement51, (FieldElement51, FieldElement51)>(fe =>
            {
                // Instead of managing which temporary variables are used
                // for what, we define as many as we need and leave stack
                // allocation to the compiler
                //
                // Each temporary variable t_i is of the form (self)^e_i.
                // Squaring t_i corresponds to multiplying e_i by 2,
                // so the pow2k function shifts e_i left by k places.
                // Multiplying t_i and t_j corresponds to adding e_i + e_j.
                //
                // Temporary t_i                      Nonzero bits of e_i
                //
                var t0 = fe.Square();             // 1         e_0 = 2^1
                var t1 = t0.Square().Square();    // 3         e_1 = 2^3
                var t2 = fe.Mul(t1);              // 3,0       e_2 = 2^3 + 2^0
                var t3 = t0.Mul(t2);               // 3,1,0
                var t4 = t3.Square();             // 4,2,1
                var t5 = t2.Mul(t4);               // 4,3,2,1,0
                var t6 = t5.Pow2k(5);             // 9,8,7,6,5
                var t7 = t6.Mul(t5);               // 9,8,7,6,5,4,3,2,1,0
                var t8 = t7.Pow2k(10);            // 19..10
                var t9 = t8.Mul(t7);               // 19..0
                var t10 = t9.Pow2k(20);            // 39..20
                var t11 = t10.Mul(t9);              // 39..0
                var t12 = t11.Pow2k(10);           // 49..10
                var t13 = t12.Mul(t7);              // 49..0
                var t14 = t13.Pow2k(50);           // 99..50
                var t15 = t14.Mul(t13);             // 99..0
                var t16 = t15.Pow2k(100);          // 199..100
                var t17 = t16.Mul(t15);             // 199..0
                var t18 = t17.Pow2k(50);           // 249..50
                var t19 = t18.Mul(t13);             // 249..0

                return (t19, t3);
            });

            var powP58 = new Func<FieldElement51, FieldElement51>(e =>
            {

                // The bits of (p-5)/8 are 101111.....11.
                //
                //                                 nonzero bits of exponen
                var t19 = pow22501(e);
                var t20 = t19.Item1.Pow2k(2);
                var t21 = e.Mul(t20);

                return t21;
            });

            // Using the same trick as in ed25519 decoding, we merge the
            // inversion, the square root, and the square test as follows.
            //
            // To compute sqrt(α), we can compute β = α^((p+3)/8).
            // Then β^2 = ±α, so multiplying β by sqrt(-1) if necessary
            // gives sqrt(α).
            //
            // To compute 1/sqrt(α), we observe that
            //    1/β = α^(p-1 - (p+3)/8) = α^((7p-11)/8)
            //                            = α^3 * (α^7)^((p-5)/8).
            //
            // We can therefore compute sqrt(u/v) = sqrt(u)/sqrt(v)
            // by first computing
            //    r = u^((p+3)/8) v^(p-1-(p+3)/8)
            //      = u u^((p-5)/8) v^3 (v^7)^((p-5)/8)
            //      = (uv^3) (uv^7)^((p-5)/8).
            //
            // If v is nonzero and u/v is square, then r^2 = ±u/v,
            //                                     so vr^2 = ±u.
            // If vr^2 =  u, then sqrt(u/v) = r.
            // If vr^2 = -u, then sqrt(u/v) = r*sqrt(-1).
            //
            // If v is zero, r is also zero.

            var vsq = v.Square();

            var v3 = v.Square().Mul(v);
            var v7 = v3.Square().Mul(v);
            var r = u.Mul(v3).Mul(powP58((u.Mul(v7))));
            var check = v.Mul(r.Square());

            var i = Consts.SQRT_M1;

            var correct_sign_sqrt = check.CtEq(u);
            var flipped_sign_sqrt = check.CtEq(u.Negate());
            var flipped_sign_sqrt_i = check.CtEq(u.Negate().Mul(i));

            var r_prime = r.Mul(Consts.SQRT_M1);
            r.ConditionalAssign(r_prime, flipped_sign_sqrt | flipped_sign_sqrt_i);

            // Choose the nonnegative square root.
            var r_is_negative = r.IsNegative();
            if (r_is_negative)
            {
                r = r.Negate();
            }
            var was_nonzero_square = correct_sign_sqrt | flipped_sign_sqrt;

            return (was_nonzero_square, r);
        }

        public void ConditionalNegate(bool choice)
        {
            var nself = Negate();
            ConditionalAssign(nself, choice);
        }

        public bool IsNegative()
        {
            var dt = BitConverter.GetBytes(_data[0]);
            var dti = dt[0] & 1;
            return dti > 0;
        }

        public void ConditionalAssign(FieldElement51 other, bool choice)
        {
            var ConditionalAssign = new Func<ulong, ulong, bool, ulong>((original, candidate, condition) =>
            {
                return condition ? candidate : original;
            });

            _data[0] = ConditionalAssign(_data[0], other._data[0], choice);
            _data[1] = ConditionalAssign(_data[1], other._data[1], choice);
            _data[2] = ConditionalAssign(_data[2], other._data[2], choice);
            _data[3] = ConditionalAssign(_data[3], other._data[3], choice);
            _data[4] = ConditionalAssign(_data[4], other._data[4], choice);
        }

        public FieldElement51 Pow2k(int k)
        {
            /// Multiply two 64-bit integers with 128 bits of output.
            var m = new Func<BigInteger, BigInteger, BigInteger>((x, y) =>
            { return x * y; });

            var a = (ulong[])_data.Clone();

            while (true)
            {
                // Precondition: assume input limbs a[i] are bounded as
                //
                // a[i] < 2^(51 + b)
                //
                // where b is a real parameter measuring the "bit excess" of the limbs.

                // Precomputation: 64-bit multiply by 19.
                //
                // This fits into a u64 whenever 51 + b + lg(19) < 64.
                //
                // Since 51 + b + lg(19) < 51 + 4.25 + b
                //                       = 55.25 + b,
                // this fits if b < 8.75.
                var a3_19 = 19 * a[3];
                var a4_19 = 19 * a[4];

                // Multiply to get 128-bit coefficients of output.
                //
                // The 128-bit multiplications by 2 turn into 1 slr + 1 slrd each,
                // which doesn't seem any better or worse than doing them as precomputations
                // on the 64-bit inputs.
                BigInteger t1 = (m(a[1], a4_19) + m(a[2], a3_19));
                BigInteger t2 = 2 * t1;
                BigInteger t3 = m(a[0], a[0]) + t2;
                BigInteger c0 = m(a[0], a[0]) + 2 * (m(a[1], a4_19) + m(a[2], a3_19));
                BigInteger c1 = m(a[3], a3_19) + 2 * (m(a[0], a[1]) + m(a[2], a4_19));
                BigInteger c2 = m(a[1], a[1]) + 2 * (m(a[0], a[2]) + m(a[4], a3_19));
                BigInteger c3 = m(a[4], a4_19) + 2 * (m(a[0], a[3]) + m(a[1], a[2]));
                BigInteger c4 = m(a[2], a[2]) + 2 * (m(a[0], a[4]) + m(a[1], a[3]));

                // Same bound as in multiply:
                //    c[i] < 2^(102 + 2*b) * (1+i + (4-i)*19)
                //         < 2^(102 + lg(1 + 4*19) + 2*b)
                //         < 2^(108.27 + 2*b)
                //
                // The carry (c[i] >> 51) fits into a u64 when
                //    108.27 + 2*b - 51 < 64
                //    2*b < 6.73
                //    b < 3.365.
                //
                // So we require b < 3 to ensure this fits.

                const ulong LOW_51_BIT_MASK = ((ulong)1 << 51) - 1;

                // Casting to u64 and back tells the compiler that the carry is bounded by 2^64, so
                // that the addition is a u128 + u64 rather than u128 + u128.
                c1 += c0 >> 51;
                a[0] = (AsU64(c0)) & LOW_51_BIT_MASK;

                c2 += c1 >> 51;
                a[1] = (AsU64(c1)) & LOW_51_BIT_MASK;

                c3 += c2 >> 51;
                a[2] = (AsU64(c2)) & LOW_51_BIT_MASK;

                c4 += c3 >> 51;
                a[3] = (AsU64(c3)) & LOW_51_BIT_MASK;

                var carry = c4 >> 51;
                a[4] = (AsU64(c4)) & LOW_51_BIT_MASK;

                // To see that this does not overflow, we need a[0] + carry * 19 < 2^64.
                //
                // c4 < a2^2 + 2*a0*a4 + 2*a1*a3 + (carry from c3)
                //    < 2^(102 + 2*b + lg(5)) + 2^64.
                //
                // When b < 3 we get
                //
                // c4 < 2^110.33  so that carry < 2^59.33
                //
                // so that
                //
                // a[0] + carry * 19 < 2^51 + 19 * 2^59.33 < 2^63.58
                //
                // and there is no overflow.
                a[0] = a[0] + (ulong)carry * 19;

                // Now a[1] < 2^51 + 2^(64 -51) = 2^51 + 2^13 < 2^(51 + epsilon).
                a[1] += a[0] >> 51;
                a[0] &= LOW_51_BIT_MASK;

                // Now all a[i] < 2^(51 + epsilon) and a = self^(2^k).
                k--;
                if (k == 0)
                {
                    break;
                }
            }

            return new FieldElement51(a);// { _data = a };
        }

        public static FieldElement51 One()
        {
            return new FieldElement51 { _data = new ulong[] { 1, 0, 0, 0, 0 } };
        }

        public static FieldElement51 Zero()
        {
            return new FieldElement51 { _data = new ulong[] { 0, 0, 0, 0, 0 } };
        }

        public FieldElement51 Mul(FieldElement51 second)
        {
            var m = new Func<BigInteger, BigInteger, BigInteger>((x, y) => { return x * y; });

            // Alias self, _rhs for more readable formulas
            var a = _data;
            var b = second;

            // Precondition: assume input limbs a[i], b[i] are bounded as
            //
            // a[i], b[i] < 2^(51 + b)
            //
            // where b is a real parameter measuring the "bit excess" of the limbs.

            // 64-bit precomputations to avoid 128-bit multiplications.
            //
            // This fits into a u64 whenever 51 + b + lg(19) < 64.
            //
            // Since 51 + b + lg(19) < 51 + 4.25 + b
            //                       = 55.25 + b,
            // this fits if b < 8.75.
            var b1_19 = b[1] * 19;
            var b2_19 = b[2] * 19;
            var b3_19 = b[3] * 19;
            var b4_19 = b[4] * 19;

            // Multiply to get 128-bit coefficients of output
            BigInteger c0 = m(a[0], b[0]) + m(a[4], b1_19) + m(a[3], b2_19) + m(a[2], b3_19) + m(a[1], b4_19);
            BigInteger c1 = m(a[1], b[0]) + m(a[0], b[1]) + m(a[4], b2_19) + m(a[3], b3_19) + m(a[2], b4_19);
            BigInteger c2 = m(a[2], b[0]) + m(a[1], b[1]) + m(a[0], b[2]) + m(a[4], b3_19) + m(a[3], b4_19);
            BigInteger c3 = m(a[3], b[0]) + m(a[2], b[1]) + m(a[1], b[2]) + m(a[0], b[3]) + m(a[4], b4_19);
            BigInteger c4 = m(a[4], b[0]) + m(a[3], b[1]) + m(a[2], b[2]) + m(a[1], b[3]) + m(a[0], b[4]);

            // Casting to u64 and back tells the compiler that the carry is
            // bounded by 2^64, so that the addition is a u128 + u64 rather
            // than u128 + u128.

            const ulong LOW_51_BIT_MASK = ((ulong)1 << 51) - 1;
            var output = new ulong[5];

            c1 += c0 >> 51;
            output[0] = (ulong)(c0 & LOW_51_BIT_MASK);

            c2 += c1 >> 51;
            output[1] = (ulong)(c1 & LOW_51_BIT_MASK);

            c3 += c2 >> 51;
            output[2] = (ulong)(c2 & LOW_51_BIT_MASK);

            c4 += c3 >> 51;
            output[3] = (ulong)(c3 & LOW_51_BIT_MASK);

            ulong carry = (ulong)(c4 >> 51);
            output[4] = (ulong)(c4 & LOW_51_BIT_MASK);

            // To see that this does not overflow, we need out[0] + carry * 19 < 2^64.
            //
            // c4 < a0*b4 + a1*b3 + a2*b2 + a3*b1 + a4*b0 + (carry from c3)
            //    < 5*(2^(51 + b) * 2^(51 + b)) + (carry from c3)
            //    < 2^(102 + 2*b + lg(5)) + 2^64.
            //
            // When b < 3 we get
            //
            // c4 < 2^110.33  so that carry < 2^59.33
            //
            // so that
            //
            // out[0] + carry * 19 < 2^51 + 19 * 2^59.33 < 2^63.58
            //
            // and there is no overflow.
            output[0] = output[0] + (carry * 19);

            // Now out[1] < 2^51 + 2^(64 -51) = 2^51 + 2^13 < 2^(51 + epsilon).
            output[1] += output[0] >> 51;
            output[0] &= LOW_51_BIT_MASK;

            // Now out[i] < 2^(51 + epsilon) for all i.
            return new FieldElement51 { _data = output };
        }

        public FieldElement51 Reduce(ulong[] limbs)
        {
            const ulong LOW_51_BIT_MASK = ((ulong)1 << 51) - 1;

            // Since the input limbs are bounded by 2^64, the biggest
            // carry-out is bounded by 2^13.
            //
            // The biggest carry-in is c4 * 19, resulting in
            //
            // 2^51 + 19*2^13 < 2^51.0000000001
            //
            // Because we don't need to canonicalize, only to reduce the
            // limb sizes, it's OK to do a "weak reduction", where we
            // compute the carry-outs in parallel.

            var c0 = limbs[0] >> 51;
            var c1 = limbs[1] >> 51;
            var c2 = limbs[2] >> 51;
            var c3 = limbs[3] >> 51;
            var c4 = limbs[4] >> 51;

            limbs[0] &= LOW_51_BIT_MASK;
            limbs[1] &= LOW_51_BIT_MASK;
            limbs[2] &= LOW_51_BIT_MASK;
            limbs[3] &= LOW_51_BIT_MASK;
            limbs[4] &= LOW_51_BIT_MASK;

            limbs[0] += c4 * 19;
            limbs[1] += c0;
            limbs[2] += c1;
            limbs[3] += c2;
            limbs[4] += c3;

            return new FieldElement51(limbs);// { _data = limbs };
        }

        public FieldElement51 Add(FieldElement51 x)
        {
            var f = new FieldElement51();
            for (var i = 0; i < 5; i++)
            {
                f[i] = _data[i] + x._data[i];
            }
            return f;
        }

        public FieldElement51 Sub(FieldElement51 x)
        {
            // To avoid underflow, first add a multiple of p.
            // Choose 16*p = p << 4 to be larger than 54-bit _rhs.
            //
            // If we could statically track the bitlengths of the limbs
            // of every FieldElement51, we could choose a multiple of p
            // just bigger than _rhs and avoid having to do a reduction.
            //
            // Since we don't yet have type-level integers to do this, we
            // have to add an explicit reduction call here.

            return Reduce(new ulong[] {
                _data[0] + 36028797018963664 - x._data[0],
                _data[1] + 36028797018963952 - x._data[1],
                _data[2] + 36028797018963952 - x._data[2],
                _data[3] + 36028797018963952 - x._data[3],
                _data[4] + 36028797018963952 - x._data[4]
            });
        }

        public FieldElement51 Square()
        {
            return Pow2k(1);
        }

        public FieldElement51 Square2()
        {
            var square = Pow2k(1);
            for (var i = 0; i < 5; i++)
            {
                square[i] *= 2;
            }

            return square;
        }

        public FieldElement51 BitXor(FieldElement51 a)
        {
            return new FieldElement51
            {
                _data = new ulong[] {
                    _data[0] ^ a._data[0],
                    _data[1] ^ a._data[1],
                    _data[2] ^ a._data[2],
                    _data[3] ^ a._data[3],
                    _data[4] ^ a._data[4]
                }
            };
        }

        public FieldElement51 BitAnd(uint a)
        {
            return new FieldElement51
            {
                _data = new ulong[] {
                    _data[0] & a,
                    _data[1] & a,
                    _data[2] & a,
                    _data[3] & a,
                    _data[4] & a,
                }
            };
        }

        public byte[] ToBytes()
        {
            // Let h = limbs[0] + limbs[1]*2^51 + ... + limbs[4]*2^204.
            //
            // Write h = pq + r with 0 <= r < p.
            //
            // We want to compute r = h mod p.
            //
            // If h < 2*p = 2^256 - 38,
            // then q = 0 or 1,
            //
            // with q = 0 when h < p
            //  and q = 1 when h >= p.
            //
            // Notice that h >= p <==> h + 19 >= p + 19 <==> h + 19 >= 2^255.
            // Therefore q can be computed as the carry bit of h + 19.

            // First, reduce the limbs to ensure h < 2*p.
            var cp = Clone();

            var limbs = cp.Reduce((ulong[])_data.Clone())._data;

            var q = (limbs[0] + 19) >> 51;
            q = (limbs[1] + q) >> 51;
            q = (limbs[2] + q) >> 51;
            q = (limbs[3] + q) >> 51;
            q = (limbs[4] + q) >> 51;

            // Now we can compute r as r = h - pq = r - (2^255-19)q = r + 19q - 2^255q

            limbs[0] += 19 * q;

            // Now carry the result to compute r + 19q ...
            var low_51_bit_mask = ((ulong)1 << 51) - 1;
            limbs[1] += limbs[0] >> 51;
            limbs[0] = limbs[0] & low_51_bit_mask;
            limbs[2] += limbs[1] >> 51;
            limbs[1] = limbs[1] & low_51_bit_mask;
            limbs[3] += limbs[2] >> 51;
            limbs[2] = limbs[2] & low_51_bit_mask;
            limbs[4] += limbs[3] >> 51;
            limbs[3] = limbs[3] & low_51_bit_mask;
            // ... but instead of carrying (limbs[4] >> 51) = 2^255q
            // into another limb, discard it, subtracting the value
            limbs[4] = limbs[4] & low_51_bit_mask;

            // Now arrange the bits of the limbs.
            var s = new byte[32];
            s[0] = (byte)limbs[0];
            s[1] = (byte)(limbs[0] >> 8);
            s[2] = (byte)(limbs[0] >> 16);
            s[3] = (byte)(limbs[0] >> 24);
            s[4] = (byte)(limbs[0] >> 32);
            s[5] = (byte)(limbs[0] >> 40);
            s[6] = (byte)((limbs[0] >> 48) | (limbs[1] << 3));
            s[7] = (byte)(limbs[1] >> 5);
            s[8] = (byte)(limbs[1] >> 13);
            s[9] = (byte)(limbs[1] >> 21);
            s[10] = (byte)(limbs[1] >> 29);
            s[11] = (byte)(limbs[1] >> 37);
            s[12] = (byte)((limbs[1] >> 45) | (limbs[2] << 6));
            s[13] = (byte)(limbs[2] >> 2);
            s[14] = (byte)(limbs[2] >> 10);
            s[15] = (byte)(limbs[2] >> 18);
            s[16] = (byte)(limbs[2] >> 26);
            s[17] = (byte)(limbs[2] >> 34);
            s[18] = (byte)(limbs[2] >> 42);
            s[19] = (byte)((limbs[2] >> 50) | (limbs[3] << 1));
            s[20] = (byte)(limbs[3] >> 7);
            s[21] = (byte)(limbs[3] >> 15);
            s[22] = (byte)(limbs[3] >> 23);
            s[23] = (byte)(limbs[3] >> 31);
            s[24] = (byte)(limbs[3] >> 39);
            s[25] = (byte)((limbs[3] >> 47) | (limbs[4] << 4));
            s[26] = (byte)(limbs[4] >> 4);
            s[27] = (byte)(limbs[4] >> 12);
            s[28] = (byte)(limbs[4] >> 20);
            s[29] = (byte)(limbs[4] >> 28);
            s[30] = (byte)(limbs[4] >> 36);
            s[31] = (byte)(limbs[4] >> 44);

            // High bit should be zero.
            //debug_assert!((s[31] & 0b1000_0000u8) == 0u8);

            return s;
        }

        public FieldElement51 Invert()
        {
            var pow22501 = new Func<FieldElement51, (FieldElement51, FieldElement51)>(fe =>
            {
                // Instead of managing which temporary variables are used
                // for what, we define as many as we need and leave stack
                // allocation to the compiler
                //
                // Each temporary variable t_i is of the form (self)^e_i.
                // Squaring t_i corresponds to multiplying e_i by 2,
                // so the pow2k function shifts e_i left by k places.
                // Multiplying t_i and t_j corresponds to adding e_i + e_j.
                //
                // Temporary t_i                      Nonzero bits of e_i
                //
                var t0 = fe.Square();             // 1         e_0 = 2^1
                var t1 = t0.Square().Square();    // 3         e_1 = 2^3
                var t2 = fe.Mul(t1);              // 3,0       e_2 = 2^3 + 2^0
                var t3 = t0.Mul(t2);               // 3,1,0
                var t4 = t3.Square();             // 4,2,1
                var t5 = t2.Mul(t4);               // 4,3,2,1,0
                var t6 = t5.Pow2k(5);             // 9,8,7,6,5
                var t7 = t6.Mul(t5);               // 9,8,7,6,5,4,3,2,1,0
                var t8 = t7.Pow2k(10);            // 19..10
                var t9 = t8.Mul(t7);               // 19..0
                var t10 = t9.Pow2k(20);            // 39..20
                var t11 = t10.Mul(t9);              // 39..0
                var t12 = t11.Pow2k(10);           // 49..10
                var t13 = t12.Mul(t7);              // 49..0
                var t14 = t13.Pow2k(50);           // 99..50
                var t15 = t14.Mul(t13);             // 99..0
                var t16 = t15.Pow2k(100);          // 199..100
                var t17 = t16.Mul(t15);             // 199..0
                var t18 = t17.Pow2k(50);           // 249..50
                var t19 = t18.Mul(t13);             // 249..0

                return (t19, t3);
            });

            // The bits of p-2 = 2^255 -19 -2 are 11010111111...11.
            //
            //                                 nonzero bits of exponent
            var r = pow22501(this);   // t19: 249..0 ; t3: 3,1,0
            var t20 = r.Item1.Pow2k(5);            // 254..5
            var t21 = t20.Mul(r.Item2);              // 254..5,3,1,0

            return t21;
        }
    }
}
