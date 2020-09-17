using System;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel
{
    public class SignedExtensions
    {
        private short _specVersion;
        private short _txVersion;
        private byte[] _genesis;
        private byte[] _blockHash;
        private byte[] _mortality;
        private CompactInteger _nonce;
        private CompactInteger _chargeTransactionPayment;

        private void SetSpecVersion(short specVersion)
        {
            _specVersion = specVersion;
        }
        private void SetTxVersion(short txVersion)
        {
            _txVersion = txVersion;
        }

        private void SetGenesis(byte[] genesis)
        {
            _genesis = genesis;
        }

        private void SetMortality(byte[] mortality, byte[] blockhash)
        {
            _mortality = mortality;
            _blockHash = blockhash;
        }

        private void SetNonce(CompactInteger nonce)
        {
            _nonce = nonce;
        }

        private void SetChargeTransactionPayment(CompactInteger chargeTransactionPayment)
        {
            _chargeTransactionPayment = chargeTransactionPayment;
        }

        public byte[] Serialize()
        {
            var bytes = new List<byte>();

            /*
             * Extra: Era, Nonce & Tip
             * */  
            bytes.AddRange(_mortality); // CheckMortality
            bytes.AddRange(_nonce.Encode()); // CheckNonce
            bytes.AddRange(_chargeTransactionPayment.Encode()); // ChargeTransactionPayment

            /*
             * Additional Signed: SpecVersion, TxVersion, Genesis, Blockhash
             * */

            // CheckSpecVersion
            bytes.AddRange(Utils.Value2Bytes(_specVersion));

            // CheckTxVersion
            bytes.AddRange(Utils.Value2Bytes(_txVersion));

            // CheckGenesis
            bytes.AddRange(_genesis);

            // CheckMortality, Immortal = genesis_hash, Mortal = logic
            bytes.AddRange(_blockHash);
            
            return bytes.ToArray();
        }

    }
}
