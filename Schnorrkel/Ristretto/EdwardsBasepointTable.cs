namespace Schnorrkel.Ristretto
{
    using Schnorrkel.Scalars;

    public class EdwardsBasepointTable
    {
        public LookupTable[] lt;

        public EdwardsPoint Mul(Scalar sclr)
        {
            var a = sclr.ToRadix16();
            var P = EdwardsPoint.Identity();

            for (var i = 0; i < 64; i++)
            {
                if (i % 2 == 1)
                {
                    var s1 = lt[i / 2].Select(a[i]);
                    var s2 = P.Add(s1);
                    var s3 = s2.ToExtended();

                    P = s3;
                }
            }

            P = P.MulByPow2(4);

            for (var i = 0; i < 64; i++)
            {
                if (i % 2 == 0)
                {
                    P = P.Add(lt[i / 2].Select(a[i])).ToExtended();
                }
            }

            return P;
        }

        public class LookupTable
        {
            private EdwardsPoint _ep;
            public AffineNielsPoint[] affineNielsPoints { get; set; }

            public LookupTable() { }

            public LookupTable(EdwardsPoint ep)
            {
                _ep = ep;
                affineNielsPoints = new AffineNielsPoint[8];
                affineNielsPoints[0] = ep.ToAffineNiels();
                for (var j = 0; j < 7; j++)
                {
                    affineNielsPoints[j + 1] = ep.Add(affineNielsPoints[j]).ToExtended().ToAffineNiels();
                }
            }

            public AffineNielsPoint Select(sbyte x)
            {
                // Compute xabs = |x|
                var xmask = x >> 7;
                sbyte xabs = (sbyte)((x + xmask) ^ xmask);

                // Set t = 0 * P = identity
                var t = new AffineNielsPoint();
                for (var i = 1; i < 9; i++)
                {
                    // Copy `points[j-1] == j*P` onto `t` in constant time if `|x| == j`.
                    t.ConditionalAssign(affineNielsPoints[i - 1], xabs == i);
                }

                // Now t == |x| * P.
                byte neg_mask = (byte)(xmask & 1);
                t.ConditionalNegate(neg_mask == 1);
                // Now t == x * P.

                return t;
            }

            public static LookupTable From(EdwardsPoint ep)
            {
                return new LookupTable(ep);
            }
        }
    }
}
