using Newtonsoft.Json;
using SubstrateNetApi.MetaDataModel.Values;
using System;

namespace SubstrateNetApi.MetaDataModel.Extrinsics
{
    public class Extrinsic
    {
        public bool Signed;

        public byte TransactionVersion;

        public Account Account;

        public Era Era;

        public CompactInteger Nonce;

        public CompactInteger Tip;

        public Method Method;

        public byte[] Signature;

        public Extrinsic(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal Extrinsic(Memory<byte> memory)
        {
            int p = 0;
            int m;

            // length
            var length = CompactInteger.Decode(memory.ToArray(), ref p);

            // signature version
            m = 1;
            var _signatureVersion = memory.Slice(p, m).ToArray()[0];
            Signed = _signatureVersion >= 0x80;
            TransactionVersion = (byte)(_signatureVersion - (Signed ? 0x80 : 0x00));
            p += m;

            // sender public key
            m = 32;
            var _senderPublicKey = memory.Slice(p, m).ToArray();
            p += m;

            // sender public key type
            m = 1;
            var _senderPublicKeyType = memory.Slice(p, m).ToArray()[0];
            p += m;

            Account = new Account((KeyType)_senderPublicKeyType, _senderPublicKey);

            // signature
            m = 64;
            Signature = memory.Slice(p, m).ToArray();
            p += m;

            // era
            m = 1;
            var era = memory.Slice(p, m).ToArray();
            if (era[0] != 0)
            {
                m = 2;
                era = memory.Slice(p, m).ToArray();
            }
            Era = Era.Decode(era);
            p += m;

            // nonce
            Nonce = CompactInteger.Decode(memory.ToArray(), ref p);

            // tip
            Tip = CompactInteger.Decode(memory.ToArray(), ref p);

            // method
            m = 2;
            var method = memory.Slice(p, m).ToArray();
            p += m;

            // parameters
            var parameter = memory.Slice(p).ToArray();

            Method = new Method(method[0], method[1], parameter);
        }

        public Extrinsic(bool signed, Account account, CompactInteger nonce, Method method, Era era, CompactInteger tip)
        {
            Signed = signed;
            TransactionVersion = Constants.EXTRINSIC_VERSION;
            Account = account;
            Era = era;
            Nonce = nonce;
            Tip = tip;
            Method = method;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
