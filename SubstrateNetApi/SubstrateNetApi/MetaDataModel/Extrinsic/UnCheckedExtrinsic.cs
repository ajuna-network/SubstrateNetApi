using SubstrateNetApi.MetaDataModel.Extrinsic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace SubstrateNetApi.MetaDataModel
{
    public class UnCheckedExtrinsic
    {
        private bool _signed;

        private int _transactionVersion;

        private byte[] _sendPublicKey;

        private byte[] _sendPublicKeyType;

        private Era _era;

        private CompactInteger _nonce;

        private CompactInteger _tip;

        private Method _method;

        private byte[] _signature;

        public UnCheckedExtrinsic(bool signed, byte[] publicKey, CompactInteger nonce, byte module, byte call, byte[] parameters, ulong blockNumber)
        {
            _signed = signed;
            _sendPublicKey = publicKey;
            _nonce = nonce;
            _method = new Method(module, call, parameters);
            _era = new Era(64, blockNumber);
        }

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

        public void SetNonce(CompactInteger nonce)
        {
            _nonce = nonce;
        }

        public void SetTip(CompactInteger tip)
        {
            _tip = tip;
        }

        public void SetSignature(byte[] signedPayload)
        {
            _signature = signedPayload;
        }

        public Payload GetPayload()
        {
            //var tt = new SignedExtensions();
            return new Payload(_method, null);
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
            
            list.AddRange(_era.Encode());
            
            list.AddRange(_nonce.Encode());
            
            list.AddRange(_tip.Encode());
            
            list.AddRange(_method.Serialize());

            // add length
            var result = new List<byte>();
            result.AddRange(new CompactInteger(list.Count).Encode());
            result.AddRange(list);
            
            return result.ToArray();
        }
    }
}
