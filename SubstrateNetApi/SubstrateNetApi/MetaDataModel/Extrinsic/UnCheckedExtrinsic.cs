using Schnorrkel.Signed;
using SubstrateNetApi.MetaDataModel.Extrinsic;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace SubstrateNetApi.MetaDataModel
{
    public class UnCheckedExtrinsic
    {
        const uint SPEC_VERSION = 1;

        const uint TX_VERSION = 1;

        const ulong EXTRINSIC_ERA_PERIOD_DEFAULT = 64;

        private bool _signed;

        private byte[] _sendPublicKey;

        private byte _sendPublicKeyType;

        private Era _era;

        private CompactInteger _nonce;

        private CompactInteger _tip;

        private Method _method;

        private Hash _genesis;

        private Hash _startEra;

        private byte[] _signature;

        public UnCheckedExtrinsic(bool signed, byte publicKeyType, byte[] publicKey, CompactInteger nonce, byte module, byte call, byte[] parameters, byte[] genesisHash, byte[] currentBlockHash, ulong currentBlockNumber, CompactInteger tip)
        {
            _signed = signed;
            _sendPublicKey = publicKey;
            _sendPublicKeyType = publicKeyType;
            _nonce = nonce;
            _method = new Method(module, call, parameters);
            _era = new Era(EXTRINSIC_ERA_PERIOD_DEFAULT, currentBlockNumber, currentBlockNumber == 0 ? true : false);
            _genesis = new Hash(genesisHash);
            _startEra = new Hash(currentBlockHash);
            _tip = tip;
        }

        public UnCheckedExtrinsic(bool signed, Method method, Era era, uint nonce, uint tip, Hash genesis, Hash startEra)
        {
            _signed = signed;
            _method = method;
            _era = era;
            _nonce = nonce;
            _tip = tip;
            _genesis = genesis;
            _startEra = startEra;
        }

        public Payload GetPayload()
        {
            return new Payload(_method, new SignedExtensions(SPEC_VERSION, TX_VERSION, _genesis, _startEra, _era, _nonce, _tip));
        }

        public void AddPayloadSignature(byte[] signature)
        {
            _signature = signature;
        }

        public byte[] Encode()
        {
            if (_signed && _signature == null)
            {
                throw new Exception("Missing payload signature for signed transaction.");
            }

            var list = new List<byte>();
            
            // 4 is the TRANSACTION_VERSION constant and it is 7 bits long, the highest bit 1 for signed transaction, 0 for unsigned.
            list.Add((byte)(4 | (_signed ? 0x80 : 0)));

            // 32 bytes
            list.AddRange(_sendPublicKey);

            // key type ed = 00 and sr = FF
            list.Add(_sendPublicKeyType);

            list.AddRange(_signature);
            
            list.AddRange(_era.Encode());
            
            list.AddRange(_nonce.Encode());
            
            list.AddRange(_tip.Encode());
            
            list.AddRange(_method.Encode());
           
            return Utils.SizePrefixedByteArray(list); ;
        }
    }
}
