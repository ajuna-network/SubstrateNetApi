using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SubstrateNetApi
{
    public class AccountId
    {
        public string Address { get; }

        public byte[] PublicKey { get; }

        public AccountId(string resultString)
        {
            PublicKey = Utils.HexToByteArray(resultString);
            Address = Utils.GetAddressFrom(PublicKey);
        }

        override
        public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}