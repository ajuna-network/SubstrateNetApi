using Schnorrkel.Merlin;
using System;

namespace Schnorrkel
{
    /// <summary>
    /// Hardcoded bytes for debug purposes
    /// </summary>
    public class Hardcoded : RandomGenerator
    {
        private byte[] res;
        public Hardcoded(byte[] validResult)
        {
            res = validResult;
        }

        public Hardcoded()
        {
            res = null;
        }

        public override void FillBytes(ref byte[] dst)
        {
            dst = new byte[] { 77, 196, 92, 65, 167, 196, 215, 23, 222, 26, 136, 164, 123, 67, 115, 78, 178, 96, 208, 59, 8, 157, 203, 111, 157, 87, 69, 105, 155, 61, 111, 153 };
        }

        public byte[] Result()
        {
            return res;
        }
    }

    public class Simple : RandomGenerator
    {
        static readonly Random _rnd;
        static Simple()
        {
            _rnd = new Random((int)DateTime.Now.Ticks);
        }

        public override void FillBytes(ref byte[] dst)
        {
            _rnd.NextBytes(dst);
        }
    }
}
