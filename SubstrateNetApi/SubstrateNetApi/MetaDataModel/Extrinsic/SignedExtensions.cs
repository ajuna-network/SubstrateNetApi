using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel
{
    public class SignedExtensions
    {
        private CompactInteger _specVersion;
        private CompactInteger _txVersion;
        private byte[] _genesis;
        private byte[] _mortality;
        private CompactInteger _nonce;
        private CompactInteger _chargeTransactionPayment;

        private void SetSpecVersion(CompactInteger specVersion)
        {
            _specVersion = specVersion;
        }
        private void SetTxVersion(CompactInteger txVersion)
        {
            _txVersion = txVersion;
        }

        private void SetGenesis(byte[] genesis)
        {
            _genesis = genesis;
        }

        private void SetMortality(byte[] mortality)
        {
            _mortality = mortality;
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

            // CheckSpecVersion
            bytes.AddRange(((CompactInteger)1).Encode());

            // CheckTxVersion
            bytes.AddRange(((CompactInteger)1).Encode());

            // CheckGenesis
            bytes.AddRange(Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9"));

            // CheckMortality
            bytes.AddRange(Utils.HexToByteArray("0x2503"));

            // CheckNonce
            bytes.AddRange(((CompactInteger)3).Encode());

            // CheckWeight
            bytes.AddRange(((CompactInteger)0).Encode());

            // ChargeTransactionPayment
            bytes.AddRange(((CompactInteger)0).Encode());

            return bytes.ToArray();
        }
    }
}
