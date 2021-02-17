using System;
using Newtonsoft.Json;
using SubstrateNetApi.Model.Rpc;

namespace SubstrateNetWallet
{
    public class ChainInfo : EventArgs
    {
        public ChainInfo(string name, string version, string chain, RuntimeVersion runtime)
        {
            Name = name;
            Version = version;
            Chain = chain;
            RuntimeVersion = runtime;
        }

        public string Name { get; }
        public string Version { get; }
        public string Chain { get; }

        public RuntimeVersion RuntimeVersion { get; }

        public ulong BlockNumber { get; private set; }

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