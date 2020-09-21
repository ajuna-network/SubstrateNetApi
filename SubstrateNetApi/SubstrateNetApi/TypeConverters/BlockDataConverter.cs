using Newtonsoft.Json;
using NLog;
using SubstrateNetApi.MetaDataModel.Values;
using System;

namespace SubstrateNetApi.TypeConverters
{

    internal class HexTypeConverter : JsonConverter<ushort>, ITypeConverter
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        /// <seealso cref="SubstrateNetApi.ITypeConverter.TypeName"/>
        public string TypeName { get; } = "unknown";

        /// <summary> Creates a new object. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> An object. </returns>
        /// <seealso cref="ITypeConverter.Create(string)"/>
        public object Create(string value)
        {
            Logger.Debug($"Converting {value} to Number.");
            throw new NotImplementedException();
        }

        public override ushort ReadJson(JsonReader reader, Type objectType, ushort existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var hexNumber = (string)reader.Value;
            var bytes = Utils.HexToByteArray(hexNumber);

            return (ushort) Utils.Bytes2Value(bytes);
        }

        public override void WriteJson(JsonWriter writer, ushort value, JsonSerializer serializer)
        {
            writer.WriteValue(Utils.Bytes2HexString(Utils.Value2Bytes(value), Utils.HexStringFormat.PREFIXED));
        }
    }
}
