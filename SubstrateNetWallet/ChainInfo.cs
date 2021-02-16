using Newtonsoft.Json;
using SubstrateNetApi.Model.Types;
using System;
using SubstrateNetApi.Model.Rpc;

namespace SubstrateNetWallet
{
    public class ChainInfo : EventArgs
    {
        public string Name { get; private set; }
        public string Version { get; private set; }
        public string Chain { get; private set; }

        public RuntimeVersion RuntimeVersion { get; private set; }

        public ulong BlockNumber { get; private set; }

        public ChainInfo(string name, string version, string chain, RuntimeVersion runtime)
        {
            Name = name;
            Version = version;
            Chain = chain;
            RuntimeVersion = runtime;
        }

        internal void UpdateFinalizedHeader(Header header)
        {
            BlockNumber = header.Number.Value;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}