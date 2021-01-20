using Newtonsoft.Json;
using NLog;
using SubstrateNetApi.Model.Extrinsics;
using System;

namespace SubstrateNetApi.TypeConverters
{
    internal class ExtrinsicJsonConverter : JsonConverter<Extrinsic>
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public override Extrinsic ReadJson(JsonReader reader, Type objectType, Extrinsic existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new Extrinsic((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, Extrinsic value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
