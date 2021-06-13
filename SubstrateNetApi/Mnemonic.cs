using System.Text;
using System.Security.Cryptography;
using Schnorrkel.Keys;
using System;
using dotnetstandard_bip39;
using System.IO;

namespace SubstrateNetApi
{
    public static class Mnemonic
    {
        /// <summary>
        /// Rfc2898DeriveBytes, with HMACSHA512 usable for .NETStandard 2.0
        /// </summary>
        /// <param name="dklen"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iterationCount"></param>
        /// <returns></returns>
        public static byte[] PBKDF2Sha512GetBytes(int dklen, byte[] password, byte[] salt, int iterationCount)
        {
            using (var hmac = new HMACSHA512(password))
            {
                int hashLength = hmac.HashSize / 8;
                if ((hmac.HashSize & 7) != 0)
                    hashLength++;
                int keyLength = dklen / hashLength;
                if (dklen > (0xFFFFFFFFL * hashLength) || dklen < 0)
                {
                    throw new ArgumentOutOfRangeException("dklen");
                }
                if (dklen % hashLength != 0)
                {
                    keyLength++;
                }
                byte[] extendedkey = new byte[salt.Length + 4];
                Buffer.BlockCopy(salt, 0, extendedkey, 0, salt.Length);
                using (var ms = new MemoryStream())
                {
                    for (int i = 0; i < keyLength; i++)
                    {
                        extendedkey[salt.Length] = (byte)(((i + 1) >> 24) & 0xFF);
                        extendedkey[salt.Length + 1] = (byte)(((i + 1) >> 16) & 0xFF);
                        extendedkey[salt.Length + 2] = (byte)(((i + 1) >> 8) & 0xFF);
                        extendedkey[salt.Length + 3] = (byte)(((i + 1)) & 0xFF);
                        byte[] u = hmac.ComputeHash(extendedkey);
                        Array.Clear(extendedkey, salt.Length, 4);
                        byte[] f = u;
                        for (int j = 1; j < iterationCount; j++)
                        {
                            u = hmac.ComputeHash(u);
                            for (int k = 0; k < f.Length; k++)
                            {
                                f[k] ^= u[k];
                            }
                        }
                        ms.Write(f, 0, f.Length);
                        Array.Clear(u, 0, u.Length);
                        Array.Clear(f, 0, f.Length);
                    }
                    byte[] dk = new byte[dklen];
                    ms.Position = 0;
                    ms.Read(dk, 0, dklen);
                    ms.Position = 0;
                    for (long i = 0; i < ms.Length; i++)
                    {
                        ms.WriteByte(0);
                    }
                    Array.Clear(extendedkey, 0, extendedkey.Length);
                    return dk;
                }
            }
        }

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
            return PBKDF2Sha512GetBytes(64, entropyBytes, saltBytes, 2048);
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
        public static KeyPair GetKeyPairFromMnemonic(string mnemonic, string password, BIP39Wordlist bIP39Wordlist)
        {
            var secretBytes = GetSecretKeyFromMnemonic(mnemonic, password, bIP39Wordlist);
            var miniSecret = new MiniSecret(secretBytes, ExpandMode.Ed25519);
            return new KeyPair(miniSecret.ExpandToPublic(), miniSecret.ExpandToSecret());
        }
    }
}
