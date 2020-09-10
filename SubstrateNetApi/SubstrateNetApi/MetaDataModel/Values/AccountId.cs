using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class AccountId
    {
        public string Address { get; }

        public byte[] PublicKey { get; }

        public AccountId(string str): this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal AccountId(Memory<byte> memory)
        {
            PublicKey = memory.ToArray();
            Address = Utils.GetAddressFrom(PublicKey);
        }

        override
        public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}