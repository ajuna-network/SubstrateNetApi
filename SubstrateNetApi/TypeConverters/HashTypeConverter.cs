/// <file> SubstrateNetApi\TypeConverters\HashTypeConverter.cs </file>
/// <copyright file="HashTypeConverter.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the hash type converter class. </summary>

using Newtonsoft.Json;
using NLog;
using SubstrateNetApi.MetaDataModel.Types;
using System;

namespace SubstrateNetApi.TypeConverters
{
    /// <summary> A hash type converter. </summary>
    /// <remarks> 19.09.2020. </remarks>
    /// <seealso cref="ITypeConverter"/>
    internal class HashTypeConverter : JsonConverter<Hash>, ITypeConverter
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        /// <seealso cref="SubstrateNetApi.ITypeConverter.TypeName"/>
        public string TypeName { get; } = "T::Hash";

        /// <summary> Creates a new object. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> An object. </returns>
        /// <seealso cref="ITypeConverter.Create(string)"/>
        public object Create(string value)
        {
            Logger.Debug($"Converting {value} to Hash.");

            if (value == null)
            {
                return null;
            }

            return new Hash(value);
        }

        public override Hash ReadJson(JsonReader reader, Type objectType, Hash existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new Hash((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, Hash value, JsonSerializer serializer)
        {
            writer.WriteValue(value.HexString);
        }
    }
}
