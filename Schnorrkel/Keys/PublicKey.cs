using Schnorrkel.Ristretto;

namespace Schnorrkel
{
    public class PublicKey
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
