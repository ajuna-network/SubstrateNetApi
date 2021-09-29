using Newtonsoft.Json;
using NLog;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Linq;

namespace SubstrateNetApi.Model.Extrinsics
{
    public class Extrinsic
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public bool Signed;

        public byte TransactionVersion;

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Account Account;

        public Era Era;

        public CompactInteger Nonce;

        public CompactInteger Tip;

        public Method Method;

        public byte[] Signature;

        /// <summary>
        /// Initializes a new instance of the <see cref="Extrinsic"/> class.
        /// </summary>
        /// <param name="str">The string.</param>
        public Extrinsic(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Extrinsic"/> class.
        /// </summary>
        /// <param name="memory">The memory.</param>
        internal Extrinsic(Memory<byte> memory)
        {
            int p = 0;
            int m;

            // length
            var length = CompactInteger.Decode(memory.ToArray(), ref p);

            // signature version
            m = 1;
            var _signatureVersion = memory.Slice(p, m).ToArray()[0];
            Signed = _signatureVersion >= 0x80;
            TransactionVersion = (byte)(_signatureVersion - (Signed ? 0x80 : 0x00));
            p += m;

            // this part is for signed extrinsics
            if (Signed)
            {

                // start bytes
                m = 1;
                var _startBytes = memory.Slice(p, m).ToArray()[0];
                p += m;

                // sender public key
                m = 32;
                var _senderPublicKey = memory.Slice(p, m).ToArray();
                p += m;

                // sender public key type
                m = 1;
                var _senderPublicKeyType = memory.Slice(p, m).ToArray()[0];
                p += m;

                var account = new Account();
                account.Create((KeyType)_senderPublicKeyType, _senderPublicKey);
                Account = account;

                // signature
                m = 64;
                Signature = memory.Slice(p, m).ToArray();
                p += m;

                // era
                m = 1;
                var era = memory.Slice(p, m).ToArray();
                if (era[0] != 0)
                {
                    m = 2;
                    era = memory.Slice(p, m).ToArray();
                }
                Era = Era.Decode(era);
                p += m;

                // nonce
                Nonce = CompactInteger.Decode(memory.ToArray(), ref p);

                // tip
                Tip = CompactInteger.Decode(memory.ToArray(), ref p);
            }

            // method
            m = 2;
            var method = memory.Slice(p, m).ToArray();
            p += m;

            // parameters
            var parameter = memory.Slice(p).ToArray();

            Method = new Method(method[0], method[1], parameter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Extrinsic"/> class.
        /// </summary>
        /// <param name="signed">if set to <c>true</c> [signed].</param>
        /// <param name="account">The account.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="method">The method.</param>
        /// <param name="era">The era.</param>
        /// <param name="tip">The tip.</param>
        public Extrinsic(bool signed, Account account, CompactInteger nonce, Method method, Era era, CompactInteger tip)
        {
            Signed = signed;
            TransactionVersion = Constants.ExtrinsicVersion;
            Account = account;
            Era = era;
            Nonce = nonce;
            Tip = tip;
            Method = method;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
