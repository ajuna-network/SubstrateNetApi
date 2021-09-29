using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SubstrateNetApi.Model.Meta
{
    public class MetaData
    {
        public MetaData(string origin = "unknown")
        {
            Origin = origin;
        }
        public string Origin { get; set; }
        public string Magic { get; set; }
        public string Version { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, new StringEnumConverter());
        }


    }
}