using System;
using System.Numerics;
using SubstrateNetApi;

namespace TestExtrinsic
{
    class Program
    {
        static void Main(string[] args)
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


            string dmogCreate = "0xa50184278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00cbbf8076d31e163051556563a9de71816ba05fac08b905b14c2e6d266b7c621f8abadb2776c6d35f1990ed0a3fd768493ce85ac78ef654d69760e7d80273af01f5020849130602";
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
            Console.WriteLine($"sendPublicKey: {Utils.GetAddressFrom(sendPublicKey)}");
            byteString = byteString.Substring(sendPublicKey.Length * 2);
            bytes = Utils.HexToByteArray(byteString);

            // seperator1
            byte[] seperator1 = Utils.HexToByteArray(byteString.Substring(0, 2));
            Console.WriteLine($"seperator1: {Utils.Bytes2HexString(seperator1)}");
            byteString = byteString.Substring(seperator1.Length * 2);
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


            //Console.WriteLine($"0x84 = {Utils.DecodeCompactInteger(Utils.HexToByteArray("0x84"))} CompactInteger");
            //Console.WriteLine($"0x02 = {Utils.DecodeCompactInteger(Utils.HexToByteArray("0x02"))} CompactInteger");
        }

        private static int GetByteArrayPiece(byte[] bytes, out byte[] sliceBytes)
        {
            var l = CompactInteger.Decode(bytes);
            sliceBytes = bytes.AsMemory().Slice(0, - (int) l).ToArray();
            return (int) l;
        }
    }
}
