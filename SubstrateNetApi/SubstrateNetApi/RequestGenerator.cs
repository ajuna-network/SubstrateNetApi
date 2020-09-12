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

        internal static string SubmitExtrinsic(int module, int call, string parameter, uint nonce, byte[] pubKey, byte[] priKey)
        {
            UncheckedExtrinsic uncheckedExtrinsic = new UncheckedExtrinsic(true, pubKey, nonce, module, call, null);

            var hashedPayload = HashExtension.Blake2(uncheckedExtrinsic.GetPayload(), 256);
            var signedPayload = Sr25519v091.SignSimple(pubKey, priKey, hashedPayload);

            uncheckedExtrinsic.SignatureType = 0x01;
            uncheckedExtrinsic.Signature = signedPayload;

            return Utils.Bytes2HexString(uncheckedExtrinsic.GetExtrinsic());
        }

        public class UncheckedExtrinsic
        {
            //4 is the TRANSACTION_VERSION constant and it is 7 bits long, the highest bit 1 for signed transaction, 0 for unsigned. 
            public byte _signatureVersion;

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
            private byte[] _moduleIndex { get; set; }
            private byte[] _callIndex { get; set; }
            private byte[] _parameters { get; set; }

            public UncheckedExtrinsic(bool IsSigned, byte[] pubKey, uint nonce, int moduleIndex, int callIndex, byte[] parameters)
            {
                _signatureVersion = (byte)(4 | (IsSigned ? 0x80 : 0)); ;
                _pubKey = pubKey;
                _era = null;
                _nonce = new BigInteger(nonce);
                _chargeTransactionPayment = BigInteger.Zero;
                _moduleIndex = Utils.EncodeCompactInteger(moduleIndex);
                _callIndex = Utils.EncodeCompactInteger(callIndex);
                _parameters = parameters;
            }

            public byte[] GetPayload()
            {
                var byteList = new List<byte>();

                // --- Call
                byteList.AddRange(_moduleIndex);
                byteList.AddRange(_callIndex);
                if (_parameters != null)
                {
                    byteList.AddRange(_parameters);
                }

                // --- Extra
                // byteList.Add(_era);
                byteList.AddRange(Utils.EncodeCompactInteger(_nonce));
                byteList.AddRange(Utils.EncodeCompactInteger(_chargeTransactionPayment));

                // --- ExtraSigned
                foreach (var b in ExtrinsicExtension())
                {
                    byteList.AddRange(b);
                }

                return byteList.ToArray();
            }

            public byte[] GetExtrinsic()
            {
                var byteList = new List<byte>();

                // 4 is the TRANSACTION_VERSION constant and it is 7 bits long,
                // the highest bit 1 for signed transaction, 0 for unsigned. 
                byteList.Add(_signatureVersion); 

                // --- Address
                byteList.AddRange(_pubKey); // public key

                // --- Signature
                byteList.Add(SignatureType);  // 0x00=Ed25519, 0x01=Sr25519, 0x02=Ecdsa

                // Signed Blake2b_256 [Call, Extra, ExtrinsicExtensions]
                // ExtrinsicExtensions => ... No clue about how to get them ...  
                //     ("CheckSpecVersion", "CheckTxVersion", "CheckGenesis", 
                //      "CheckMortality", "CheckNonce", "CheckWeight",
                //      "ChargeTransactionPayment") 
                byteList.AddRange(Signature); // ... TODO: FixedSizeArrayConverter(64)

                // --- Extra
                // byteList.Add(_era); // I have no clue here ....
                byteList.AddRange(Utils.EncodeCompactInteger(_nonce));
                byteList.AddRange(Utils.EncodeCompactInteger(_chargeTransactionPayment));

                // --- Call
                byteList.AddRange(_moduleIndex);
                byteList.AddRange(_callIndex);
                if (_parameters != null)
                {
                    byteList.AddRange(_parameters);
                }

                return byteList.ToArray();
            }

            private IEnumerable<byte[]> ExtrinsicExtension()
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

                IEnumerable<byte[]> signedExtra = extrinsicExtensions.Select(x =>
                {
                    switch (x)
                    {
                        case "CheckSpecVersion":
                            return Utils.EncodeCompactInteger(1);
                        case "CheckTxVersion":
                            return Utils.EncodeCompactInteger(1);
                        case "CheckGenesis":
                            return Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9");
                        case "CheckMortality":
                            return new byte[0];
                        case "CheckNonce":
                            return new byte[0];
                        case "CheckWeight":
                            return new byte[0];
                        case "ChargeTransactionPayment":
                            return new byte[0];
                        default:
                            throw new Exception("");
                    };
                });

                return signedExtra;
            }
        }
    }

    public class Era
    {
    }
}
