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
