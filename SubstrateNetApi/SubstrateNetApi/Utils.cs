using Chaos.NaCl;
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

        internal static byte[] StringValueArrayBytesArray(string valueArray)
        {
            var strArray = valueArray
                .Replace("[","")
                .Replace("]", "")
                .Replace(" ", "")
                .Split(',');

            var result = new byte[strArray.Length];

            for (int i = 0; i < strArray.Length; i++)
            {
                if (byte.TryParse(strArray[i], out byte parsedByte))
                {
                    result[i] = parsedByte;
                } else
                {
                    throw new Exception("Not valid string array for byte array conversion. Format should be [ 0-255, 0-255, ...]");
                }
            }

            return result;
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

        public static object Bytes2Value(byte[] value, bool littleEndian = true)
        {
            if (!littleEndian)
            {
                Array.Reverse(value);
            }

            switch (value.Length)
            {
                case 2:
                    return BitConverter.ToUInt16(value, 0);
                case 4:
                    return BitConverter.ToUInt32(value, 0);
                case 8:
                    return BitConverter.ToUInt64(value, 0);
                default:
                    throw new Exception($"Unhandled byte size {value.Length} for this method!");
            }

        }

        public static byte[] SizePrefixedByteArray(List<byte> list)
        {
            var result = new List<byte>();
            result.AddRange(new CompactInteger(list.Count).Encode());
            result.AddRange(list);
            return result.ToArray();
        }

        public static byte[] Value2Bytes(object value, bool littleEndian = true)
        {
            byte[] result;

            switch (value)
            {
                case ushort s:
                    result = BitConverter.GetBytes(s);
                    break;
                case uint s:
                    result = BitConverter.GetBytes(s);
                    break;
                case ulong s:
                    result = BitConverter.GetBytes(s);
                    break;
                default:
                    throw new Exception("Unhandled byte size for this method!");
            }

            if (!littleEndian)
            {
                Array.Reverse(result);
            }

            return result;
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
    }
}
