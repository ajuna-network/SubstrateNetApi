using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SubstrateNetApi
{

    //public partial class RequestGenerator
    //{
    //    public class UncheckedExtrinsic
    //    {
    //        //4 is the TRANSACTION_VERSION constant and it is 7 bits long, the highest bit 1 for signed transaction, 0 for unsigned. 
    //        public byte _signatureVersion;

    //        /// <summary>
    //        ///  ADDRESS  
    //        ///  ----------------------------------------------------------
    //        /// </summary>

    //        //[FixedSizeArrayConverter(32)]
    //        public byte[] _pubKey;

    //        /// <summary>
    //        ///  SIGNATURE
    //        ///  ----------------------------------------------------------
    //        /// </summary>

    //        // Ed25519 = 0, Sr25519 = 1, Ecdsa = 2
    //        public byte SignatureType { get; set; }

    //        // [FixedSizeArrayConverter(64)]
    //        public byte[] Signature { get; set; }

    //        /// <summary>
    //        ///  EXTRA
    //        ///  ----------------------------------------------------------
    //        /// </summary>

    //        private Era _era { get; set; }

    //        //  [CompactBigIntegerConverter]
    //        private CompactInteger _nonce { get; set; }

    //        // [CompactBigIntegerConverter]
    //        private CompactInteger _chargeTransactionPayment { get; set; }

    //        /// <summary>
    //        ///  CALL
    //        ///  ----------------------------------------------------------
    //        /// </summary>
    //        private byte[] _moduleIndex { get; set; }
    //        private byte[] _callIndex { get; set; }
    //        private byte[] _parameters { get; set; }

    //        public UncheckedExtrinsic(bool IsSigned, byte[] pubKey, uint nonce, CompactInteger moduleIndex, CompactInteger callIndex, byte[] parameters)
    //        {
    //            _signatureVersion = (byte)(4 | (IsSigned ? 0x80 : 0)); ;
    //            _pubKey = pubKey;
    //            _era = null;
    //            _nonce = new BigInteger(nonce);
    //            _chargeTransactionPayment = BigInteger.Zero;
    //            _moduleIndex = moduleIndex.Encode();
    //            _callIndex = callIndex.Encode();
    //            _parameters = parameters;
    //        }

    //        public byte[] GetPayload()
    //        {
    //            var byteList = new List<byte>();

    //            // --- Call
    //            byteList.AddRange(_moduleIndex);
    //            byteList.AddRange(_callIndex);
    //            if (_parameters != null)
    //            {
    //                byteList.AddRange(_parameters);
    //            }

    //            // --- Extra
    //            // byteList.Add(_era);
    //            byteList.AddRange(_nonce.Encode());
    //            byteList.AddRange(_chargeTransactionPayment.Encode());

    //            // --- ExtraSigned
    //            foreach (var b in ExtrinsicExtension())
    //            {
    //                byteList.AddRange(b);
    //            }

    //            return byteList.ToArray();
    //        }

    //        public byte[] GetExtrinsic()
    //        {

    //            // Calculate total extrinsic length
    //            //Length = 134 + Utils.EncodeCompactInteger(_nonce).Length + compactNonce.Length;
    //            //var compactLength = Scale.EncodeCompactInteger(Length);

    //            var byteList = new List<byte>();

    //            // 4 is the TRANSACTION_VERSION constant and it is 7 bits long,
    //            // the highest bit 1 for signed transaction, 0 for unsigned. 
    //            byteList.Add(_signatureVersion); 

    //            // --- Address
    //            byteList.AddRange(_pubKey); // public key

    //            // --- Signature
    //            byteList.Add(SignatureType);  // 0x00=Ed25519, 0x01=Sr25519, 0x02=Ecdsa

    //            // Signed Blake2b_256 [Call, Extra, ExtrinsicExtensions]
    //            // ExtrinsicExtensions => ... No clue about how to get them ...  
    //            //     ("CheckSpecVersion", "CheckTxVersion", "CheckGenesis", 
    //            //      "CheckMortality", "CheckNonce", "CheckWeight",
    //            //      "ChargeTransactionPayment") 
    //            byteList.AddRange(Signature); // ... TODO: FixedSizeArrayConverter(64)

    //            // --- Extra
    //            // byteList.Add(_era); // I have no clue here ....
    //            byteList.AddRange(_nonce.Encode());
    //            byteList.AddRange(_chargeTransactionPayment.Encode());

    //            // --- Call
    //            byteList.AddRange(_moduleIndex);
    //            byteList.AddRange(_callIndex);
    //            if (_parameters != null)
    //            {
    //                byteList.AddRange(_parameters);
    //            }

    //            var bytes = byteList.ToArray();
                

    //            return bytes;
    //        }

    //        private IEnumerable<byte[]> ExtrinsicExtension()
    //        {
    //            string[] extrinsicExtensions = new string[] {
    //            "CheckSpecVersion",
    //            "CheckTxVersion",
    //            "CheckGenesis",
    //            "CheckMortality",
    //            "CheckNonce",
    //            "CheckWeight",
    //            "ChargeTransactionPayment"
    //            };

    //            IEnumerable<byte[]> signedExtra = extrinsicExtensions.Select(x =>
    //            {
    //                switch (x)
    //                {
    //                    case "CheckSpecVersion":
    //                        return ((CompactInteger)1).Encode();
    //                    case "CheckTxVersion":
    //                        return ((CompactInteger)1).Encode();
    //                    case "CheckGenesis":
    //                        return Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9");
    //                    case "CheckMortality":
    //                        return new byte[1];
    //                    case "CheckNonce":
    //                        return ((CompactInteger)3).Encode();
    //                    case "CheckWeight":
    //                        return ((CompactInteger)0).Encode();
    //                    case "ChargeTransactionPayment":
    //                        return new byte[0];
    //                    default:
    //                        throw new Exception($"Unknown signed extension '{x}'!");
    //                };
    //            });

    //            return signedExtra;
    //        }
    //    }
    //}
}
