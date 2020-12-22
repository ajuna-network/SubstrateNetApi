/// <file> SubstrateNetApi\TypeConverters\AccountInfoConverter.cs </file>
/// <copyright file="AccountInfoConverter.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the account information converter class. </summary>
using Newtonsoft.Json;
using NLog;
using SubstrateNetApi.MetaDataModel.Values;
using System;

namespace SubstrateNetApi.TypeConverters
{
    /// <summary> An account information converter. </summary>
    /// <remarks> 19.09.2020. </remarks>
    /// <seealso cref="ITypeConverter"/>
    internal class AccountInfoTypeConverter : JsonConverter<AccountInfo>, ITypeConverter
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        /// <seealso cref="SubstrateNetApi.ITypeConverter.TypeName"/>
        public string TypeName { get; } = "AccountInfo<T::Index, T::AccountData>";

        /// <summary> Creates a new object. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> An object. </returns>
        public object Create(string value)
        {
            Logger.Debug($"Converting '{value}' to AccountInfo.");
            return value != null ? new AccountInfo(value) : null;
        }

        public override AccountInfo ReadJson(JsonReader reader, Type objectType, AccountInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, AccountInfo value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}