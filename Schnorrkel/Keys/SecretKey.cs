using Schnorrkel.Ristretto;
using Schnorrkel.Scalars;
using System;

namespace Schnorrkel
{
    public struct SecretKey
    {
        /// Actual public key represented as a scalar.
        public Scalar key;
        /// Seed for deriving the nonces used in signing.
        ///
        /// We require this be random and secret or else key compromise attacks will ensue.
        /// Any modificaiton here may dirupt some non-public key derivation techniques.
        public byte[] nonce; //[u8; 32],

        public byte[] ToBytes()
        {
            var result = new byte[64];
            key.ScalarBytes.CopyTo(result, 0);
            nonce.CopyTo(result, 32);
            return result;
        }

        public PublicKey ExpandToPublic()
        {
            var tbl = new RistrettoBasepointTable();
            var R = tbl.Mul(key).Compress();

            return new PublicKey(R.ToBytes());
        }

        public static SecretKey FromBytes085(byte[] data)
        {
            if (data.Length != Consts.SIGNATURE_LENGTH)
                throw new Exception("SecretKey - SignatureError::BytesLengthError");

            // var key = data.AsMemory().Slice(0, 32).ToArray();
            return new SecretKey
            {
                key = new Scalar { ScalarBytes = data.AsMemory().Slice(0, 32).ToArray() },
                nonce = data.AsMemory().Slice(32, 32).ToArray()
            };
        }

        public static SecretKey FromBytes011(byte[] data)
        {
            if (data.Length != Consts.SIGNATURE_LENGTH)
                throw new Exception("SecretKey - SignatureError::BytesLengthError");

            // TODO:  We should consider making sure the scalar is valid,
            // maybe by zering the high bit, orp referably by checking < l.
            // key[31] &= 0b0111_1111;
            // We devide by the cofactor to internally keep a clean
            // representation mod l.
            var key = data.AsMemory().Slice(0, 32).ToArray();
            Scalar.DivideScalarBytesByCofactor(ref key);//::divide_scalar_bytes_by_cofactor(&mut key);

            return new SecretKey
            {
                key = new Scalar { ScalarBytes = key },
                nonce = data.AsMemory().Slice(32, 32).ToArray()
            };
        }
    }
}
