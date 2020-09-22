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

        public byte KeyTypeByte
        {
            get
            {
                switch (KeyType)
                {
                    case KeyType.SR25519:
                        return (byte)1;
                    case KeyType.ED25519:
                        return (byte) 0;
                    default:
                        throw new Exception($"Unknown key type found '{KeyType}'.");
                }
            }
        }

        [JsonIgnore]
        public byte[] PrivateKey { get; private set; }

        public Account(KeyType keyType, byte[] privateKey, byte[] publicKey) : base(publicKey)
        {
            KeyType = keyType;
            PrivateKey = privateKey;

        }


    }
}
