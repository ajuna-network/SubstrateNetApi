namespace Schnorrkel.Signed
{
    using Schnorrkel.Merlin;
    using Schnorrkel.Ristretto;
    using Schnorrkel.Scalars;

    internal class SigningTranscript
    {
        SigningTranscriptOperation _operations;
        ISigningContext _context;

        public SigningTranscript(SigningContext085 context)
        {
            _operations = new SigningTranscriptOperation();
            _context = context;
        }

        public SigningTranscript(SigningContext011 context)
        {
            _operations = new SigningTranscriptOperation();
            _context = context;
        }

        public void SetProtocolName(byte[] label)
        {
            _operations.CommitBytes(_context.GetTranscript(), "proto-name", label);
        }

        public void CommitPoint(byte[] label, CompressedRistretto compressed)
        {
            _operations.CommitBytes(_context.GetTranscript(), label, compressed.ToBytes());
        }

        public void CommitPoint(byte[] label, byte[] bytes)
        {
            _operations.CommitBytes(_context.GetTranscript(), label, bytes);
        }

        public Scalar WitnessScalar(byte[] label, byte[] bytes, RandomGenerator rng)
        {
            return _operations.WitnessScalar(_context.GetTranscript(), label, bytes, rng);
        }

        public Scalar WitnessScalar(byte[] bytes, RandomGenerator rng)
        {
            return _operations.WitnessScalar(_context.GetTranscript(), bytes, rng);
        }

        public Scalar ChallengeScalar(byte[] label)
        {
            var data = ChallengeBytes(label);
            return Scalar.FromBytesModOrderWide(data);
        }

        public byte[] ChallengeBytes(byte[] label)
        {
            var result = _operations.ChallengeBytes(_context.GetTranscript(), label);
            return result;
        }
    }

    internal class SigningTranscriptOperation
    {
        public byte[] ChallengeBytes(Transcript ts, byte[] label)
        {
            var buf = new byte[64];
            ts.ChallengeBytes(label, ref buf);
            return buf;
        }

        public void CommitBytes(Transcript ts, byte[] label, byte[] bytes)
        {
            ts.AppendMessage(label, bytes);
        }

        public void CommitBytes(Transcript ts, string label, byte[] bytes)
        {
            ts.AppendMessage(label, bytes);
        }

        public void CommitPoint(Transcript ts, byte[] label, byte[] compressedRistretto)
        {
            CommitBytes(ts, label, compressedRistretto);
        }

        public Scalar WitnessScalar(Transcript ts, byte[] nonce, RandomGenerator rng)
        {
            byte[] bt = new byte[64];
            bt.Initialize();
            ts.WitnessBytes(ref bt, nonce, rng);

            return Scalar.FromBytesModOrderWide(bt);
        }

        public Scalar WitnessScalar(Transcript ts, byte[] label, byte[] nonce, RandomGenerator rng)
        {
            byte[] bt = new byte[64];
            bt.Initialize();
            ts.WitnessBytes(label, ref bt, nonce, rng);

            return Scalar.FromBytesModOrderWide(bt);
        }
    }
}
