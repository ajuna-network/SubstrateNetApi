using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SubstrateNetApi.TypeConverters;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class StorageChangeSet
    {
        [JsonConverter(typeof(HashTypeConverter))]
        public Hash Block { get; set; }

        //[JsonConverter(typeof(JArray))]
        public string[][] Changes { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
