/// <file> SubstrateNetApi\TypeConverters\U16TypeConverter.cs </file>
/// <copyright file="U16TypeConverter.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the 16 type converter class. </summary>
using Newtonsoft.Json;
using NLog;
using System;
using static SubstrateNetApi.Utils;

namespace SubstrateNetApi.TypeConverters
{
    /// <summary> A 8 type converter. </summary>
    /// <remarks> 19.09.2020. </remarks>
    /// <seealso cref="ITypeConverter"/>
    internal class U8TypeConverter : JsonConverter<byte>, ITypeConverter
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        /// <seealso cref="SubstrateNetApi.ITypeConverter.TypeName"/>
        public string TypeName { get; } = "u8";

        /// <summary> Creates a new object. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> An object. </returns>
        /// <seealso cref="ITypeConverter.Create(string)"/>
        public object Create(string value)
        {
            byte[] bytes = Utils.HexToByteArray(value);
            Logger.Debug($"Converting {value} [{bytes.Length}] to byte.");
            return BitConverter.ToUInt16(bytes, 0);
        }
        public override byte ReadJson(JsonReader reader, Type objectType, byte existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = (string)reader.Value;
            byte[] bytes = Utils.HexToByteArray(value, true);
            if (bytes.Length != 1)
            {
                throw new Exception($"Can't deserialize '{value}' as byte!");
            }
            return bytes[0];
        }

        public override void WriteJson(JsonWriter writer, byte value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value}");
        }
    }
}
