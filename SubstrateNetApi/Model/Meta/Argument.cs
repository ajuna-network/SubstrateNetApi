using Newtonsoft.Json;

namespace SubstrateNetApi.Model.Meta
{
    public class Argument
    {
        public string Name { get; set; }
        public string Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Value { get; set; }
    }
}