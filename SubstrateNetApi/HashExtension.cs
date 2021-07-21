using System;
using System.Collections.Generic;
using System.Linq;
using Blake2Core;
using Extensions.Data;

namespace SubstrateNetApi
{
    public class HashExtension
    {
        /// <summary>Xxes the hash128.</summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static byte[] XxHash128(byte[] bytes)
        {
            return BitConverter.GetBytes(XXHash.XXH64(bytes, 0)).Concat(BitConverter.GetBytes(XXHash.XXH64(bytes, 1)))
                .ToArray();
        }

        /// <summary>Blake2s the specified bytes.</summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="size">The size.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static byte[] Blake2(byte[] bytes, int size = 128, IReadOnlyList<byte> key = null)
        {
            var config = new Blake2BConfig {OutputSizeInBits = size, Key = null};
            return Blake2B.ComputeHash(bytes, config);
        }

        /// <summary>Blake2s the concat.</summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static byte[] Blake2Concat(byte[] bytes, int size = 128)
        {
            var config = new Blake2BConfig {OutputSizeInBits = size, Key = null};
            return Blake2B.ComputeHash(bytes, config).Concat(bytes).ToArray();
        }

        /// <summary>Blake2s the specified ss prefixed.</summary>
        /// <param name="ssPrefixed">The ss prefixed.</param>
        /// <param name="start">The start.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        internal static byte[] Blake2(byte[] ssPrefixed, int start, int count)
        {
            return Blake2B.ComputeHash(ssPrefixed, start, count);
        }
    }
}