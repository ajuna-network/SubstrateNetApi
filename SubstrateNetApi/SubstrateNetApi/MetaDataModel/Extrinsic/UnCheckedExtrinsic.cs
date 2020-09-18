using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.MetaDataModel
{
    public class UnCheckedExtrinsic
    {
        private bool _signed;

        private int _transactionVersion;

        private byte[] _sendPublicKey;

        private byte[] _sendPublicKeyType;

        private byte[] _era;

        private CompactInteger _nonce;

        private CompactInteger _tip;

        private byte[] _call;

        private byte[] _signature;

        public void IsSigned(bool flag)
        {
            _signed = flag;
        }

        public void SetTransactionVersion(int transactionVersion)
        {
            _transactionVersion = transactionVersion;
        }

        public void SetSenderPublicKey(byte[] sendPublicKey)
        {
            _sendPublicKey = sendPublicKey;
        }

        public void SetSenderPublicKeyType(byte[] sendPublicKeyType)
        {
            _sendPublicKeyType = sendPublicKeyType;
        }

        public void SetEra(byte[] era)
        {
            _era = era;
        }

        public void SetNonce(CompactInteger nonce)
        {
            _nonce = nonce;
        }

        public void SetTip(CompactInteger tip)
        {
            _tip = tip;
        }

        public void SetCall(byte[] moduleIndex)
        {
            _call = moduleIndex;
        }

        public void SetSignature(byte[] signedPayload)
        {
            _signature = signedPayload;
        }

        public Payload GetPayload()
        {
            return new Payload(null, null);
        }

        public byte[] Serialize(byte[] signedPayload)
        {
            var list = new List<byte>();
            
            //4 is the TRANSACTION_VERSION constant and it is 7 bits long, the highest bit 1 for signed transaction, 0 for unsigned.
            list.Add((byte)(_transactionVersion | (_signed ? 0x80 : 0)));

            // 32 bytes
            list.AddRange(_sendPublicKey);

            // key type ed = 00 and sr = FF
            list.AddRange(_sendPublicKeyType);

            list.AddRange(signedPayload);
            
            list.AddRange(_era);
            
            list.AddRange(_nonce.Encode());
            
            list.AddRange(_tip.Encode());
            
            list.AddRange(_call);

            // add length
            var result = new List<byte>();
            result.AddRange(new CompactInteger(list.Count).Encode());
            result.AddRange(list);
            
            return result.ToArray();
        }
    }
}
