using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel.Extrinsics
{
    public class UnCheckedExtrinsic : Extrinsic
    {
        private readonly Hash _genesis;

        private readonly Hash _startEra;

        public UnCheckedExtrinsic(bool signed, Account account, Method method, Era era, CompactInteger nonce, CompactInteger tip, Hash genesis, Hash startEra)
             : base(signed, account, nonce, method, era, tip)
        {
            _genesis = genesis;
            _startEra = startEra;
        }

        public Payload GetPayload()
        {
            return new Payload(Method, new SignedExtensions(Constants.SPEC_VERSION, Constants.TX_VERSION, _genesis, _startEra, Era, Nonce, Tip));
        }

        public void AddPayloadSignature(byte[] signature)
        {
            Signature = signature;
        }

        public byte[] Encode()
        {
            if (Signed && Signature == null)
            {
                throw new Exception("Missing payload signature for signed transaction.");
            }

            var list = new List<byte>();

            // 4 is the TRANSACTION_VERSION constant and it is 7 bits long, the highest bit 1 for signed transaction, 0 for unsigned.
            list.Add((byte)(Constants.EXTRINSIC_VERSION | (Signed ? 0x80 : 0)));

            // 32 bytes + prefix depending on address encoding in chain, see Constants.Address_version
            list.AddRange(Account.Encode());

            // key type ed = 00 and sr = FF
            list.Add(Account.KeyTypeByte);

            list.AddRange(Signature);

            list.AddRange(Era.Encode());

            list.AddRange(Nonce.Encode());

            list.AddRange(Tip.Encode());

            list.AddRange(Method.Encode());

            return Utils.SizePrefixedByteArray(list); ;
        }
    }
}
