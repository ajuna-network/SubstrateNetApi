using SubstrateNetApi.MetaDataModel.Extrinsic;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel
{
    public class SignedExtensions
    {
        private uint _specVersion;

        private uint _txVersion;

        private byte[] _genesisHash;

        private byte[] _blockHash;

        private Era _mortality;

        private CompactInteger _nonce;

        private CompactInteger _chargeTransactionPayment;

        public SignedExtensions(uint specVersion, uint txVersion, byte[] genesisHash, byte[] blockHash, Era mortality, CompactInteger nonce, CompactInteger chargeTransactionPayment)
        {
            _specVersion = specVersion;
            _txVersion = txVersion;
            _genesisHash = genesisHash;
            _blockHash = blockHash;
            _mortality = mortality;
            _nonce = nonce;
            _chargeTransactionPayment = chargeTransactionPayment;
        }

        public byte[] GetExtra()
        {
            var bytes = new List<byte>();
           
            // CheckMortality
            bytes.AddRange(_mortality.Encode());

            // CheckNonce
            bytes.AddRange(_nonce.Encode());

            // ChargeTransactionPayment
            bytes.AddRange(_chargeTransactionPayment.Encode());
            
            return bytes.ToArray();
        }

        public byte[] GetAdditionalSigned()
        {
            var bytes = new List<byte>();

            // CheckSpecVersion
            bytes.AddRange(Utils.Value2Bytes(_specVersion));

            // CheckTxVersion
            bytes.AddRange(Utils.Value2Bytes(_txVersion));

            // CheckGenesis
            bytes.AddRange(_genesisHash);

            // CheckMortality, Additional Blockhash check. Immortal = genesis_hash, Mortal = logic
            bytes.AddRange(_blockHash);

            return bytes.ToArray();
        }

        public byte[] Encode()
        {
            var bytes = new List<byte>();

            // Extra: Era, Nonce & Tip
            bytes.AddRange(GetExtra());

            // Additional Signed: SpecVersion, TxVersion, Genesis, Blockhash
            bytes.AddRange(GetAdditionalSigned());
            
            return bytes.ToArray();
        }

    }
}
