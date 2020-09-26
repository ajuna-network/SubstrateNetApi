using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SubstrateNetApi.MetaDataModel
{
    public class MetaData
    {
        public MetaData(string origin = "unknown")
        {
            this.Origin = origin;
        }
        public string Origin { get; set; }
        public string Magic { get; set; }
        public string Version { get; set; }
        public Module[] Modules { get; set; }
        public string[] ExtrinsicExtensions { get; set; }

        public bool TryGetModuleByName(string name, out Module result)
        {
            result = null;
            foreach (Module module in Modules)
            {
                if (module.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = module;
                    return true;
                }
            }

            return false;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, new StringEnumConverter());
        }


    }
}