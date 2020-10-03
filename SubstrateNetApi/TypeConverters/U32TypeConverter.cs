/// <file> SubstrateNetApi\TypeConverters\U32TypeConverter.cs </file>
/// <copyright file="U32TypeConverter.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the 32 type converter class. </summary>
using Newtonsoft.Json;
using NLog;
using System;

namespace SubstrateNetApi.TypeConverters
{
    /// <summary> A 32 type converter. </summary>
    /// <remarks> 19.09.2020. </remarks>
    /// <seealso cref="ITypeConverter"/>
    internal class U32TypeConverter : JsonConverter<uint>, ITypeConverter
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        /// <seealso cref="SubstrateNetApi.ITypeConverter.TypeName"/>
        public string TypeName { get; } = "u32";

        /// <summary> Creates a new object. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> An object. </returns>
        /// <seealso cref="ITypeConverter.Create(string)"/>
        public object Create(string value)
        {
            byte[] bytes = Utils.HexToByteArray(value);
            Logger.Debug($"Converting {value} [{bytes.Length}] to UInt32.");
            return BitConverter.ToUInt32(bytes, 0);
        }

        public override uint ReadJson(JsonReader reader, Type objectType, uint existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = (string)reader.Value;
            byte[] bytes = Utils.HexToByteArray(value, true);
            Array.Reverse(bytes);
            byte[] result = new byte[4];
            Array.Copy(bytes, 0, result, 0, bytes.Length);
            return BitConverter.ToUInt32(result, 0);
        }

        public override void WriteJson(JsonWriter writer, uint value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value}");
        }
    }
}
