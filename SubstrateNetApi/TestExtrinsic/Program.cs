using System;
using System.Collections.Generic;
using System.Numerics;
using Schnorrkel;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel;

namespace TestExtrinsic
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetTesting();


            for (ulong x = 0; x < 1000; x++)
            {

                //Console.WriteLine($"{x} => {BitConverter.ToString(BitConverter.GetBytes(x))}");
                //Console.WriteLine($"{-(long)x} => {BitConverter.ToString(BitConverter.GetBytes(-(long)x))}");
                Console.WriteLine($" {x.ToString("0000")}: {(x & (ulong) - (long) x).ToString("0000")} [test & (ulong)-(long)test]");
            }
            //Console.WriteLine($"{test}[test] => ");
            //Console.WriteLine($"{test}[test] => ");
            //Console.WriteLine($"{(ulong)-(long)test} [(ulong)-(long)test] => ");
            //Console.WriteLine($"{test & (ulong)-(long)test} [test & (ulong)-(long)test]");

        }

        private static void GetTesting()
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
            //string pendingExtrinsic = balanceTransfer;


            //string dmogCreate = "0xa50184278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00cbbf8076d31e163051556563a9de71816ba05fac08b905b14c2e6d266b7c621f8abadb2776c6d35f1990ed0a3fd768493ce85ac78ef654d69760e7d80273af01f5020849130602";
            //string dmogCreate = "0xa10184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01bc9103c06e696c1d110380ddbf8b5b3dc990f1432ea44231e14d0f9f3824f700a067d3695f3050a8eff3d1053c56b1b36550ff93ee79c888a376b9bfa42ebc8f250308000602";
            //string dmogCreate = "0xa10184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d015aeaba077ee63272f4c32d563ab72301a64e2a4d0bd02445b25cc16e6827e4317ef9304a5af9d5061581b0bf17e8a6a880465ed278251f301bbb3cd719fbf28105030c000602";
            //string dmogCreate = "0xa10184278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00516811e8fc5c2fe66e86b251fe96a5d3e1bef77a56da8dedac31973018ee27d4a4bd72e34b73f637213e991b35605f236f0136c53627dd620478a4091ae4bd0d001049130602";

            //string dmogCreate   = "0xa10184278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00743f9c4d923490da02db6567e3128b7af336e3c3ff586dc7c262a787912b251eadd87001192db949215a5a9fb76b7ab2dc50fc70aea3b64a99cd4bc5a423c60a250310000602";
            // public key              0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e  824108F78CE80A3772BA19D0EA661D726C32974633058691E73ECCFA6F5E34C89278A756357063FD14942C10E79DB49F712AC5F0D160982C61023D5C19F56E01      0602
            string dmogCreate = "0xa10184278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00de0b65d4479f7f041b0af2a519893d9c4dd637ab3baa9e51da894663869304dd5dba12a0e1aa140cf40a07f17f690c0aede100fa083cbdd15c637bc2d044ec04450314000602";
            string pendingExtrinsic = dmogCreate;

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

            if (byteString.Length > 0)
            {
                // dest public key
                byte[] destPublicKey = Utils.HexToByteArray(byteString.Substring(0, PUBIC_KEY_SIZE * 2));
                Console.WriteLine($"destPublicKey: {Utils.GetAddressFrom(destPublicKey)}");
                byteString = byteString.Substring(destPublicKey.Length * 2);
                bytes = Utils.HexToByteArray(byteString);

                // parameters
                p = 0;
                var amount = CompactInteger.Decode(bytes, ref p);
                Console.WriteLine($"amount: {amount} [{p}]");
                byteString = byteString.Substring(p * 2);
                bytes = Utils.HexToByteArray(byteString);
            }

            var uncheckedExtrinsic = new UnCheckedExtrinsic();
            uncheckedExtrinsic.IsSigned(true);
            uncheckedExtrinsic.SetTransactionVersion(4);
            uncheckedExtrinsic.SetSenderPublicKey(sendPublicKey);
            uncheckedExtrinsic.SetSenderPublicKeyType(sendPublicKeyType);
            uncheckedExtrinsic.SetEra(era);
            uncheckedExtrinsic.SetNonce(nonce);
            uncheckedExtrinsic.SetTip(tip);
            uncheckedExtrinsic.SetCall(moduleIndex);
            Console.WriteLine($"UncheckedExtrinsic: {dmogCreate.ToUpper().Equals(Utils.Bytes2HexString(uncheckedExtrinsic.Serialize(signature)).ToUpper())}");

            //var signedExtensionsBytes = new SignedExtensions().Serialize();
            //var methodBytes = new Method(0x06, 0x02).Serialize();

            //Console.WriteLine($"Method - {Utils.Bytes2HexString(methodBytes)} + SignedExtensions - {Utils.Bytes2HexString(signedExtensionsBytes)}");
            //var payload = new List<byte>();
            //payload.AddRange(methodBytes);
            //payload.AddRange(signedExtensionsBytes);
            //var payloadBytes = payload.ToArray();
            //Console.WriteLine($"Payload: {Utils.Bytes2HexString(payloadBytes)}");
            //if (payloadBytes.Length > 256)
            //{
            //    Console.WriteLine("Payload is hashed with blake2b 256, as it is bigger then 256");
            //    payloadBytes = HashExtension.Blake2(payloadBytes, 256);
            //}
            //Console.WriteLine($"Payload: {Utils.Bytes2HexString(payloadBytes)}");

            //string priKey0x = "0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e";
            //string pubKey0x = "0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e";

            //var signedPayload = Chaos.NaCl.Ed25519.Sign(payloadBytes, Utils.HexToByteArray(priKey0x));
            //Console.WriteLine($"Signed Payload [{signedPayload.Length}]: {Utils.Bytes2HexString(signedPayload)}");


            //Console.WriteLine($"0x84 = {Utils.DecodeCompactInteger(Utils.HexToByteArray("0x84"))} CompactInteger");
            //Console.WriteLine($"0x02 = {Utils.DecodeCompactInteger(Utils.HexToByteArray("0x02"))} CompactInteger");
            //Console.WriteLine($"SignatureVersion (true): {Utils.Bytes2HexString(new byte[] {(byte)(4 | 0x80 )})}");
            //Console.WriteLine($"SignatureVersion (false): {Utils.Bytes2HexString(new byte[] { (byte)(4 | 0x00) })}");


            //Console.WriteLine($"0xc502  = {CompactInteger.Decode(Utils.HexToByteArray("0xc502"))} CompactInteger");
            //Console.WriteLine($"0xf502  = {CompactInteger.Decode(Utils.HexToByteArray("0xf502"))} CompactInteger");
            //Console.WriteLine($"0x0503  = {CompactInteger.Decode(Utils.HexToByteArray("0x0503"))} CompactInteger");

            /**
            ➜  ~subkey inspect - key //Alice --scheme=ed25519
                    Secret Key URI `//Alice` is account:
                      Secret seed:      0xabf8e5bdbe30c65656c0a3cbd181ff8a56294a69dfedd27982aace4a76909115
                      Public key(hex): 0x88dc3417d5058ec4b4503e0c12ea1a0a89be200fe98922423d4334014fa6b0ee
                      Account ID:       0x88dc3417d5058ec4b4503e0c12ea1a0a89be200fe98922423d4334014fa6b0ee
                      SS58 Address:     5FA9nQDVg267DEd8m1ZypXLBnvN7SFxYwV7ndqSYGiN9TTpu
                    ➜  ~subkey inspect - key //Alice --scheme=sr25519
                    Secret Key URI `//Alice` is account:
                      Secret seed:      0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a
                      Public key(hex): 0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
                      Account ID:       0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
                      SS58 Address:     5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY
            */
            //     278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e
            //0xff d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d  9101

        }
    }
}
