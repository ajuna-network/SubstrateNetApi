using Schnorrkel;
using SubstrateNetApi.MetaDataModel;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace SubstrateNetApi
{

    public partial class RequestGenerator
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

        internal static string SubmitExtrinsic(byte moduleIndex, byte callIndex, byte[] parameters, uint nonce, byte[] pubKey, byte[] priKey)
        {
            ulong currentBlockNumber = 0;


            //var uncheckedExtrinsic = new UnCheckedExtrinsic(true, pubKeyType, pubKey, nonce, moduleIndex, callIndex, parameters, genesisHash, currentBlockHash, currentBlockNumber, tip);

            //var hashedPayload = HashExtension.Blake2(uncheckedExtrinsic.GetPayload(), 256);
            //var signedPayload = Sr25519v091.SignSimple(pubKey, priKey, hashedPayload);

            //uncheckedExtrinsic.SignatureType = 0x01;
            //uncheckedExtrinsic.Signature = signedPayload;

            return null; //Utils.Bytes2HexString(uncheckedExtrinsic.GetExtrinsic());
        }
    }
}
