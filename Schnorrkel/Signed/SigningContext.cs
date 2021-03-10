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

namespace Schnorrkel.Signed
{
    internal interface ISigningContext
    {
        Transcript Bytes(byte[] data);
        Transcript GetTranscript();
        Transcript Xof();
        Transcript Hash256();
        Transcript Hash512();
    }

    internal class SigningContext011 : ISigningContext
    {
        public Transcript ts;

        public SigningContext011(byte[] context)
        {
            ts = new Transcript(context);
            //   ts.AppendMessage(string.Empty, context);
        }

        //public SigningContext011(byte[] context)
        //{
        //    ts = new Transcript("SigningContext");
        //    ts.AppendMessage(string.Empty, context);
        //}

        public Transcript Bytes(byte[] data)
        {
            var clone = ts.Clone();
            clone.AppendMessage("sign-bytes", data);
            return clone;
        }

        public Transcript GetTranscript()
        {
            return ts;
        }

        public Transcript Xof()
        {
            throw new NotImplementedException();
        }

        public Transcript Hash256()
        {
            throw new NotImplementedException();
        }

        public Transcript Hash512()
        {
            throw new NotImplementedException();
        }
    }

    internal class SigningContext085 : ISigningContext
    {
        public Transcript ts;

        public SigningContext085(byte[] context)
        {
            ts = new Transcript("SigningContext");
            ts.AppendMessage(string.Empty, context);
        }

        //public SigningContext011(byte[] context)
        //{
        //    ts = new Transcript("SigningContext");
        //    ts.AppendMessage(string.Empty, context);
        //}

        public Transcript Bytes(byte[] data)
        {
            var clone = ts.Clone();
            clone.AppendMessage("sign-bytes", data);
            return clone;
        }

        public Transcript GetTranscript()
        {
            return ts;
        }

        public Transcript Xof()
        {
            throw new NotImplementedException();
        }

        public Transcript Hash256()
        {
            throw new NotImplementedException();
        }

        public Transcript Hash512()
        {
            throw new NotImplementedException();
        }
    }
}
