using Newtonsoft.Json;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel.Extrinsics
{
    public class Method
    {
        public string ModuleName;

        public byte ModuleIndex;

        public string CallName;

        public byte CallIndex;

        public byte[] Parameters;

        public Method(byte moduleIndex, byte callIndex, byte[] parameters)
        {
            ModuleIndex = moduleIndex;
            CallIndex = callIndex;
            Parameters = parameters ?? new byte[0];
        }

        public Method(byte moduleIndex, byte callIndex)
        {
            ModuleIndex = moduleIndex;
            CallIndex = callIndex;
            Parameters = new byte[0];
        }

        public Method(Module module, Call call, byte[] parameters)
        {
            ModuleName = module.Name;
            ModuleIndex = module.Index;
            CallName = call.Name;
            CallIndex = module.IndexOf(call);
            Parameters = parameters;
        }

        public byte[] Encode()
        {
            var result = new List<byte>();
            result.Add(ModuleIndex);
            result.Add(CallIndex);
            result.AddRange(Parameters);
            return result.ToArray();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
