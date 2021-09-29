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
    /// <summary>
    /// Request Generator creates a requests for storage queries or extrinsic calls.
    /// </summary>
    public class RequestGenerator
    {
        /// <summary>
        /// Create a request for a storage call, for generated code.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="hashers"></param>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="key1Param"></param>
        /// <param name="key2Param"></param>
        /// <returns></returns>
        public static string GetStorage(string module, string item, Storage.Type type, Storage.Hasher[] hashers = null, IType[] keys = null)
        {
            var keybytes = GetStorageKeyBytesHash(module, item);

            switch (type)
            {
                case Storage.Type.Plain:
                    return Utils.Bytes2HexString(keybytes);

                case Storage.Type.Map:
                    for (int i = 0; i < hashers.Length; i++)
                    {
                        var key = keys[i].Encode();
                        var hasher = hashers[i];
                        keybytes = keybytes.Concat(HashExtension.Hash(hasher, key)).ToArray();
                    }
                    return Utils.Bytes2HexString(keybytes);

                default:
                    throw new NotSupportedException();
            }
        }



        /// <summary>
        /// Gets the parameter bytes.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <exception cref="MissingParameterException">Needs {keys.Length} keys, but provided where {parameter.Length} keys!</exception>
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

        /// <summary>
        /// Gets the storage key bytes hash.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static byte[] GetStorageKeyBytesHash(string module, string item)
        {
            var mBytes = Encoding.ASCII.GetBytes(module);
            var iBytes = Encoding.ASCII.GetBytes(item);
            return HashExtension.Twox128(mBytes).Concat(HashExtension.Twox128(iBytes)).ToArray();
        }

        /// <summary>
        /// Submits the extrinsic.
        /// </summary>
        /// <param name="signed">if set to <c>true</c> [signed].</param>
        /// <param name="account">The account.</param>
        /// <param name="method">The method.</param>
        /// <param name="era">The era.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="tip">The tip.</param>
        /// <param name="genesis">The genesis.</param>
        /// <param name="startEra">The start era.</param>
        /// <param name="runtime">The runtime.</param>
        /// <returns></returns>
        /// <exception cref="UnCheckedExtrinsic">signed, account, method, era, nonce, tip, genesis, startEra</exception>
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