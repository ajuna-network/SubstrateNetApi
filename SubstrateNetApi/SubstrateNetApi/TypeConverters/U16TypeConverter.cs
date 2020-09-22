/// <file> SubstrateNetApi\TypeConverters\U16TypeConverter.cs </file>
/// <copyright file="U16TypeConverter.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the 16 type converter class. </summary>
using Newtonsoft.Json;
using NLog;
using System;

namespace SubstrateNetApi.TypeConverters
{
    /// <summary> A 16 type converter. </summary>
    /// <remarks> 19.09.2020. </remarks>
    /// <seealso cref="ITypeConverter"/>
    internal class U16TypeConverter : JsonConverter<ushort>, ITypeConverter
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        /// <seealso cref="SubstrateNetApi.ITypeConverter.TypeName"/>
        public string TypeName { get; } = "u16";

        /// <summary> Creates a new object. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> An object. </returns>
        /// <seealso cref="ITypeConverter.Create(string)"/>
        public object Create(string value)
        {
            byte[] bytes = Utils.HexToByteArray(value);
            Logger.Debug($"Converting {value} [{bytes.Length}] to UInt16.");
            return BitConverter.ToUInt16(bytes, 0);
        }
        public override ushort ReadJson(JsonReader reader, Type objectType, ushort existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = (string)reader.Value;
            byte[] bytes = Utils.HexToByteArray(value);
            byte[] result = new byte[2];
            Array.Copy(bytes, 0, result, 0, bytes.Length);
            return BitConverter.ToUInt16(result, 0);
        }

        public override void WriteJson(JsonWriter writer, ushort value, JsonSerializer serializer)
        {
            writer.WriteValue(Utils.Bytes2HexString(BitConverter.GetBytes(value), Utils.HexStringFormat.PREFIXED));
        }
    }
}
