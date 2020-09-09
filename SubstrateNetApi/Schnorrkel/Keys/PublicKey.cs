using System;
using Schnorrkel.Ristretto;

namespace Schnorrkel
{
    internal class PublicKey
    {
        public byte[] Key { get; }

        public PublicKey(byte[] keyBytes)
        {
            Key = keyBytes;
        }

        internal EdwardsPoint GetEdwardsPoint()
        {
            return EdwardsPoint.Decompress(Key);
        }
    }
}
