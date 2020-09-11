using Schnorrkel;
using SubstrateNetApi.MetaDataModel;
using System;
using System.Linq;
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

            throw new NotImplementedException();
        }
    }
}
