using Chaos.NaCl;
using Schnorrkel;
using SubstrateNetApi.Exceptions;
using SubstrateNetApi.Model.Extrinsics;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubstrateNetApi
{

    public partial class RequestGenerator
    {
        public static string GetStorage(Module module, Item item, string[] parameter = null)
        {
            var keybytes = GetStorageKeyBytesHash(module, item);
            
            byte[] parameterBytes = null;
            if (item.Function?.Key1 != null)
            {
                parameterBytes = GetParameterBytes(item.Function.Key1, parameter);

            }

            switch (item.Type)
            {
                case Storage.Type.Plain:
                    return Utils.Bytes2HexString(keybytes, Utils.HexStringFormat.PREFIXED);

                case Storage.Type.Map:

                    switch (item.Function.Hasher)
                    {
                        case Storage.Hasher.Identity:
                            return Utils.Bytes2HexString(keybytes.Concat(parameterBytes).ToArray(), Utils.HexStringFormat.PREFIXED);

                        case Storage.Hasher.Blake2_128:
                        case Storage.Hasher.Blake2_256:
                        case Storage.Hasher.Blake2_128Concat:
                            return Utils.Bytes2HexString(keybytes.Concat(HashExtension.Blake2Concat(parameterBytes)).ToArray(), Utils.HexStringFormat.PREFIXED);

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
        private static byte[] GetParameterBytes(string key, string[] parameter)
        {
            // multi keys support
            if (key.StartsWith("("))
            {
                var keysDelimited = key.Replace("(", "").Replace(")", "");
                var keys = keysDelimited.Split(',');
                if (keys.Length != parameter.Length)
                {
                    throw new MissingParameterException($"Needs {keys.Length} keys, but provided where {parameter.Length} keys!");
                }
                List<byte> byteList = new List<byte>();
                for (int i = 0; i < keys.Length; i++)
                {
                    byteList.AddRange(Utils.KeyTypeToBytes(keys[i].Trim(), parameter[i]));
                }
                return byteList.ToArray();
            }
            // single key support
            else
            {
                return Utils.KeyTypeToBytes(key, parameter[0]);
            }
        }

        public static byte[] GetStorageKeyBytesHash(Module module, Item item)
        {
            var mBytes = Encoding.ASCII.GetBytes(module.Name);
            var iBytes = Encoding.ASCII.GetBytes(item.Name);
            return HashExtension.XXHash128(mBytes).Concat(HashExtension.XXHash128(iBytes)).ToArray();
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
