using System;
using System.Collections.Generic;

using System.Linq;
using Extensions.Data;

namespace SubstrateNetApi
{
    public class HashExtension
    {

        public static byte[] XXHash128(byte[] bytes)
        {
            return BitConverter.GetBytes(XXHash.XXH64(bytes, 0)).Concat(BitConverter.GetBytes(XXHash.XXH64(bytes, 1))).ToArray();
        }

        public static byte[] Blake2(byte[] bytes, int size = 128, IReadOnlyList<byte> key = null)
        {
            var config = new Blake2Core.Blake2BConfig() { OutputSizeInBits = size, Key = null };
            return Blake2Core.Blake2B.ComputeHash(bytes, config);

        }

        public static byte[] Blake2Concat(byte[] bytes, int size = 128)
        {
            var config = new Blake2Core.Blake2BConfig() { OutputSizeInBits = size, Key = null };
            return Blake2Core.Blake2B.ComputeHash(bytes, config).Concat(bytes).ToArray();
        }

        private static byte[] Blake2_128Concat(byte[] bytes, int length)
        {
            var config = new Blake2Core.Blake2BConfig { OutputSizeInBytes = 16 };
            var b2Hash = Blake2Core.Blake2B.ComputeHash(bytes, 0, length, config);
            //var result = new byte[b2Hash.Length + bytes.Length];
            //for (int i = 0; i < result.Length; i++)
            //{
            //    result[i] = i < b2Hash.Length ? b2Hash[i] : bytes[i- b2Hash.Length];
            //}
            //return result;
            return b2Hash.Concat(bytes).ToArray();
        }

        internal static byte[] Blake2(byte[] ssPrefixed, int start, int count)
        {
            return Blake2Core.Blake2B.ComputeHash(ssPrefixed, start, count);
        }
    }
}
