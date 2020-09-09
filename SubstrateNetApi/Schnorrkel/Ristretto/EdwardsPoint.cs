namespace Schnorrkel.Ristretto
{
    using System;
    using Schnorrkel.Scalars;

    public class EdwardsPoint
    {
        public FieldElement51 X, Y, Z, T;

        public EdwardsPoint() {}

        public EdwardsPoint(FieldElement51 x, FieldElement51 y, FieldElement51 z, FieldElement51 t)
        {
            X = x;
            Y = y;
            Z = z;
            T = t;
        }

        public bool Equals(EdwardsPoint a)
        {
            bool result = true;
            for (var i = 0; i < 5; i++)
            {
                result &= X._data[i] == a.X._data[i]
                    && Y._data[i] == a.Y._data[i]
                    && Z._data[i] == a.Z._data[i]
                    && T._data[i] == a.T._data[i];
            }

            return result;
        }

        public EdwardsPoint Copy()
        {
            return new EdwardsPoint
            {
                T = T,
                X = X,
                Y = Y,
                Z = Z
            };
        }

        internal static EdwardsPoint Decompress(byte[] bytes)
        {
            var invsqrt =
            new Func<FieldElement51, (bool, FieldElement51)>((el) => {
                return FieldElement51.SqrtRatioI(FieldElement51.One(), el);
            });

            var s = FieldElement51.FromBytes(bytes);

            // Step 2.  Compute (X:Y:Z:T).
            var one = FieldElement51.One();
            var ss = s.Square();
            var u1 = one - ss;      //  1 + as²
            var u2 = one + ss;      //  1 - as²    where a=-1
            var u2_sqr = u2.Square(); // (1 - as²)²

            // v == ad(1+as²)² - (1-as²)²            where d=-121665/121666
            var nEdwardsD = Consts.EDWARDS_D.Negate();
            var v = nEdwardsD * u1.Square() - u2_sqr;

            var I = invsqrt(v * u2_sqr); // 1/sqrt(v*u_2²)

            var Dx = I.Item2 * u2;         // 1/sqrt(v)
            var Dy = I.Item2 * Dx * v; // 1/u2

            // x == | 2s/sqrt(v) | == + sqrt(4s²/(ad(1+as²)² - (1-as²)²))
            var x = (s + s) * Dx;
            var x_neg = x.IsNegative();
            x.ConditionalNegate(x_neg);

            // y == (1-as²)/(1+as²)
            var y = u1 * Dy;

            // t == ((1+as²) sqrt(4s²/(ad(1+as²)² - (1-as²)²)))/(1-as²)
            var t = x * y;

            return new EdwardsPoint(x, y, one, t);
        }

        internal static EdwardsPoint Double(EdwardsPoint point)
        {
            return point.ToProjective().Double().ToExtended();
        }

        /// Compute \\([2\^k] P \\) by successive doublings. Requires \\( k > 0 \\).
        public EdwardsPoint MulByPow2(int k)
        {
            CompletedPoint r;
            var s = ToProjective();
            for (var i = 0; i < k - 1; i++)
            {
                r = s.Double();
                s = r.ToProjective();
            }

            // Unroll last iteration so we can go directly to_extended()
            return s.Double().ToExtended();
        }

        public static FieldElement51 Zero()
        {
            return new FieldElement51 { _data = new ulong[] { 0, 0, 0, 0, 0 } };
        }

        public static FieldElement51 One()
        {
            return new FieldElement51 { _data = new ulong[] { 1, 0, 0, 0, 0 } };
        }

        public static EdwardsPoint Identity()
        {
            return new EdwardsPoint
            {
                X = Zero(),
                Y = One(),
                Z = One(),
                T = Zero()
            };
        }

        internal EdwardsPoint Negate()
        {
            return new EdwardsPoint
            {
                X = X.Negate(),
                Y = Y,
                Z = Z,
                T = T.Negate()
            };
        }

        EdwardsPoint ToExtended()
        {
            return new EdwardsPoint
            {
                X = X.Mul(T),
                Y = Y.Mul(Z),
                Z = Z.Mul(T),
                T = X.Mul(Y)
            };
        }

        public CompletedPoint Add(ProjectiveNielsPoint other)
        {
            var Y_plus_X = Y.Add(X);
            var Y_minus_X = Y.Sub(X);
            var PP = Y_plus_X.Mul(other.Y_plus_X);
            var MM = Y_minus_X.Mul(other.Y_minus_X);
            var TT2d = T.Mul(other.T2d);
            var ZZ = Z.Mul(other.Z);
            var ZZ2 = ZZ.Add(ZZ);

            return new CompletedPoint
            {
                X = PP.Sub(MM),
                Y = PP.Add(MM),
                Z = ZZ2.Add(TT2d),
                T = ZZ2.Sub(TT2d)
            };
        }

        public CompletedPoint Sub(AffineNielsPoint other)
        {
            var Y_plus_X = Y.Add(X);
            var Y_minus_X = Y.Sub(X);
            var PM = Y_plus_X.Mul(other.Y_minus_X);
            var MP = Y_minus_X.Mul(other.Y_plus_X);
            var Txy2d = T.Mul(other.XY2d);
            var Z2 = Z.Add(Z);

            return new CompletedPoint
            {
                X = PM.Sub(MP),
                Y = PM.Add(MP),
                Z = Z2.Sub(Txy2d),
                T = Z2.Add(Txy2d)
            };
        }

        public CompletedPoint Sub(ProjectiveNielsPoint other)
        {
            var Y_plus_X = Y.Add(X);
            var Y_minus_X = Y.Sub(X);
            var PM = Y_plus_X.Mul(other.Y_minus_X);
            var MP = Y_minus_X.Mul(other.Y_plus_X);
            var TT2d = T.Mul(other.T2d);
            var ZZ = Z.Mul(other.Z);
            var ZZ2 = ZZ.Add(ZZ);

            return new CompletedPoint
            {
                X = PM.Sub(MP),
                Y = PM.Add(MP),
                Z = ZZ2.Sub(TT2d),
                T = ZZ2.Add(TT2d)
            };
        }

        public CompletedPoint Add(AffineNielsPoint other)
        {
            var Y_plus_X = Y.Add(X);
            var Y_minus_X = Y.Sub(X);
            var PP = Y_plus_X.Mul(other.Y_plus_X);
            var MM = Y_minus_X.Mul(other.Y_minus_X);
            var Txy2d = T.Mul(other.XY2d);
            var Z2 = Z.Add(Z);

            return new CompletedPoint
            {
                X = PP.Sub(MM),
                Y = PP.Add(MM),
                Z = Z2.Add(Txy2d),
                T = Z2.Sub(Txy2d)
            };
        }

        public EdwardsPoint Add(EdwardsPoint other)
        {
            return Add(other.ToProjectiveNiels()).ToExtended();
        }

        public ProjectiveNielsPoint ToProjectiveNiels()
        {
            return new ProjectiveNielsPoint
            {
                Y_plus_X = Y.Add(X),
                Y_minus_X = Y.Sub(X),
                Z = Z,
                T2d = T.Mul(Consts.EDWARDS_D2)
            };
        }

        public ProjectivePoint ToProjective()
        {
            return new ProjectivePoint
            {
                X = X,
                Y = Y,
                Z = Z
            };
        }

        public AffineNielsPoint ToAffineNiels()
        {
            var recip = Z.Invert();
            var x = X.Mul(recip);
            var y = Y.Mul(recip);
            var xy2d = X.Mul(Y).Mul(Consts.EDWARDS_D2);
            return new AffineNielsPoint
            {
                Y_plus_X = y.Add(x),
                Y_minus_X = y.Sub(x),
                XY2d = xy2d
            };
        }
    }
}
