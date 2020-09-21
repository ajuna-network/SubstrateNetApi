/// <file> SubstrateNetApi\TypeConverters\AccountIdTypeConverter.cs </file>
/// <copyright file="AccountIdTypeConverter.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the account identifier type converter class. </summary>
using Newtonsoft.Json;
using NLog;
using SubstrateNetApi.MetaDataModel.Values;
using System;

namespace SubstrateNetApi.TypeConverters
{
    /// <summary> An account identifier type converter. </summary>
    /// <remarks> 19.09.2020. </remarks>
    /// <seealso cref="ITypeConverter"/>
    internal class AccountIdTypeConverter : JsonConverter<AccountId>, ITypeConverter
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        /// <seealso cref="SubstrateNetApi.ITypeConverter.TypeName"/>
        public string TypeName { get; } = "T::AccountId";

        /// <summary> Creates a new object. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> An object. </returns>
        /// <seealso cref="ITypeConverter.Create(string)"/>
        public object Create(string value)
        {
            Logger.Debug($"Converting {value} to AccountId.");
            return new AccountId(value);
        }

        public override AccountId ReadJson(JsonReader reader, Type objectType, AccountId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new AccountId((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, AccountId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.PublicKey);
        }
    }
}