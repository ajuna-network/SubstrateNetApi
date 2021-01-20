using SubstrateNetApi.Model.Types;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Extrinsics
{
    public class SignedExtensions
    {
        private uint _specVersion;

        private uint _txVersion;

        private Hash _genesis;

        private Hash _startEra;

        private Era _mortality;

        private CompactInteger _nonce;

        private CompactInteger _chargeTransactionPayment;

        public SignedExtensions(uint specVersion, uint txVersion, Hash genesis, Hash startEra, Era mortality, CompactInteger nonce, CompactInteger chargeTransactionPayment)
        {
            _specVersion = specVersion;
            _txVersion = txVersion;
            _genesis = genesis;
            _startEra = startEra;
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
            bytes.AddRange(_genesis.Bytes);

            // CheckMortality, Additional Blockhash check. Immortal = genesis_hash, Mortal = logic
            bytes.AddRange(_startEra.Bytes);

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
