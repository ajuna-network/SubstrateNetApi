using Newtonsoft.Json;
using NLog;
using SubstrateNetApi.MetaDataModel.Extrinsic;
using SubstrateNetApi.MetaDataModel.Values;
using System;

namespace SubstrateNetApi.TypeConverters
{
    internal class ExtrinsicJsonConverter : JsonConverter<ExtrinsicModel>
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public override ExtrinsicModel ReadJson(JsonReader reader, Type objectType, ExtrinsicModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new ExtrinsicModel((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, ExtrinsicModel value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
