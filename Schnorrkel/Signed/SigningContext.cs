namespace Schnorrkel.Signed
{
    using Schnorrkel.Merlin;
    using System;

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
