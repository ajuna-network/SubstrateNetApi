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
using Schnorrkel.Keys;
using Schnorrkel.Merlin;
using Schnorrkel.Ristretto;
using Schnorrkel.Scalars;
using Schnorrkel.Signed;
using System.Text;

namespace Schnorrkel
{
    public class Sr25519v091 : Sr25519Base
    {
        public Sr25519v091(SchnorrkelSettings settings) : base(settings)
        {
        }

        public static byte[] SignSimple(KeyPair pair, byte[] message)
        {
            var signingContext = new SigningContext085(Encoding.UTF8.GetBytes("substrate"));
            var st = new SigningTranscript(signingContext);
            signingContext.ts = signingContext.Bytes(message);
            var rng = new Simple();
            var sig = Sign(st, pair.Secret, pair.Public, rng);

            return sig.ToBytes();
        }

        public static byte[] SignSimple(byte[] publicKey, byte[] secretKey, byte[] message)
        {
            var sk = SecretKey.FromBytes085(secretKey);
            var pk = new PublicKey(publicKey);
            var signingContext = new SigningContext085(Encoding.UTF8.GetBytes("substrate"));
            var st = new SigningTranscript(signingContext);
            signingContext.ts = signingContext.Bytes(message);
            var rng = new Simple();
            var sig = Sign(st, sk, pk, rng);

            return sig.ToBytes();
        }

        public static bool Verify(byte[] signature, PublicKey publicKey, byte[] message)
        {
            var s = new Signature();
            s.FromBytes(signature);
            var signingContext = new SigningContext085(Encoding.UTF8.GetBytes("substrate"));
            var st = new SigningTranscript(signingContext);
            signingContext.ts = signingContext.Bytes(message);

            return Verify(st, s, publicKey);
        }

        public static bool Verify(byte[] signature, byte[] publicKey, byte[] message)
        {
            var s = new Signature();
            s.FromBytes(signature);
            var pk = new PublicKey(publicKey);
            var signingContext = new SigningContext085(Encoding.UTF8.GetBytes("substrate"));
            var st = new SigningTranscript(signingContext);
            signingContext.ts = signingContext.Bytes(message);

            return Verify(st, s, pk);
        }
        internal static bool Verify(SigningTranscript st, Signature sig, PublicKey publicKey)
        {
            st.SetProtocolName(GetStrBytes("Schnorr-sig"));
            st.CommitPoint(GetStrBytes("sign:pk"), publicKey.Key);
            st.CommitPoint(GetStrBytes("sign:R"), sig.R);

            var k = st.ChallengeScalar(GetStrBytes("sign:c")); // context, message, A/public_key, R=rG
            var A = publicKey.GetEdwardsPoint();
            var negA = A.Negate();

            var R = RistrettoPoint.VartimeDoubleScalarMulBasepoint(k, negA, sig.S);

            return new RistrettoPoint(R).Compress().Equals(sig.R);
        }

        internal static Signature Sign(SigningTranscript st, SecretKey secretKey, PublicKey publicKey, RandomGenerator rng)
        {
            st.SetProtocolName(GetStrBytes("Schnorr-sig"));
            st.CommitPoint(GetStrBytes("sign:pk"), publicKey.Key);

            var r = st.WitnessScalar(GetStrBytes("signing"), secretKey.nonce, rng);

            var tbl = new RistrettoBasepointTable();
            var R = tbl.Mul(r).Compress();

            st.CommitPoint(GetStrBytes("sign:R"), R);

            Scalar k = st.ChallengeScalar(GetStrBytes("sign:c"));  // context, message, A/public_key, R=rG
            k.Recalc();
            secretKey.key.Recalc();
            r.Recalc();

            var scalar = k.ScalarInner * secretKey.key.ScalarInner + r.ScalarInner;

            var s = new Scalar { ScalarBytes = scalar.ToBytes() };
            s.Recalc();

            return new Signature { R = R, S = s };
        }
    }
}
