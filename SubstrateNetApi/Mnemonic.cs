using System.Text;
using System.Security.Cryptography;
using Schnorrkel.Keys;
using System;
using dotnetstandard_bip39;
using System.IO;
using dotnetstandard_bip39.System.Security.Cryptography;

namespace SubstrateNetApi
{
    public static class Mnemonic
    {
        /// <summary>
        /// Get seed from entropy bytes, make  sure entropy is a byte array from a correctly recovered and checksumed BIP39.
        /// Following slices are supported:
        /// + 16 bytes for 12 words.
        /// + 20 bytes for 15 words.
        /// + 24 bytes for 18 words.
        /// + 28 bytes for 21 words.
        /// + 32 bytes for 24 words.
        /// Other slices will lead to a InvalidEntropy error. 
        /// https://github.com/paritytech/substrate-bip39/blob/eef2f86337d2dab075806c12948e8a098aa59d59/src/lib.rs#L45
        /// </summary>
        /// <param name="entropyBytes"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static byte[] SeedFromEntropy(byte[] entropyBytes, string password)
        {
            if (entropyBytes.Length < 16 || entropyBytes.Length > 32 || entropyBytes.Length % 4 != 0)
            {
                throw new Exception($"InvalidEntropy, length not allowed '{entropyBytes.Length}'");
            }
            var saltBytes = Encoding.UTF8.GetBytes("mnemonic" + password);

            var rfc2898DerivedBytes = new Rfc2898DeriveBytesExtended(entropyBytes, saltBytes, 2048, HashAlgorithmName.SHA512);
            var key = rfc2898DerivedBytes.GetBytes(64);

            //return PBKDF2Sha512GetBytes(64, entropyBytes, saltBytes, 2048);
            return key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mnemonic"></param>
        /// <param name="bIP39Wordlist"></param>
        /// <returns></returns>
        public static string GetEntropy(string mnemonic, BIP39Wordlist bIP39Wordlist) 
            => new BIP39().MnemonicToEntropy(mnemonic, bIP39Wordlist);

        /// <summary>
        /// Get secret key from mnemonic
        /// </summary>
        /// <param name="mnemonic"></param>
        /// <param name="password"></param>
        /// <param name="bIP39Wordlist"></param>
        /// <returns></returns>
        public static byte[] GetSecretKeyFromMnemonic(string mnemonic, string password, BIP39Wordlist bIP39Wordlist)
        {
            var entropyBytes = Utils.HexToByteArray(GetEntropy(mnemonic, bIP39Wordlist));
            var seedBytes = SeedFromEntropy(entropyBytes, password);
            return seedBytes.AsMemory().Slice(0, 32).ToArray();
        }

        /// <summary>
        /// Get a key pair from mnemonic
        /// </summary>
        /// <param name="mnemonic"></param>
        /// <param name="password"></param>
        /// <param name="bIP39Wordlist"></param>
        /// <param name="expandMode"></param>
        /// <returns></returns>
        public static KeyPair GetKeyPairFromMnemonic(string mnemonic, string password, BIP39Wordlist bIP39Wordlist, ExpandMode expandMode)
        {
            var secretBytes = GetSecretKeyFromMnemonic(mnemonic, password, bIP39Wordlist);
            var miniSecret = new MiniSecret(secretBytes, expandMode);
            return new KeyPair(miniSecret.ExpandToPublic(), miniSecret.ExpandToSecret());
        }
    }
}
