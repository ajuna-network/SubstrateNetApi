using Newtonsoft.Json;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.MetaDataModel.Extrinsic
{
    public class Extrinsic
    {
        public byte[] Bytes;

        public CompactInteger Length;

        public byte SignatureVersion;

        public Account Sender;

        public byte[] Signature;

        public Era Era;

        public CompactInteger Nonce;

        public CompactInteger Tip;

        public Method Method;

        public Extrinsic(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal Extrinsic(Memory<byte> memory)
        {
            Bytes = memory.ToArray();

            int p = 0;
            int m;

            // length
            Length = CompactInteger.Decode(memory.ToArray(), ref p);

            // signature version
            m = 1;
            SignatureVersion = memory.Slice(p, m).ToArray()[0];
            p += m;

            // sender public key
            m = 32;
            var _senderPublicKey = memory.Slice(p, m).ToArray();
            p += m;

            // sender public key type
            m = 1;
            var _senderPublicKeyType = memory.Slice(p, m).ToArray()[0];
            p += m;

            Sender = new Account((KeyType)_senderPublicKeyType, _senderPublicKey);

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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
