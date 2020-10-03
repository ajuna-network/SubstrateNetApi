using Chaos.NaCl;
using Schnorrkel;
using SubstrateNetApi.MetaDataModel;
using SubstrateNetApi.MetaDataModel.Extrinsics;
using SubstrateNetApi.MetaDataModel.Values;
using System;
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

        internal static UnCheckedExtrinsic SubmitExtrinsic(bool signed, Account account, Method method, Era era, uint nonce, uint tip, Hash genesis, Hash startEra)
        {
            var uncheckedExtrinsic = new UnCheckedExtrinsic(signed, account, method, era, nonce, tip, genesis, startEra);

            if (!signed)
            {
                return uncheckedExtrinsic;
            }

            var payload = uncheckedExtrinsic.GetPayload().Encode();

            /// Payloads longer than 256 bytes are going to be `blake2_256`-hashed.
            if (payload.Length > 256)
            {
                payload = HashExtension.Blake2(payload, 256);
            }

            byte[] signature;
            switch (account.KeyType)
            {
                case KeyType.SR25519:
                    signature = Sr25519v091.SignSimple(account.PublicKey, account.PrivateKey, payload);
                    break;
                case KeyType.ED25519:
                    signature = Ed25519.Sign(payload, account.PrivateKey);
                    break;
                default:
                    throw new Exception($"Unknown key type found '{account.KeyType}'.");
            }

            uncheckedExtrinsic.AddPayloadSignature(signature);
            return uncheckedExtrinsic;
        }
    }
}
