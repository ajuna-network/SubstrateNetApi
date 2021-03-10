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
using Schnorrkel.Merlin;
using Schnorrkel.Ristretto;
using Schnorrkel.Scalars;
using Schnorrkel.Signed;
using System.Text;

namespace Schnorrkel
{
    public class Sr25519v011 : Sr25519Base
    {
        private RandomGenerator _rng;

        public Sr25519v011(SchnorrkelSettings settings) : base(settings)
        {
            _rng = settings.RandomGenerator;
        }

        internal Signature Sign(SigningTranscript st, SecretKey secretKey, PublicKey publicKey)
        {
            return Sign(st, secretKey, publicKey, _rng);
        }

        public static byte[] SignSimple(byte[] publicKey, byte[] secretKey, byte[] message)
        {
            var sk = SecretKey.FromBytes011(secretKey);
            var pk = new PublicKey(publicKey);
            var signingContext = new SigningContext011(Encoding.UTF8.GetBytes("substrate"));
            var st = new SigningTranscript(signingContext);
            signingContext.ts = signingContext.Bytes(message);

            var rng = new Simple();

            var sig = Sign(st, sk, pk, rng);

            return sig.ToBytes011();
        }

        public static bool Verify(byte[] signature, byte[] publicKey, byte[] message)
        {
            var s = new Signature();
            s.FromBytes011(signature);
            var pk = new PublicKey(publicKey);
            var signingContext = new SigningContext011(Encoding.UTF8.GetBytes("substrate"));
            var st = new SigningTranscript(signingContext);
            signingContext.ts = signingContext.Bytes(message);

            return Verify(st, s, pk);
        }

        public static byte[] SignSimple(string publicKey, string secretKey, string message)
        {
            var sk = SecretKey.FromBytes011(Encoding.UTF8.GetBytes(secretKey));
            var pk = new PublicKey(Encoding.UTF8.GetBytes(publicKey));
            var signingContext = new SigningContext011(Encoding.UTF8.GetBytes("substrate"));
            var st = new SigningTranscript(signingContext);
            signingContext.ts = signingContext.Bytes(Encoding.UTF8.GetBytes(message));

            var rng = new Simple();

            var sig = Sign(st, sk, pk, rng);

            return sig.ToBytes011();
        }

        internal static bool Verify(SigningTranscript st, Signature sig, PublicKey publicKey)
        {
            st.SetProtocolName(GetStrBytes("Schnorr-sig"));
            st.CommitPoint(GetStrBytes("pk"), publicKey.Key);
            st.CommitPoint(GetStrBytes("no"), sig.R);

            var k = st.ChallengeScalar(GetStrBytes("")); // context, message, A/public_key, R=rG
            var ep = publicKey.GetEdwardsPoint().Negate();

            var R = RistrettoPoint.VartimeDoubleScalarMulBasepoint(k, ep, sig.S);

            return new RistrettoPoint(R).Compress().Equals(sig.R);
        }

        internal static Signature Sign(SigningTranscript st, SecretKey secretKey, PublicKey publicKey, RandomGenerator rng)
        {
            st.SetProtocolName(GetStrBytes("Schnorr-sig"));
            st.CommitPoint(GetStrBytes("pk"), publicKey.Key);

            var r = st.WitnessScalar(secretKey.nonce, rng);

            var tbl = new RistrettoBasepointTable();
            var mr = tbl.Mul(r);
            var R = tbl.Mul(r).Compress();

            st.CommitPoint(GetStrBytes("no"), R);

            Scalar k = st.ChallengeScalar(GetStrBytes(""));  // context, message, A/public_key, R=rG
            k.Recalc();
            secretKey.key.Recalc();
            r.Recalc();

            var t1 = k.ScalarInner * secretKey.key.ScalarInner;
            var t2 = t1 + r.ScalarInner;

            var scalar = k.ScalarInner * secretKey.key.ScalarInner + r.ScalarInner;

            var s = new Scalar { ScalarBytes = scalar.ToBytes() };
            s.Recalc();

            return new Signature { R = R, S = s };
        }
    }
}
