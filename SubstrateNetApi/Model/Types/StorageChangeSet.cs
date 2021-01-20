using Newtonsoft.Json;
using SubstrateNetApi.TypeConverters;

namespace SubstrateNetApi.Model.Types
{
    public class StorageChangeSet
    {
        [JsonConverter(typeof(HashTypeConverter))]
        public Hash Block { get; set; }

        public string[][] Changes { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
