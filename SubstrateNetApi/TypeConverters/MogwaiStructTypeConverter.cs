/// <file> SubstrateNetApi\TypeConverters\MogwaiStructTypeConverter.cs </file>
/// <copyright file="MogwaiStructTypeConverter.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the mogwai structure type converter class. </summary>
using NLog;
using SubstrateNetApi.MetaDataModel.Values;

namespace SubstrateNetApi.TypeConverters
{
    /// <summary> A mogwai structure type converter. </summary>
    /// <remarks> 19.09.2020. </remarks>
    /// <seealso cref="ITypeConverter"/>
    public class MogwaiStructTypeConverter : ITypeConverter
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        /// <seealso cref="SubstrateNetApi.ITypeConverter.TypeName"/>
        public string TypeName { get; } = "MogwaiStruct<T::Hash, T::BlockNumber, BalanceOf<T>>";

        /// <summary> Creates a new object. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> An object. </returns>
        /// <seealso cref="ITypeConverter.Create(string)"/>
        public object Create(string value)
        {
            Logger.Debug($"Converting {value} to MogwaiStruct.");
            return new MogwaiStruct(value);
        }
    }
}
