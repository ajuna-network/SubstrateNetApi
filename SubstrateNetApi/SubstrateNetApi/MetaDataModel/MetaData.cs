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

        /// <summary>
        /// Get index of module in the modules array, currently modules with out calls don't count to the index.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool TryGetIndexOfCallModules(Module module, out byte index)
        {
            index = 0;
            foreach(var m in Modules)
            {
                if (m == module )
                {
                    return true;
                }

                if (m.Calls != null)
                {
                    index++;
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