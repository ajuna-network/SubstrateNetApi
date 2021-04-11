using System;
using System.Collections.Generic;
using System.Linq;
using SimpleBase;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi
{
    /// <summary> An utilities. </summary>
    /// <remarks> 19.09.2020. </remarks>
    public class Utils
    {
        /// <summary> Values that represent Hexadecimal string formats. </summary>
        /// <remarks> 19.09.2020. </remarks>
        public enum HexStringFormat
        {
            Pure,
            Dash,
            Prefixed
        }

        /// <summary> Bytes 2 hexadecimal string. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <exception cref="Exception"> Thrown when an exception error condition occurs. </exception>
        /// <param name="bytes">  The bytes. </param>
        /// <param name="format"> (Optional) Describes the format to use. </param>
        /// <returns> A string. </returns>
        public static string Bytes2HexString(byte[] bytes, HexStringFormat format = HexStringFormat.Prefixed)
        {
            switch (format)
            {
                case HexStringFormat.Pure:
                    return BitConverter.ToString(bytes).Replace("-", string.Empty);
                case HexStringFormat.Dash:
                    return BitConverter.ToString(bytes);
                case HexStringFormat.Prefixed:
                    return $"0x{BitConverter.ToString(bytes).Replace("-", string.Empty)}";
                default:
                    throw new Exception($"Unimplemented hex string format '{format}'");
            }
        }

        /// <summary> String value array bytes array. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <exception cref="Exception"> Thrown when an exception error condition occurs. </exception>
        /// <param name="valueArray"> Array of values. </param>
        /// <returns> A byte[]. </returns>
        internal static byte[] StringValueArrayBytesArray(string valueArray)
        {
            var strArray = valueArray
                .Replace("[", "")
                .Replace("]", "")
                .Replace(" ", "")
                .Split(',');

            var result = new byte[strArray.Length];

            for (var i = 0; i < strArray.Length; i++)
                if (byte.TryParse(strArray[i], out var parsedByte))
                    result[i] = parsedByte;
                else
                    throw new Exception(
                        "Not valid string array for byte array conversion. Format should be [ 0-255, 0-255, ...]");

            return result;
        }

        /// <summary> Hexadecimal to byte array. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <exception cref="Exception"> Thrown when an exception error condition occurs. </exception>
        /// <param name="hex"> The hexadecimal. </param>
        /// <returns> A byte[]. </returns>
        public static byte[] HexToByteArray(string hex, bool evenLeftZeroPad = false)
        {
            if (hex.Equals("0x0")) return new byte[] {0x00};

            if (hex.Length % 2 == 1 && !evenLeftZeroPad)
                throw new Exception("The binary key cannot have an odd number of digits");

            if (hex.StartsWith("0x")) hex = hex.Substring(2);

            if (hex.Length % 2 != 0) hex = hex.PadLeft(hex.Length + 1, '0');

            var arr = new byte[hex.Length >> 1];

            for (var i = 0; i < hex.Length >> 1; ++i)
                arr[i] = (byte) ((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));

            return arr;
        }

        /// <summary> Bytes 2 value. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <exception cref="Exception"> Thrown when an exception error condition occurs. </exception>
        /// <param name="value">        The value. </param>
        /// <param name="littleEndian"> (Optional) True to little endian. </param>
        /// <returns> An object. </returns>
        public static object Bytes2Value(byte[] value, bool littleEndian = true)
        {
            if (!littleEndian) Array.Reverse(value);

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

        /// <summary> Size prefixed byte array. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="list"> The list. </param>
        /// <returns> A byte[]. </returns>
        public static byte[] SizePrefixedByteArray(List<byte> list)
        {
            var result = new List<byte>();
            result.AddRange(new CompactInteger(list.Count).Encode());
            result.AddRange(list);
            return result.ToArray();
        }

        /// <summary> Value 2 bytes. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <exception cref="Exception"> Thrown when an exception error condition occurs. </exception>
        /// <param name="value">        The value. </param>
        /// <param name="littleEndian"> (Optional) True to little endian. </param>
        /// <returns> A byte[]. </returns>
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

            if (!littleEndian) Array.Reverse(result);

            return result;
        }

        /// <summary> Gets hexadecimal value. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="hex"> The hexadecimal. </param>
        /// <returns> The hexadecimal value. </returns>
        public static int GetHexVal(char hex)
        {
            int val = hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : val < 97 ? 55 : 87);
        }

        /// <summary> Gets public key from. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <exception cref="ApplicationException"> Thrown when an Application error condition occurs. </exception>
        /// <param name="address"> The address. </param>
        /// <returns> An array of byte. </returns>
        public static byte[] GetPublicKeyFrom(string address)
        {
            var PUBLIC_KEY_LENGTH = 32;

            var pubkByteList = new List<byte>();

            var bs58decoded = Base58.Bitcoin.Decode(address).ToArray();
            var len = bs58decoded.Length;

            if (len == 35)
            {
                byte[] ssPrefixed = {0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45};
                pubkByteList.AddRange(ssPrefixed);
                pubkByteList.AddRange(bs58decoded.Take(PUBLIC_KEY_LENGTH + 1));

                var blake2bHashed = HashExtension.Blake2(pubkByteList.ToArray(), 512);
                if (bs58decoded[PUBLIC_KEY_LENGTH + 1] != blake2bHashed[0] ||
                    bs58decoded[PUBLIC_KEY_LENGTH + 2] != blake2bHashed[1])
                    throw new ApplicationException("Address checksum is wrong.");

                return bs58decoded.Skip(1).Take(PUBLIC_KEY_LENGTH).ToArray();
            }

            throw new ApplicationException("Address checksum is wrong.");
        }

        /// <summary> Key type to bytes. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <exception cref="Exception"> Thrown when an exception error condition occurs. </exception>
        /// <param name="keyType">   Type of the key. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <returns> A byte[]. </returns>
        internal static byte[] KeyTypeToBytes(string keyType, string parameter)
        {
            switch (keyType)
            {
                case "u16":
                    return BitConverter.GetBytes(ushort.Parse(parameter));
                case "u32":
                    return BitConverter.GetBytes(uint.Parse(parameter));
                case "u64":
                    return BitConverter.GetBytes(ulong.Parse(parameter));
                case "T::Hash":
                    var hash = new Hash();
                    hash.Create(parameter);
                    return hash.Bytes;
                case "T::AccountId":
                    var accountId = new AccountId();
                    accountId.Create(parameter);
                    return accountId.Bytes;
                case "Vec<u8>":
                    var vecU8 =  Utils.SizePrefixedByteArray(Utils.HexToByteArray(parameter).ToList());
                    return vecU8;
                default:
                    throw new Exception("Unimplemented item function key 'item.Function.Key1'!");
            }
        }

        /// <summary> Gets address from. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="bytes"> The bytes. </param>
        /// <returns> The address from. </returns>
        public static string GetAddressFrom(byte[] bytes)
        {
            var SR25519_PUBLIC_SIZE = 32;
            var PUBLIC_KEY_LENGTH = 32;

            var plainAddr = Enumerable
                .Repeat((byte) 0x2A, 35)
                .ToArray();

            bytes.CopyTo(plainAddr.AsMemory(1));

            var ssPrefixed = new byte[SR25519_PUBLIC_SIZE + 8];
            var ssPrefixed1 = new byte[] {0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45};
            ssPrefixed1.CopyTo(ssPrefixed, 0);
            plainAddr.AsSpan(0, SR25519_PUBLIC_SIZE + 1).CopyTo(ssPrefixed.AsSpan(7));

            var blake2bHashed = HashExtension.Blake2(ssPrefixed, 0, SR25519_PUBLIC_SIZE + 8);
            plainAddr[1 + PUBLIC_KEY_LENGTH] = blake2bHashed[0];
            plainAddr[2 + PUBLIC_KEY_LENGTH] = blake2bHashed[1];

            var addrCh = Base58.Bitcoin.Encode(plainAddr).ToArray();

            return new string(addrCh);
        }
    }
}