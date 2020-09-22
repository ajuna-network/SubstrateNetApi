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
        private readonly bool _signed;

        private readonly Account _account;

        private readonly Era _era;

        private readonly CompactInteger _nonce;

        private readonly CompactInteger _tip;

        private readonly Method _method;

        private readonly Hash _genesis;

        private readonly Hash _startEra;

        private byte[] _signature;

        public byte[] PayloadSignature => _signature;

        public UnCheckedExtrinsic(bool signed, Account account, CompactInteger nonce, byte module, byte call, byte[] parameters, byte[] genesisHash, byte[] currentBlockHash, ulong currentBlockNumber, CompactInteger tip)
        {
            _signed = signed;
            _account = account;
            _nonce = nonce;
            _method = new Method(module, call, parameters);
            _era = new Era(Constants.EXTRINSIC_ERA_PERIOD_DEFAULT, currentBlockNumber, currentBlockNumber == 0 ? true : false);
            _genesis = new Hash(genesisHash);
            _startEra = new Hash(currentBlockHash);
            _tip = tip;
        }

        public UnCheckedExtrinsic(bool signed, Account account, Method method, Era era, uint nonce, uint tip, Hash genesis, Hash startEra)
        {
            _signed = signed;
            _account = account;
            _method = method;
            _era = era;
            _nonce = nonce;
            _tip = tip;
            _genesis = genesis;
            _startEra = startEra;
        }

        public Payload GetPayload()
        {
            return new Payload(_method, new SignedExtensions(Constants.SPEC_VERSION, Constants.TX_VERSION, _genesis, _startEra, _era, _nonce, _tip));
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
            list.AddRange(_account.PublicKey);

            // key type ed = 00 and sr = FF
            list.Add(_account.KeyTypeByte);

            list.AddRange(_signature);
            
            list.AddRange(_era.Encode());
            
            list.AddRange(_nonce.Encode());
            
            list.AddRange(_tip.Encode());
            
            list.AddRange(_method.Encode());
           
            return Utils.SizePrefixedByteArray(list); ;
        }
    }
}
