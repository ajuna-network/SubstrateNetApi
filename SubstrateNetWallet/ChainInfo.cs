using Newtonsoft.Json;
using SubstrateNetApi.MetaDataModel.Types;
using System;

namespace SubstrateNetWallet
{
    public class ChainInfo : EventArgs
    {
        public string Name { get; private set; }
        public string Version { get; private set; }
        public string Chain { get; private set; }

        public uint BlockNumber { get; private set; }

        public ChainInfo(string name, string version, string chain)
        {
            Name = name;
            Version = version;
            Chain = chain;
        }

        internal void UpdateFinalizedHeader(Header header)
        {
            BlockNumber = header.Number;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}