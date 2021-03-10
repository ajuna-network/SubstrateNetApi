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
using Schnorrkel.Ristretto;
using Schnorrkel.Scalars;
using System;

namespace Schnorrkel.Signed
{
    public struct Signature
    {
        public CompressedRistretto R { get; set; }
        public Scalar S { get; set; }

        public void FromBytes011(byte[] signatureBytes)
        {
            var r = new CompressedRistretto(signatureBytes.AsMemory(0, 32).ToArray());
            var s = new Scalar();
            s.ScalarBytes = new byte[32];
            signatureBytes.AsMemory(32, 32).CopyTo(s.ScalarBytes);
            s.Recalc();

            R = r;
            S = s;
        }

        public byte[] ToBytes011()
        {
            var bytes = new byte[Consts.SIGNATURE_LENGTH];
            R.ToBytes().AsMemory().CopyTo(bytes.AsMemory(0, 32));
            S.ScalarBytes.AsMemory().CopyTo(bytes.AsMemory(32, 32));
            return bytes;
        }

        public void FromBytes(byte[] signatureBytes)
        {
            if ((signatureBytes[63] & 128) == 0)
            {
                throw new Exception("Signature bytes not marked as a schnorrkel signature");
            }

            // remove schnorrkel signature mark
            signatureBytes[63] &= 127;
            var r = new CompressedRistretto(signatureBytes.AsMemory(0, 32).ToArray());
            var s = new Scalar();
            s.ScalarBytes = new byte[32];
            signatureBytes.AsMemory(32, 32).CopyTo(s.ScalarBytes);
            s.Recalc();

            R = r;
            S = s;
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[Consts.SIGNATURE_LENGTH];
            R.ToBytes().AsMemory().CopyTo(bytes.AsMemory(0, 32));
            S.ScalarBytes.AsMemory().CopyTo(bytes.AsMemory(32, 32));
            // add schnorrkel signature mark
            bytes[63] |= 128;
            return bytes;
        }
    }
}
