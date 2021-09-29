using Newtonsoft.Json;
using SubstrateNetApi.Model.Meta;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Extrinsics
{
    public class Method
    {
        public string ModuleName;

        public byte ModuleIndex;

        public string CallName;

        public byte CallIndex;

        public byte[] Parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Method"/> class.
        /// </summary>
        /// <param name="moduleIndex">Index of the module.</param>
        /// <param name="callIndex">Index of the call.</param>
        /// <param name="parameters">The parameters.</param>
        public Method(byte moduleIndex, byte callIndex, byte[] parameters)
        {
            ModuleIndex = moduleIndex;
            CallIndex = callIndex;
            Parameters = parameters ?? new byte[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Method"/> class.
        /// </summary>
        /// <param name="moduleIndex">Index of the module.</param>
        /// <param name="callIndex">Index of the call.</param>
        public Method(byte moduleIndex, byte callIndex)
        {
            ModuleIndex = moduleIndex;
            CallIndex = callIndex;
            Parameters = new byte[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Method"/> class.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="call">The call.</param>
        /// <param name="parameters">The parameters.</param>
        public Method(byte moduleIndex, string moduleName, byte callIndex, string callName, byte[] parameters)
        {
            ModuleName = moduleName;
            ModuleIndex = moduleIndex;
            CallName = callName;
            CallIndex = callIndex;
            Parameters = parameters;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        /// <returns></returns>
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
