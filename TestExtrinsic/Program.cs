using System;
using System.Collections.Generic;
using System.Numerics;
using Schnorrkel;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel;
using SubstrateNetApi.MetaDataModel.Extrinsic;
using SubstrateNetApi.MetaDataModel.Values;

namespace TestExtrinsic
{
    class Program
    {
        static void Main(string[] args)
        {
            DecodeExtrinsicString();
            //EraTesting();
            //CompactIntegerDecodingTest()

            //Console.WriteLine(CompactInteger.Decode(Utils.HexToByteArray("0x76d")));
        }

        private static void EraTesting()
        {
            var t1 = Era.Decode(Utils.HexToByteArray("0x1503"));
            Console.WriteLine($"NODE: {t1}");

            var t2 = Era.Decode(Utils.HexToByteArray("0xD503"));
            Console.WriteLine($" API: {t2}");

            var t3 = Era.Create(64, 15793);
            Console.WriteLine($" API: {t3}");

            ulong currentBlockNumber = (ulong)15689;
            var lastBit = currentBlockNumber & (ulong)-(long)currentBlockNumber;
            var nextPowerOf2 = Math.Pow(2, Math.Round(Math.Log(15689, 2)));
            Console.WriteLine($"currentBlockNumber[{currentBlockNumber}]: {Math.Round(Math.Log(15689, 2))} {nextPowerOf2}");
        }

        private static void CompactIntegerDecodingTest()
        {

            var bytes = new byte[2];

            Console.WriteLine($"0xFC0 - {ulong.Parse(Convert.ToString(0xFFC0, 2)):0000 0000 0000 0000}");
            for (int n = 4000; n < 4124; n++)
            {
                Console.WriteLine($"{n:0000}: {ulong.Parse(Convert.ToString(n, 2)):0000 0000 0000 0000} " +
                                  $"..[{uint.Parse(Convert.ToString(((n & 0x3F) << 2) | 0x01, 2)):0000 0000} " +
                                  $"{uint.Parse(Convert.ToString((n & 0xFFC0) >> 6, 2)):0000 0000}]..");


            }

            //Console.WriteLine($"      0x3F               => {uint.Parse(Convert.ToString(0x3F, 2)).ToString("0000 0000")}");
            //Console.WriteLine($"  n & 0x3F               => {uint.Parse(Convert.ToString((n & 0x3F), 2)).ToString("0000 0000")}");
            //Console.WriteLine($" (n & 0x3F) << 2         => {uint.Parse(Convert.ToString((n & 0x3F) << 2, 2)).ToString("0000 0000")}");

            //Console.WriteLine($"((n & 0x3F) << 2) | 0x01 => {uint.Parse(Convert.ToString(((n & 0x3F) << 2) | 0x01, 2)).ToString("0000 0000")}");
            //Console.WriteLine($" (n & 0xFC0) >> 6         => {uint.Parse(Convert.ToString((n & 0xFC0) >> 6, 2)).ToString("0000 0000")}");


            //bytes[0] = (byte)(((n & 0x3F) << 2) | 0x01);
            //bytes[1] = (byte)((n & 0xFC0) >> 6);

            //Console.WriteLine($"{Utils.Bytes2HexString(bytes)} --> {bytes[0]} {bytes[1]}");
            //Console.WriteLine(Utils.Bytes2HexString(bytes));
            //Console.WriteLine(CompactInteger.Decode(bytes));

        }

