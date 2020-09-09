namespace Schnorrkel.Scalars
{
    public class UnpackedScalar
    {
        public static Scalar Pack(Scalar52 scalar)
        {
            var sc = new Scalar { ScalarBytes = scalar.ToBytes() };
            sc.Recalc();
            return sc;
        }

        public static Scalar52 FromBytes(byte[] data)
        {
            ulong[] words = new ulong[4];
            words.Initialize();

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    words[i] |= (ulong)data[(i * 8) + j] << (j * 8);
                }
            }

            ulong mask = (1L << 52) - 1;
            ulong topMask = (1L << 48) - 1;
            var s = Scalar52.Zero();

            s[0] = words[0] & mask;
            s[1] = ((words[0] >> 52) | (words[1] << 12)) & mask;
            s[2] = ((words[1] >> 40) | (words[2] << 24)) & mask;
            s[3] = ((words[2] >> 28) | (words[3] << 36)) & mask;
            s[4] = (words[3] >> 16) & topMask;

            return s;
        }

        public static Scalar52 FromBytesWide(byte[] data)
        {
            ulong[] words = new ulong[8];
            words.Initialize();

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    words[i] |= ((ulong)data[(i * 8) + j]) << (j * 8);
                }
            }

            ulong mask = (1L << 52) - 1;
            var lo = Scalar52.Zero();
            var hi = Scalar52.Zero();

            lo[0] = words[0] & mask;
            lo[1] = ((words[0] >> 52) | (words[1] << 12)) & mask;
            lo[2] = ((words[1] >> 40) | (words[2] << 24)) & mask;
            lo[3] = ((words[2] >> 28) | (words[3] << 36)) & mask;
            lo[4] = ((words[3] >> 16) | (words[4] << 48)) & mask;
            hi[0] = (words[4] >> 4) & mask;
            hi[1] = ((words[4] >> 56) | (words[5] << 8)) & mask;
            hi[2] = ((words[5] >> 44) | (words[6] << 20)) & mask;
            hi[3] = ((words[6] >> 32) | (words[7] << 32)) & mask;
            hi[4] = words[7] >> 20;

            lo = Scalar52.MontgomeryMul(lo, Consts.R);  // (lo * R) / R = lo
            hi = Scalar52.MontgomeryMul(hi, Consts.RR); // (hi * R^2) / R = hi * R

            return Scalar52.Add(hi, lo);
        }
    }
}
