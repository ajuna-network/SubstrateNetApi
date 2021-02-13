using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types
{
    public partial class AccountId : IEncodable
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



        //pub enum MultiAddress<AccountId, AccountIndex> {
        //    Id(AccountId),
        //    Index(AccountIndex),
        //    Raw(Vec<u8>),
        //    Address32([u8; 32]),
        //    Address20([u8; 20]),
        //}
        //Jaco
        //    BOT
        //gestern um 22:58 Uhr
        //    First entry in the enum, thus 00 preceding for Id
        //    Index would be, 01, etc.
        //    So just normal SCALE enum encoding.

        public byte[] Encode()
        {
            var bytes = new List<byte>();
            switch (Constants.AddressVersion)
            {
                case 0:
                    return PublicKey;
                case 1:
                    bytes.Add(0xFF);
                    bytes.AddRange(PublicKey);
                    return bytes.ToArray();
                case 2:
                    bytes.Add(0x00);
                    bytes.AddRange(PublicKey);
                    //bytes.AddRange(new byte[]{ 0x00, 0x00, 0x00, 0x00 });
                    return bytes.ToArray();
                default:
                    throw new NotImplementedException("Unknown address version please refere to Constants.cs");
            }
        }
    }
}