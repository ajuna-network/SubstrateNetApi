using Schnorrkel;
using SubstrateNetApi.MetaDataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SubstrateNetApi
{

    public class RequestGenerator
    {
        public static string GetStorage(Module module, Item item, byte[] parameter = null)
        {
            var mBytes = Encoding.ASCII.GetBytes(module.Name);
            var iBytes = Encoding.ASCII.GetBytes(item.Name);

            var keybytes = HashExtension.XXHash128(mBytes).Concat(HashExtension.XXHash128(iBytes)).ToArray();

            switch (item.Type)
            {
                case Storage.Type.Plain:
                    return BitConverter.ToString(keybytes).Replace("-", "");
                case Storage.Type.Map:

                    switch (item.Function.Hasher)
                    {
                        case Storage.Hasher.Identity:
                            return BitConverter.ToString(keybytes.Concat(parameter).ToArray()).Replace("-", "");

                        case Storage.Hasher.Blake2_128:
                        case Storage.Hasher.Blake2_256:
                        case Storage.Hasher.Blake2_128Concat:
                            return BitConverter.ToString(keybytes.Concat(HashExtension.Blake2Concat(parameter)).ToArray()).Replace("-", "");

                        case Storage.Hasher.Twox128:
                        case Storage.Hasher.Twox256:
                        case Storage.Hasher.Twox64Concat:
                        case Storage.Hasher.None:
                        default:
                            break;
                    }
                    return "";
                case Storage.Type.DoubleMap:
                    return "";
                default:
                    return "";
            }
        }

        internal static string SubmitExtrinsic(Module module, Call call, string parameter, byte[] pubKey, byte[] priKey)
        {
            var mBytes = Encoding.ASCII.GetBytes(module.Name);
            var iBytes = Encoding.ASCII.GetBytes(call.Name);

            var keybytes = HashExtension.XXHash128(mBytes).Concat(HashExtension.XXHash128(iBytes)).ToArray();

            if (call.Arguments?.Length == 0)
            {
                return BitConverter.ToString(keybytes).Replace("-", "");
            }

            var extra = new byte[0];
            var extraSigned = new byte[0];

            var payload = SignaturePayLoad(keybytes, extra, extraSigned);

            Sr25519v091.SignSimple(pubKey, priKey, payload);

            throw new NotImplementedException("Arguments in call aren't implemented.");
        }

        private static byte[] SignaturePayLoad(byte[] a, byte[] b, byte[] c)
        {
            string[] extrinsicExtensions = new string[] {
                "CheckSpecVersion",
                "CheckTxVersion",
                "CheckGenesis",
                "CheckMortality",
                "CheckNonce",
                "CheckWeight",
                "ChargeTransactionPayment"
            };

            IEnumerable<object> signedExtra = extrinsicExtensions.Select(x => {
                switch (x)
                {
                    case "CheckSpecVersion":
                        return (object) 1;
                    case "CheckTxVersion":
                        return (object)1;
                    case "CheckGenesis":
                        return (object) Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9");
                    case "CheckMortality":
                        return (object)null;
                    case "CheckNonce":
                        return (object)null;
                    case "CheckWeight":
                        return (object)null;
                    case "ChargeTransactionPayment":
                        return (object)null;
                    default:
                        throw new Exception("");
                };
            });

            throw new NotImplementedException();
        }

        public class UncheckedExtrinsic
        {
            //4 is the TRANSACTION_VERSION constant and it is 7 bits long, the highest bit 1 for signed transaction, 0 for unsigned. 
            public byte SignatureVersion { get; set; } = (byte)(4 | (true ? 0x80 : 0));

            /// <summary>
            ///  ADDRESS  
            ///  ----------------------------------------------------------
            /// </summary>

            //[FixedSizeArrayConverter(32)]
            public byte[] _pubKey;

            /// <summary>
            ///  SIGNATURE
            ///  ----------------------------------------------------------
            /// </summary>

            // Ed25519 = 0, Sr25519 = 1, Ecdsa = 2
            public byte SignatureType { get; set; }

            // [FixedSizeArrayConverter(64)]
            public byte[] Signature { get; set; }

            /// <summary>
            ///  EXTRA
            ///  ----------------------------------------------------------
            /// </summary>

            private Era _era { get; set; }

            //  [CompactBigIntegerConverter]
            private BigInteger _nonce { get; set; }

            // [CompactBigIntegerConverter]
            private BigInteger _chargeTransactionPayment { get; set; }

            /// <summary>
            ///  CALL
            ///  ----------------------------------------------------------
            /// </summary>
            private byte _moduleIndex { get; set; }
            private byte _callIndex { get; set; }
            private byte[] _parameters { get; set; }

            public UncheckedExtrinsic(byte[] pubKey, BigInteger nonce, byte moduleIndex, byte callIndex, byte[] parameters)
            {
                _pubKey = pubKey;
                _era = null;
                _nonce = nonce;
                _chargeTransactionPayment = BigInteger.Zero;
                _moduleIndex = moduleIndex;
                _callIndex = callIndex;
                _parameters = parameters;
            }

            public byte[] GetPayload()
            {
                // --- Call
                var byteList = new List<byte>();
                byteList.Add(_moduleIndex);
                byteList.Add(_callIndex);
                byteList.AddRange(_parameters);

                // --- Extra
                // byteList.Add(_era);
                byteList.AddRange(Utils.EncodeCompactInteger(_nonce));
                byteList.AddRange(Utils.EncodeCompactInteger(_chargeTransactionPayment));

                return byteList.ToArray();
            }
        }
    }

    public class Era
    {
    }
}
