using Newtonsoft.Json;

namespace SubstrateNetApi.MetaDataModel
{
    public class Argument
    {
        public string Name { get; internal set; }
        public string Type { get; internal set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Value { get; internal set; }
    }
}