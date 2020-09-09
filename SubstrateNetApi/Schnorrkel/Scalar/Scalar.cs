namespace Schnorrkel.Scalars
{
    using System;
    using System.Linq;

    public class Scalar
    {
        /// `bytes` is a little-endian byte encoding of an integer representing a scalar modulo the
        /// group order.
        public byte[] ScalarBytes { get; set; }
        public Scalar52 ScalarInner { get; private set; }

        //public Scalar(byte[] scalarBytes)
        //{
        //    _scalarBytes = scalarBytes;
        //}

        public byte[] GetBytes()
        {
            return ScalarBytes;
        }

        public void Recalc()
        {
            ScalarInner = new Scalar52(ScalarBytes);
        }

        public sbyte[] ToRadix16()
        {
            var output = new sbyte[64];

            // Step 1: change radix.
            // Convert from radix 256 (bytes) to radix 16 (nibbles)
            var botHalf = new Func<byte, byte>(x => { return (byte)((x >> 0) & 15); });
            var topHalf = new Func<byte, byte>(x => { return (byte)((x >> 4) & 15); });

            for (var i = 0; i < 32; i++)
            {
                output[2 * i] = (sbyte)botHalf(ScalarBytes[i]);
                output[2 * i + 1] = (sbyte)topHalf(ScalarBytes[i]);
            }
            // Precondition note: since self[31] <= 127, output[63] <= 7

            // Step 2: recenter coefficients from [0,16) to [-8,8)
            for (var i = 0; i < 63; i++)
            {
                sbyte carry = (sbyte)((output[i] + 8) >> 4);
                output[i] -= (sbyte)(carry << 4);
                output[i + 1] += (sbyte)carry;
            }
            // Precondition note: output[63] is not recentered.  It
            // increases by carry <= 1.  Thus output[63] <= 8.

            return output;
        }

        public static void DivideScalarBytesByCofactor(ref byte[] bytes)
        {
            byte low = 0;
            // for i in scalar.iter_mut().rev() {
            for (var i = bytes.Length - 1; i >= 0; i-- )
            {
                var r = bytes[i] & 0b00000111; // save remainder
                bytes[i] >>= 3; // divide by 8
                bytes[i] += low;
                low = (byte)(r << 5);
            }
        }

        public sbyte[] NonAdjacentForm(int size)
        {
            sbyte[] naf = new sbyte[256];

            var xU64 = Scalar52.GetU64Data(ScalarBytes);

            var width = 1 << size;
            var window_mask = width - 1;

            var pos = 0;
            var carry = 0;
            while (pos < 256)
            {
                // Construct a buffer of bits of the scalar, starting at bit `pos`
                var u64_idx = pos / 64;
                var bit_idx = pos % 64;
                ulong bit_buf;
                if (bit_idx < 64 - size) {
                    // This window's bits are contained in a single u64
                    bit_buf = xU64[u64_idx] >> bit_idx;
                }
                else
                {
                    // Combine the current u64's bits with the bits from the next u64
                    bit_buf = (xU64[u64_idx] >> bit_idx) | (xU64[1 + u64_idx] << (64 - bit_idx));
                }

                // Add the carry into the current window
                var window = (ulong)carry + (bit_buf & (ulong)window_mask);

                if ((window & 1) == 0)
                {
                    // If the window value is even, preserve the carry and continue.
                    // Why is the carry preserved?
                    // If carry == 0 and window & 1 == 0, then the next carry should be 0
                    // If carry == 1 and window & 1 == 0, then bit_buf & 1 == 1 so the next carry should be 1
                    pos += 1;
                    continue;
                }

                if (window < (ulong)(width / 2))
                {
                    carry = 0;
                    naf[pos] = (sbyte)window;
                }
                else
                {
                    carry = 1;
                    naf[pos] = (sbyte)((sbyte)window - (sbyte)width);
                }

                pos += size;
            }

            return naf;
        }

        public static Scalar FromBytesModOrder(byte[] data)
        {
            var sc = UnpackedScalar.FromBytes(data);
            return UnpackedScalar.Pack(sc);
        }

        public static Scalar FromBytesModOrderWide(byte[] data)
        {
            var sc = UnpackedScalar.FromBytesWide(data);
            return UnpackedScalar.Pack(sc);
        }
    }
}
