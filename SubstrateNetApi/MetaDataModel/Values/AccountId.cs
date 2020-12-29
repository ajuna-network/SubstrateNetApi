using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class AccountId : IEncodable
    {
        public string Address { get; }

        public byte[] PublicKey { get; }

        public static AccountId CreateFromAddress(string address) => new AccountId(Utils.GetPublicKeyFrom(address));

        public AccountId(byte[] publicKey)
        {
            Address = Utils.GetAddressFrom(publicKey);
            PublicKey = publicKey;
        }

        public AccountId(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal AccountId(Memory<byte> memory)
        {
            PublicKey = memory.ToArray();
            Address = Utils.GetAddressFrom(PublicKey);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public byte[] Encode()
        {
            switch(Constants.ADDRESS_VERSION)
            {
                case 0:
                    return PublicKey;
                case 1:
                    var bytes = new List<byte>();
                    bytes.Add(0xFF);
                    bytes.AddRange(PublicKey);
                    return bytes.ToArray();
                default:
                    throw new NotImplementedException("Unknown address version please refere to Constants.cs");
            }
        }
    }
}