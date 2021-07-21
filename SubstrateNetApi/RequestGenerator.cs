using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.NaCl;
using Schnorrkel;
//using Schnorrkel;
using SubstrateNetApi.Exceptions;
using SubstrateNetApi.Model.Extrinsics;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi
{
    public class RequestGenerator
    {
        public static string GetStorage(Module module, Item item, string[] parameter = null)
        {
            var keybytes = GetStorageKeyBytesHash(module, item);

            byte[] parameterBytes = null;
            if (item.Function?.Key1 != null) parameterBytes = GetParameterBytes(item.Function.Key1, parameter);

            switch (item.Type)
            {
                case Storage.Type.Plain:
                    return Utils.Bytes2HexString(keybytes);

                case Storage.Type.Map:

                    switch (item.Function.Hasher)
                    {
                        case Storage.Hasher.Identity:
                            return Utils.Bytes2HexString(keybytes.Concat(parameterBytes).ToArray());

                        case Storage.Hasher.BlakeTwo128:
                        case Storage.Hasher.BlakeTwo256:
                        case Storage.Hasher.BlakeTwo128Concat:
                            return Utils.Bytes2HexString(keybytes.Concat(HashExtension.Blake2Concat(parameterBytes))
                                .ToArray());

                        case Storage.Hasher.Twox128:
                        case Storage.Hasher.Twox256:
                        case Storage.Hasher.Twox64Concat:
                            return Utils.Bytes2HexString(keybytes.Concat(HashExtension.XxHash128(parameterBytes))
                                .ToArray());

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
                    throw new MissingParameterException(
                        $"Needs {keys.Length} keys, but provided where {parameter.Length} keys!");
                var byteList = new List<byte>();
                for (var i = 0; i < keys.Length; i++)
                    byteList.AddRange(Utils.KeyTypeToBytes(keys[i].Trim(), parameter[i]));
                return byteList.ToArray();
            }
            // single key support

            return Utils.KeyTypeToBytes(key, parameter[0]);
        }

        public static byte[] GetStorageKeyBytesHash(Module module, Item item)
        {
            return GetStorageKeyBytesHash(module.Name, item.Name);
        }

        public static byte[] GetStorageKeyBytesHash(string module, string item)
        {
            var mBytes = Encoding.ASCII.GetBytes(module);
            var iBytes = Encoding.ASCII.GetBytes(item);
            return HashExtension.XxHash128(mBytes).Concat(HashExtension.XxHash128(iBytes)).ToArray();
        }

        internal static UnCheckedExtrinsic SubmitExtrinsic(bool signed, Account account, Method method, Era era,
            uint nonce, uint tip, Hash genesis, Hash startEra, RuntimeVersion runtime)
        {
            var uncheckedExtrinsic =
                new UnCheckedExtrinsic(signed, account, method, era, nonce, tip, genesis, startEra);

            if (!signed)
            {
                return uncheckedExtrinsic;
            }

            var payload = uncheckedExtrinsic.GetPayload(runtime).Encode();

            /// Payloads longer than 256 bytes are going to be `blake2_256`-hashed.
            if (payload.Length > 256) payload = HashExtension.Blake2(payload, 256);

            byte[] signature;
            switch (account.KeyType)
            {
                case KeyType.Ed25519:
                    signature = Ed25519.Sign(payload, account.PrivateKey);
                    break;
                case KeyType.Sr25519:
                    signature = Sr25519v091.SignSimple(account.Bytes, account.PrivateKey, payload);
                    break;
                default:
                    throw new Exception($"Unknown key type found '{account.KeyType}'.");
            }

            uncheckedExtrinsic.AddPayloadSignature(signature);
            return uncheckedExtrinsic;
        }
    }
}