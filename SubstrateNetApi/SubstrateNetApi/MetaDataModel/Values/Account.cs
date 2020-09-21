using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public enum KeyType
    {
        SR25519, ED25519
    }

    public class Account : AccountId
    {
        public KeyType KeyType;

        [JsonIgnore]
        private byte[] _privateKey;

        public Account(KeyType keyType, byte[] privateKey, byte[] publicKey) : base(publicKey)
        {
            KeyType = keyType;
            _privateKey = privateKey;

        }


    }
}
