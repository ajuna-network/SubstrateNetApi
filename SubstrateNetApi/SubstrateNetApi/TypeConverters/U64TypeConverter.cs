/// <file> SubstrateNetApi\TypeConverters\U64TypeConverter.cs </file>
/// <copyright file="U64TypeConverter.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the 64 type converter class. </summary>
using Newtonsoft.Json;
using NLog;
using System;

namespace SubstrateNetApi.TypeConverters
{
    /// <summary> A 64 type converter. </summary>
    /// <remarks> 19.09.2020. </remarks>
    /// <seealso cref="ITypeConverter"/>
    internal class U64TypeConverter : JsonConverter<ulong>, ITypeConverter
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        /// <seealso cref="SubstrateNetApi.ITypeConverter.TypeName"/>
        public string TypeName { get; } = "u64";

        /// <summary> Creates a new object. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> An object. </returns>
        /// <seealso cref="ITypeConverter.Create(string)"/>
        public object Create(string value)
        {
            byte[] bytes = Utils.HexToByteArray(value);
            Logger.Debug($"Converting {value} [{bytes.Length}] to UInt64.");
            return BitConverter.ToUInt64(bytes, 0);
        }

        public override ulong ReadJson(JsonReader reader, Type objectType, ulong existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = (string)reader.Value;
            byte[] bytes = Utils.HexToByteArray(value);
            byte[] result = new byte[8];
            Array.Copy(bytes, 0, result, 0, bytes.Length);
            return BitConverter.ToUInt64(result, 0);
        }

        public override void WriteJson(JsonWriter writer, ulong value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value}");
        }
    }
}
