using System;
using System.Collections.Generic;
using System.Linq;
using Blake2Core;
using Extensions.Data;
using SubstrateNetApi.Model.Meta;

namespace SubstrateNetApi
{
    public class HashExtension
    {
        public static byte[] Hash(Storage.Hasher hasher, byte[] bytes)
        {
            switch (hasher)
            {
                case Storage.Hasher.Identity:
                    return bytes;

                case Storage.Hasher.BlakeTwo128:
                    return Blake2(bytes, 128);

                case Storage.Hasher.BlakeTwo256:
                    return Blake2(bytes, 256);

                case Storage.Hasher.BlakeTwo128Concat:
                    return Blake2Concat(bytes, 128);

                case Storage.Hasher.Twox128:
                    return Twox128(bytes);

                case Storage.Hasher.Twox256:
                    return Twox256(bytes);

                case Storage.Hasher.Twox64Concat:
                    return Twox64Concat(bytes);

                case Storage.Hasher.None:
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>Blake2 hashed the specified bytes.</summary>
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

        /// <summary>Blake2 hashed with bytes concated at the end.</summary>
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

        /// <summary>Blake2 hashed the specified ss prefixed.</summary>
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

        /// <summary>XXHash 128 bytes.</summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static byte[] Twox128(byte[] bytes)
        {
            return BitConverter.GetBytes(XXHash.XXH64(bytes, 0))
                .Concat(BitConverter.GetBytes(XXHash.XXH64(bytes, 1)))
                .ToArray();
        }

        /// <summary>XXHash 256 bytes.</summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static byte[] Twox256(byte[] bytes)
        {
            return BitConverter.GetBytes(XXHash.XXH64(bytes, 0))
                .Concat(BitConverter.GetBytes(XXHash.XXH64(bytes, 1)))
                .Concat(BitConverter.GetBytes(XXHash.XXH64(bytes, 2)))
                .Concat(BitConverter.GetBytes(XXHash.XXH64(bytes, 3)))
                .ToArray();
        }

        /// <summary>XXHashed 64 hashed with bytes concated at the end.</summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static byte[] Twox64Concat(byte[] bytes)
        {
            return BitConverter.GetBytes(XXHash.XXH64(bytes)).Concat(bytes).ToArray().ToArray();
        }
    }
}