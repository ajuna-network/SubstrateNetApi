﻿namespace Schnorrkel
{
    using Schnorrkel.Merlin;
    using System.Text;

    public abstract class Sr25519Base
    {
        private RandomGenerator _rng;

        public Sr25519Base(SchnorrkelSettings settings)
        {
            _rng = settings.RandomGenerator;
        }

        protected static byte[] GetStrBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
    }
}
