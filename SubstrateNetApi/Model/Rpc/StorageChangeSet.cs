using Newtonsoft.Json;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.TypeConverters;

namespace SubstrateNetApi.Model.Rpc
{
    public class StorageChangeSet
    {
        [JsonConverter(typeof(GenericTypeConverter<Hash>))]
        public Hash Block { get; set; }

        public string[][] Changes { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