        private static void DecodeExtrinsicString()
        {
            const int PUBIC_KEY_SIZE = 32;
            const int SIGNATURE_SIZE = 64;

            // public key                     0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e
            // dest public key                                                                                                                                                                                                                              0x9effc1668ca381c242885516ec9fa2b19c67b6684c02a8a3237b6862e5c8cd7e
            //string balanceTransfer = "0x450284278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e004313aef86edd83200a3650bac543a45ce1c3013c29df7ca08a0c6f0e9822057b259b9fa3ef10f950da6b07ddf0b21179a834d92921e1130f9c95018ae3df6c01c502000004009effc1668ca381c242885516ec9fa2b19c67b6684c02a8a3237b6862e5c8cd7e1300008a5d78456301";

            // public key                     0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e
            // dest public key                                                                                                                                                                                                                                    0x9effc1668ca381c242885516ec9fa2b19c67b6684c02a8a3237b6862e5c8cd7e
            //string balanceTransfer = "0x3d0284278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00d517a97eebab11a9d9fa4b8c3961428cd2511e2007a7778da8127bc0f826ca513cc6702675d4d672f823de338c91aea48a573d7f5155d32c583b96fb0b110505c50200c6c96fb904009effc1668ca381c242885516ec9fa2b19c67b6684c02a8a3237b6862e5c8cd7e56346f1d";

            // public key                     0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e
            // signature                                                                                        0x14ae74dd7964365038eba44f51c347b9c7070231d56e38ef1024457ebdc6dc03d20226243b1b2731df6fd80f7170643221bd8bf8d06215d4bfeac68a2c9d2305
            // dest public key                                                                                                                                                                                                                                0x9effc1668ca381c242885516ec9fa2b19c67b6684c02a8a3237b6862e5c8cd7e
            //string balanceTransfer = "0x350284278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e0014ae74dd7964365038eba44f51c347b9c7070231d56e38ef1024457ebdc6dc03d20226243b1b2731df6fd80f7170643221bd8bf8d06215d4bfeac68a2c9d2305f50204491304009effc1668ca381c242885516ec9fa2b19c67b6684c02a8a3237b6862e5c8cd7e068d6deb";
            //string balanceTransfer = "0x2d0284d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01726ba1fab06d3e1bf6abfa0d5af85e25f2a970e11384162b7caf83935c58f769b6fef3b83a29ffd8d813a037d01cd6bcb21beaa88e9a18b3abe366b0458a8a82a5001049130400278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e8543";
            string balanceTransfer = "0x3102844b94e38b0c2ee21c367d4c9584204ce62edf5b4a6f675f10678cc56b6ea86e71000b893ef2bbed9be566d61d14eba57b454118328929944d71db86b4c7989570be959597948561639da02eadd516f830ff5ba7aab938b87e63bc9d61d1c178e80cb50020000500d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d0284d717";
            string pendingExtrinsic = balanceTransfer;


            //string dmogCreate = "0xa50184278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00cbbf8076d31e163051556563a9de71816ba05fac08b905b14c2e6d266b7c621f8abadb2776c6d35f1990ed0a3fd768493ce85ac78ef654d69760e7d80273af01f5020849130602";
            //string dmogCreate = "0xa10184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01bc9103c06e696c1d110380ddbf8b5b3dc990f1432ea44231e14d0f9f3824f700a067d3695f3050a8eff3d1053c56b1b36550ff93ee79c888a376b9bfa42ebc8f250308000602";
            //string dmogCreate = "0xa10184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d015aeaba077ee63272f4c32d563ab72301a64e2a4d0bd02445b25cc16e6827e4317ef9304a5af9d5061581b0bf17e8a6a880465ed278251f301bbb3cd719fbf28105030c000602";
            //string dmogCreate = "0xa10184278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00516811e8fc5c2fe66e86b251fe96a5d3e1bef77a56da8dedac31973018ee27d4a4bd72e34b73f637213e991b35605f236f0136c53627dd620478a4091ae4bd0d001049130602";

            //string dmogCreate   = "0xa10184278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00743f9c4d923490da02db6567e3128b7af336e3c3ff586dc7c262a787912b251eadd87001192db949215a5a9fb76b7ab2dc50fc70aea3b64a99cd4bc5a423c60a250310000602";
            // public key              0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e  824108F78CE80A3772BA19D0EA661D726C32974633058691E73ECCFA6F5E34C89278A756357063FD14942C10E79DB49F712AC5F0D160982C61023D5C19F56E01      0602
            //string dmogCreate = "0xa10184278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00de0b65d4479f7f041b0af2a519893d9c4dd637ab3baa9e51da894663869304dd5dba12a0e1aa140cf40a07f17f690c0aede100fa083cbdd15c637bc2d044ec04450314000602";


            //string dmogCreate = "0xa10184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01448082984004e4dc7cb964eba2eb7201c5686d80e666944e2aa01c2be95eaa5be9d547da63616a82631e87e4078a647fbd07920f97c8ea0993207c0fbdd2a98e150314000602";
            //string dmogCreate = "0xA10184278117FC144C72340F67D0F2316E8386CEFFBF2B2428C9C51FEF7C597F1D426E008C8C7DC53DE26E417D2D1BC0295399D2670361A84D267BD60DE92C2FD7C4C9E515833EEF4107ED71EAC395D4BDAC79C81B144538DFC55090506DC4758D5A3109D50320000602";
            //string pendingExtrinsic = dmogCreate;

            byte[] bytes; // = Utils.HexToByteArray(pendingExtrinsic);

            Console.WriteLine($"author_pendingExtrinsics: {pendingExtrinsic}");
            Console.WriteLine($"********* DECODING *********");
            int p = 0;

            string byteString = pendingExtrinsic.Substring(2);

            // length
            bytes = Utils.HexToByteArray(byteString);
            var length = CompactInteger.Decode(bytes, ref p);
            Console.WriteLine($"length: {length} [{p}]");
            byteString = byteString.Substring(p * 2);

            // signature version [byte]
            byte[] signatureVersion = Utils.HexToByteArray(byteString.Substring(0, 2));
            Console.WriteLine($"signatureVersion: {Utils.Bytes2HexString(signatureVersion)}");
            byteString = byteString.Substring(signatureVersion.Length * 2);
            bytes = Utils.HexToByteArray(byteString);

            // send public key
            byte[] sendPublicKey = Utils.HexToByteArray(byteString.Substring(0, PUBIC_KEY_SIZE * 2));
            Console.WriteLine($"sendPublicKey: {Utils.GetAddressFrom(sendPublicKey)} [{Utils.Bytes2HexString(sendPublicKey)}]");
            byteString = byteString.Substring(sendPublicKey.Length * 2);
            bytes = Utils.HexToByteArray(byteString);

            // seperator1
            byte[] sendPublicKeyType = Utils.HexToByteArray(byteString.Substring(0, 2));
            Console.WriteLine($"sendPublicKeyType: {Utils.Bytes2HexString(sendPublicKeyType)}");
            byteString = byteString.Substring(sendPublicKeyType.Length * 2);
            bytes = Utils.HexToByteArray(byteString);

            // signature
            byte[] signature = Utils.HexToByteArray(byteString.Substring(0, SIGNATURE_SIZE * 2));
            Console.WriteLine($"signature: {Utils.Bytes2HexString(signature)}");
            byteString = byteString.Substring(signature.Length * 2);
            bytes = Utils.HexToByteArray(byteString);

            // era
            byte[] era = Utils.HexToByteArray(byteString.Substring(0, 4));
            //byte[] era = Utils.HexToByteArray(byteString.Substring(0, 2));
            Console.WriteLine($"era: {Utils.Bytes2HexString(era)}");
            byteString = byteString.Substring(era.Length * 2);
            bytes = Utils.HexToByteArray(byteString);

            // nonce
            p = 0;
            var nonce = CompactInteger.Decode(bytes, ref p);
            Console.WriteLine($"nonce: {nonce} [{p}]");
            byteString = byteString.Substring(p * 2);
            bytes = Utils.HexToByteArray(byteString);

            // tip
            p = 0;
            var tip = CompactInteger.Decode(bytes, ref p);
            Console.WriteLine($"tip: {tip} [{p}]");
            byteString = byteString.Substring(p * 2);
            bytes = Utils.HexToByteArray(byteString);

            // module index
            byte[] moduleIndex = Utils.HexToByteArray(byteString.Substring(0, 4));
            Console.WriteLine($"moduleIndex: {Utils.Bytes2HexString(moduleIndex)}");
            byteString = byteString.Substring(moduleIndex.Length * 2);
            bytes = Utils.HexToByteArray(byteString);

            byte[] parameters = bytes;

            if (byteString.Length > 0)
            {
                // dest public key
                byte[] destPublicKey = Utils.HexToByteArray(byteString.Substring(0, PUBIC_KEY_SIZE * 2));
                Console.WriteLine($"destPublicKey: {Utils.GetAddressFrom(destPublicKey)} [{Utils.Bytes2HexString(destPublicKey)}]");
                byteString = byteString.Substring(destPublicKey.Length * 2);
                bytes = Utils.HexToByteArray(byteString);

                // parameters
                p = 0;
                var amount = CompactInteger.Decode(bytes, ref p);
                Console.WriteLine($"amount: {amount} [{p}]");
                byteString = byteString.Substring(p * 2);
                bytes = Utils.HexToByteArray(byteString);
            }

            Method method = new Method(moduleIndex[0], moduleIndex[1], parameters);

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, new Account(sendPublicKeyType[0] == 0 ? KeyType.ED25519 : KeyType.SR25519, new byte[0], sendPublicKey), nonce, method, new byte[0], new byte[0], 47, 1234);

            uncheckedExtrinsic.AddPayloadSignature(signature);

            //Console.WriteLine(Utils.Bytes2HexString(uncheckedExtrinsic.Encode()));
        }
    }
}
