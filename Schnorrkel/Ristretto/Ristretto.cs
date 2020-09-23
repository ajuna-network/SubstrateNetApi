namespace Schnorrkel.Ristretto
{
    using Schnorrkel.Scalars;
    using System;

    public class CompressedRistretto : IEquatable<CompressedRistretto>
    {
        public byte[] _compressedRistrettoBytes { get; set; }

        public CompressedRistretto(byte[] data)
        {
            _compressedRistrettoBytes = data;
        }

        public byte[] ToBytes()
        {
            return _compressedRistrettoBytes;
        }

        public byte[] GetBytes()
        {
            return _compressedRistrettoBytes;
        }

        public bool Equals(CompressedRistretto other)
        {
            for(var i = 0; i < 32; i++)
            {
                if (!(_compressedRistrettoBytes[i] == other._compressedRistrettoBytes[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }

    internal class RistrettoBasepointTable
    {
        private EdwardsBasepointTable edwardsBasepointTable;

        public RistrettoBasepointTable()
        {
            edwardsBasepointTable = Consts.ED25519_BASEPOINT_TABLE_INNER;
        }

        public RistrettoPoint Mul(Scalar s)
        {
            var ep = edwardsBasepointTable.Mul(s);

            return new RistrettoPoint(ep);
        }
    }

    internal class RistrettoPoint
    {
        public EdwardsPoint Ep;

        public RistrettoPoint(EdwardsPoint ep)
        {
            Ep = ep;
        }

        /// Compute \\(aA + bB\\) in variable time, where \\(B\\) is the
        /// Ristretto basepoint.
        public static EdwardsPoint VartimeDoubleScalarMulBasepoint(Scalar a, EdwardsPoint A, Scalar b)
        {
            var aNaf = a.NonAdjacentForm(5);
            var bNaf = b.NonAdjacentForm(8);
            int i = 0;

            /// Find starting index
            for (var ind = 255; ind >= 0; ind--)
            {
                i = ind;
                if (aNaf[i] != 0 || bNaf[i] != 0)
                {
                    break;
                }
            }

            var tableA = NafLookupTable.FromEdwardsPoint(A);
            var tableB = Consts.AFFINE_ODD_MULTIPLES_OF_BASEPOINT;

            var r = ProjectivePoint.Identity();

            while(i >= 0)
            {
                var t = r.Double();

                if (aNaf[i] > 0)
                {
                    var t1 = t.ToExtended();
                    var i1 = Math.Abs((sbyte)(-1 * aNaf[i]) / 2);
                    var t2 = tableA.Pnp[i1];
                    t = t1.Add(t2);
                }
                else if (aNaf[i] < 0)
                {
                    var t1 = t.ToExtended();
                    var i1 = Math.Abs((sbyte)(-1 * aNaf[i]) / 2);
                    var t2 = tableA.Pnp[i1];
                    t = t1.Sub(t2);
                }

                if (bNaf[i] > 0)
                {
                    var t1 = t.ToExtended();
                    var i1 = Math.Abs((sbyte)(-1 * bNaf[i]) / 2);
                    var t2 = tableB.affineNielsPoints[i1];
                    t = t1.Add(t2);
                }
                else if (bNaf[i] < 0)
                {
                    var t1 = t.ToExtended();
                    var i1 = Math.Abs((sbyte)(-1 * bNaf[i]) / 2);
                    var t2 = tableB.affineNielsPoints[i1];
                    t = t1.Sub(t2);
                }

                r = t.ToProjective();

                i--;
            }

            return r.ToExtended();
        }

        /// Compress this point using the Ristretto encoding.
        public CompressedRistretto Compress()
        {
            var invsqrt = 
            new Func<FieldElement51, (bool, FieldElement51)>((el) => {
                return FieldElement51.SqrtRatioI(FieldElement51.One(), el);
            });

            var X = Ep.X;
            var Y = Ep.Y;
            var Z = Ep.Z;
            var T = Ep.T;

            var u1 = Z.Add(Y).Mul(Z.Sub(Y));
            var u2 = X.Mul(Y);

            // Ignore return value since this is always square
            var inv = invsqrt(u1.Mul(u2.Square()));
            var i1 = inv.Item2.Mul(u1);
            var i2 = inv.Item2.Mul(u2);
            var z_inv = i1.Mul(i2.Mul(T));
            var den_inv = i2;

            var iX = X.Mul(Consts.SQRT_M1);
            var iY = Y.Mul(Consts.SQRT_M1);
            var ristretto_magic = (Consts.INVSQRT_A_MINUS_D);
            var enchanted_denominator = i1.Mul(ristretto_magic);
            var rotate = T.Mul(z_inv).IsNegative();

            X.ConditionalAssign(iY, rotate);
            Y.ConditionalAssign(iX, rotate);
            den_inv.ConditionalAssign(enchanted_denominator, rotate);

            Y.ConditionalNegate(X.Mul(z_inv).IsNegative());

            var s = den_inv.Mul(Z.Sub(Y));
            var s_is_negative = s.IsNegative();
            s.ConditionalNegate(s_is_negative);

            return new CompressedRistretto(s.ToBytes());
        }
    }
}
