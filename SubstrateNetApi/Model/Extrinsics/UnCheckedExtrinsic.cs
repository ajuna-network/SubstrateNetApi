using SubstrateNetApi.Model.Types;
using System;
using System.Collections.Generic;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Extrinsics
{
    public class UnCheckedExtrinsic : Extrinsic
    {
        private readonly Hash _genesis;

        private readonly Hash _startEra;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnCheckedExtrinsic"/> class.
        /// </summary>
        /// <param name="signed">if set to <c>true</c> [signed].</param>
        /// <param name="account">The account.</param>
        /// <param name="method">The method.</param>
        /// <param name="era">The era.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="tip">The tip.</param>
        /// <param name="genesis">The genesis.</param>
        /// <param name="startEra">The start era.</param>
        public UnCheckedExtrinsic(bool signed, Account account, Method method, Era era, CompactInteger nonce, CompactInteger tip, Hash genesis, Hash startEra)
             : base(signed, account, nonce, method, era, tip)
        {
            _genesis = genesis;
            _startEra = startEra;
        }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <param name="runtime">The runtime.</param>
        /// <returns></returns>
        public Payload GetPayload(RuntimeVersion runtime)
        {
            return new Payload(Method, new SignedExtensions(runtime.SpecVersion, runtime.TransactionVersion, _genesis, _startEra, Era, Nonce, Tip));
        }

        /// <summary>
        /// Adds the payload signature.
        /// </summary>
        /// <param name="signature">The signature.</param>
        public void AddPayloadSignature(byte[] signature)
        {
            Signature = signature;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Missing payload signature for signed transaction.</exception>
        public byte[] Encode()
        {
            if (Signed && Signature == null)
            {
                throw new Exception("Missing payload signature for signed transaction.");
            }

            var list = new List<byte>();

            // 4 is the TRANSACTION_VERSION constant and it is 7 bits long, the highest bit 1 for signed transaction, 0 for unsigned.
            list.Add((byte)(Constants.ExtrinsicVersion | (Signed ? 0x80 : 0)));

            // 32 bytes + prefix depending on address encoding in chain, see Constants.Address_version
            list.AddRange(Account.Encode());

            // key type ed = 00 and sr = FF
            list.Add(Account.KeyTypeByte);

            // add signature if exists
            if (Signature != null)
            {
                list.AddRange(Signature);
            }
            else
            {

            }

            list.AddRange(Era.Encode());

            list.AddRange(Nonce.Encode());

            list.AddRange(Tip.Encode());

            list.AddRange(Method.Encode());

            return Utils.SizePrefixedByteArray(list);
        }
    }
}
