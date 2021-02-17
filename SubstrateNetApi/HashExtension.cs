using System;
using System.Collections.Generic;
using System.Linq;
using Blake2Core;
using Extensions.Data;

namespace SubstrateNetApi
{
    public class HashExtension
    {
        public static byte[] XxHash128(byte[] bytes)
        {
            return BitConverter.GetBytes(XXHash.XXH64(bytes, 0)).Concat(BitConverter.GetBytes(XXHash.XXH64(bytes, 1)))
                .ToArray();
        }

        public static byte[] Blake2(byte[] bytes, int size = 128, IReadOnlyList<byte> key = null)
        {
            var config = new Blake2BConfig {OutputSizeInBits = size, Key = null};
            return Blake2B.ComputeHash(bytes, config);
        }

        public static byte[] Blake2Concat(byte[] bytes, int size = 128)
        {
            var config = new Blake2BConfig {OutputSizeInBits = size, Key = null};
            return Blake2B.ComputeHash(bytes, config).Concat(bytes).ToArray();
        }

        internal static byte[] Blake2(byte[] ssPrefixed, int start, int count)
        {
            return Blake2B.ComputeHash(ssPrefixed, start, count);
        }
    }
}