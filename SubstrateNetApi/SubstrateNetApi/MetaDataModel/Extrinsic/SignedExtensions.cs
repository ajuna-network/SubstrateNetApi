using System;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel
{
    public class SignedExtensions
    {
        private uint _specVersion;

        private uint _txVersion;

        private byte[] _genesis;

        private byte[] _blockHash;

        private byte[] _mortality;

        private CompactInteger _nonce;

        private CompactInteger _chargeTransactionPayment;

        public void SetSpecVersion(uint specVersion)
        {
            _specVersion = specVersion;
        }
        public void SetTxVersion(uint txVersion)
        {
            _txVersion = txVersion;
        }

        public void SetGenesis(byte[] genesis)
        {
            _genesis = genesis;
        }

        public void SetMortality(byte[] mortality, byte[] blockhash)
        {
            _mortality = mortality;
            _blockHash = blockhash;
        }

        public void SetNonce(CompactInteger nonce)
        {
            _nonce = nonce;
        }

        public void SetChargeTransactionPayment(CompactInteger chargeTransactionPayment)
        {
            _chargeTransactionPayment = chargeTransactionPayment;
        }

        public byte[] GetExtra()
        {
            var bytes = new List<byte>();
            
            // CheckMortality
            bytes.AddRange(_mortality);

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
            bytes.AddRange(_genesis);

            // CheckMortality, Additional Blockhash check. Immortal = genesis_hash, Mortal = logic
            bytes.AddRange(_blockHash);

            return bytes.ToArray();
        }

        public byte[] Serialize()
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
