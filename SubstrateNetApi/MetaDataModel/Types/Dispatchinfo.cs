using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SubstrateNetApi.MetaDataModel.Types
{
    public partial class DispatchInfo
    {
        public ulong Weight { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Enums DispatchClass { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Pays Pays { get; set; }

        public DispatchInfo(ulong weight, Enums dispatchClass, Pays pays)
        {
            Weight = weight;
            DispatchClass = dispatchClass;
            Pays = pays;
        }
    }
}