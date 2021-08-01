using Newtonsoft.Json;
using NLog;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Struct;
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

        /// <summary>
        /// Gets the typed extrinsic.
        /// </summary>
        /// <param name="extrinsic">The extrinsic.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns></returns>
        internal static Extrinsic GetTypedExtrinsic(Extrinsic extrinsic, MetaData metaData)
        {
            var modules = metaData.Modules.ToList();
            var module = modules.FirstOrDefault(t => t.Index == extrinsic.Method.ModuleIndex);
            extrinsic.Method.ModuleName = module?.Name;

            var call = module?.Calls[extrinsic.Method.CallIndex];
            extrinsic.Method.CallName = call?.Name;
            extrinsic.Method.Arguments = call?.Arguments;

            extrinsic.EvaluateTypedArguments();

            // need to change to the typed extrinsic
            return extrinsic;
        }

        private void EvaluateTypedArguments()
        {
            if (Method.Arguments == null || Method.Arguments.Length == 0 || Method.Parameters == null || Method.Parameters.Length == 0)
            {
                Logger.Warn("Can't evaluate typed arguments extrinsic isn't properly enriched.");
            }

            var arguments = Method.Arguments;
            var memory = Method.Parameters.AsMemory();

            int m;

            for (var i = 0; i < arguments.Length; i++)
            {
                var p = 0;
                var argument = arguments[i];
                switch (argument.Type)
                {
                    case "Compact<T::BlockNumber>":
                        argument.Value = CompactInteger.Decode(memory.ToArray(), ref p);
                        break;

                    case "Compact<T::Balance>":
                        argument.Value = CompactInteger.Decode(memory.ToArray(), ref p);
                        break;

                    case "Compact<T::Moment>":
                        argument.Value = CompactInteger.Decode(memory.ToArray(), ref p);
                        break;

                    case "<T::Lookup as StaticLookup>::Source":
                        m = 1;
                        var _ = memory.Slice(p, m).ToArray()[0]; // public key type
                        p += m;

                        m = 32;
                        argument.Value = Utils.GetAddressFrom(memory.Slice(p, m).ToArray()); // public key
                        p += m;
                        break;

                    case "Vec<T::Header>":
                        argument.Value = "Unhandled 'argument.Type'";
                        break;

                    case "u8":
                        var u8 = new U8();
                        u8.Decode(memory.Slice(p).ToArray(), ref p);
                        argument.Value = u8.ToString();
                        break;

                    case "Option<u8>":
                        var optionU8 = new Option<U8>();
                        optionU8.Decode(memory.Slice(p).ToArray(), ref p);
                        argument.Value = optionU8.OptionFlag ? optionU8.ToString() : "null";
                        break;

                    default:
                        Logger.Warn($"Argument is currently unhandled in GetTypedArguments, '{argument.Type}', please add!");
                        argument.Value = $"Unhandled '{argument.Type}'";
                        break;
                }
            }
        }
    }
}
