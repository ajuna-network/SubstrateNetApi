using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SubstrateNetApi
{
    public class Utils
    {
        public enum HexStringFormat { PURE, DASH, PREFIXED }

        public static string Bytes2HexString(byte[] bytes, HexStringFormat format = HexStringFormat.PREFIXED)
        {
            switch (format)
            {
                case HexStringFormat.PURE:
                    return BitConverter.ToString(bytes).Replace("-", string.Empty);
                case HexStringFormat.DASH:
                    return BitConverter.ToString(bytes);
                case HexStringFormat.PREFIXED:
                    return $"0x{BitConverter.ToString(bytes).Replace("-", string.Empty)}";
                default:
                    throw new Exception($"Unimplemented hex string format '{format}'");
            }
        }

        public static byte[] HexToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            if (hex.StartsWith("0x"))
            {
                hex = hex.Substring(2);
            }

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static byte[] GetPublicKeyFrom(string address)
        {
            int PUBLIC_KEY_LENGTH = 32;

            var pubkByteList = new List<byte>();

            var bs58decoded = SimpleBase.Base58.Bitcoin.Decode(address).ToArray();
            int len = bs58decoded.Length;

            if (len == 35)
            {
                byte[] ssPrefixed = { 0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45 };
                pubkByteList.AddRange(ssPrefixed);
                pubkByteList.AddRange(bs58decoded.Take(PUBLIC_KEY_LENGTH + 1));

                var blake2bHashed = HashExtension.Blake2(pubkByteList.ToArray(), 512);
                if (bs58decoded[PUBLIC_KEY_LENGTH + 1] != blake2bHashed[0] ||
                    bs58decoded[PUBLIC_KEY_LENGTH + 2] != blake2bHashed[1])
                {
                    throw new ApplicationException("Address checksum is wrong.");
                }

                return bs58decoded.Skip(1).Take(PUBLIC_KEY_LENGTH).ToArray();
            }

            throw new ApplicationException("Address checksum is wrong.");
        }

        internal static byte[] KeyTypeToBytes(string keyType, string parameter)
        {
            switch (keyType)
            {
                case "u16":
                    return BitConverter.GetBytes(UInt16.Parse(parameter));
                case "u32":
                    return BitConverter.GetBytes(UInt32.Parse(parameter));
                case "u64":
                    return BitConverter.GetBytes(UInt64.Parse(parameter));
                case "T::Hash":
                    return new Hash(parameter).Bytes;
                case "T::AccountId":
                    return new AccountId(parameter).PublicKey;
                default:
                    throw new Exception("Unimplemented item function key 'item.Function.Key1'!");
            }

        }

        public static string GetAddressFrom(byte[] bytes)
        {
            int SR25519_PUBLIC_SIZE = 32;
            int PUBLIC_KEY_LENGTH = 32;

            var plainAddr = Enumerable
                .Repeat((byte)0x2A, 35)
                .ToArray();

            bytes.CopyTo(plainAddr.AsMemory(1));

            var ssPrefixed = new byte[SR25519_PUBLIC_SIZE + 8];
            var ssPrefixed1 = new byte[] { 0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45 };
            ssPrefixed1.CopyTo(ssPrefixed, 0);
            plainAddr.AsSpan(0, SR25519_PUBLIC_SIZE + 1).CopyTo(ssPrefixed.AsSpan(7));

            var blake2bHashed = HashExtension.Blake2(ssPrefixed, 0, SR25519_PUBLIC_SIZE + 8);
            plainAddr[1 + PUBLIC_KEY_LENGTH] = blake2bHashed[0];
            plainAddr[2 + PUBLIC_KEY_LENGTH] = blake2bHashed[1];

            var addrCh = SimpleBase.Base58.Bitcoin.Encode(plainAddr).ToArray();

            return new string(addrCh);
        }

        public static BigInteger DecodeCompactInteger(byte[] m)
        {
            int p = 0;
            return DecodeCompactInteger(m, ref p);
        }

        public static BigInteger DecodeCompactInteger(byte[] m, ref int p)
        {
            uint first_byte = m[p++];
            uint flag = (first_byte) & 0b00000011u;
            uint number = 0u;

            switch (flag)
            {
                case 0b00u:
                    {
                        number = first_byte >> 2;
                        break;
                    }

                case 0b01u:
                    {
                        uint second_byte = m[p++];

                        number = ((uint)((first_byte) & 0b11111100u) + (uint)(second_byte) * 256u) >> 2;
                        break;
                    }

                case 0b10u:
                    {
                        number = first_byte;
                        uint multiplier = 256u;

                        for (var i = 0; i < 3; ++i)
                        {
                            number += m[p++] * multiplier;
                            multiplier = multiplier << 8;
                        }
                        number = number >> 2;
                        break;
                    }

                case 0b11:
                    {
                        uint bytes_count = ((first_byte) >> 2) + 4u;
                        CompactInteger multiplier = new CompactInteger { Value = 1u };
                        CompactInteger value = new CompactInteger { Value = 0 };

                        // we assured that there are m more bytes,
                        // no need to make checks in a loop
                        for (var i = 0; i < bytes_count; ++i)
                        {
                            value += multiplier * m[p++];
                            multiplier *= 256u;
                        }

                        return value.Value;
                    }

                default:
                    throw new Exception("CompactInteger decode error: unknown flag");
            }

            return new BigInteger(number);
        }

        public static byte[] EncodeCompactInteger(BigInteger n)
        {
            if (n <= 63)
            {
                return new byte[] { (byte)(n << 2) };
            }
            else if (n <= 0x3FFF)
            {
                return new byte[] { (byte)(((n & 0x3F) << 2) | 0x01), (byte)((n & 0xFC0) >> 6) };
            }
            else if (n <= 0x3FFFFFFF)
            {
                var result = new byte[4];
                result[0] = (byte)(((n & 0x3F) << 2) | 0x02);
                n >>= 6;
                for (int i = 1; i < 4; ++i)
                {
                    result[i] = (byte)(n & 0xFF);
                    n >>= 8;
                }
                return result;
            }
            else
            {
                var b0 = new List<byte>();
                while (n > 0)
                {
                    b0.Add((byte)(n & 0xFF));
                    n >>= 8;
                }
                var result = new List<byte>
                {
                    (byte)(((b0.Count - 4) << 2) | 0x03)
                };
                result.AddRange(b0);
                return result.ToArray();
            }
        }

    }
}
