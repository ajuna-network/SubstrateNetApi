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
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace Schnorrkel.Keys
{
    public class MiniSecret
    {
        private Scalar secret;
        private byte[] nonce;

        public MiniSecret(byte[] miniKey, ExpandMode expandMode)
        {
            switch (expandMode)
            {
                case ExpandMode.Uniform:
                    {
                        ExpandUniform(miniKey);
                        break;
                    }
                case ExpandMode.Ed25519:
                    {
                        ExpandEd25519(miniKey);
                        break;
                    }
                default:
                {
                    throw new InvalidEnumArgumentException(nameof(expandMode), (int) expandMode, typeof(ExpandMode));
                }
            }
        }

        public KeyPair GetPair()
        {
            return new KeyPair(ExpandToPublic(), 
                new SecretKey
                {
                    key = secret,
                    nonce = nonce
                });
        }

        public SecretKey ExpandToSecret()
        {
            return new SecretKey
            {
                key = secret,
                nonce = nonce
            };
        }

        public PublicKey ExpandToPublic()
        {
            var tbl = new RistrettoBasepointTable();
            var R = tbl.Mul(secret).Compress();

            return new PublicKey(R.ToBytes());
        }

        private void ExpandUniform(byte[] miniKey)
        {
            Transcript ts = new Transcript("ExpandSecretKeys");
            ts.AppendMessage("mini", miniKey);

            var scalar_bytes = new byte[64];
            ts.ChallengeBytes(Encoding.UTF8.GetBytes("sk"), ref scalar_bytes);

            secret = Scalar.FromBytesModOrderWide(scalar_bytes);

            nonce = new byte[32];
            ts.ChallengeBytes(Encoding.UTF8.GetBytes("no"), ref nonce);
        }

        private void ExpandEd25519(byte[] miniKey)
        {
            SHA512 shaM = new SHA512Managed();
            var shaHash = shaM.ComputeHash(miniKey);

            // We need not clamp in a Schnorr group like Ristretto, but here
            // we do so to improve Ed25519 comparability.
            var key = shaHash.AsMemory().Slice(0, 32).ToArray();
            nonce = shaHash.AsMemory().Slice(32, 32).ToArray();
            key[0] &= 248;
            key[31] &= 63;
            key[31] |= 64;

            // We then divide by the cofactor to internally keep a clean
            // representation mod l.
            Scalar.DivideScalarBytesByCofactor(ref key);
            secret = Scalar.FromBits(key);
        }
    }

    public enum ExpandMode
    {
        Uniform, Ed25519
    }
}
