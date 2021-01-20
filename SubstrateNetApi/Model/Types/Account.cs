using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.Model.Types
{
    public enum KeyType
    {
        ED25519, SR25519
    }

    public class Account : AccountId
    {
        public KeyType KeyType;

        [JsonIgnore]
        public byte KeyTypeByte
        {
            get
            {
                switch (KeyType)
                {
                    case KeyType.ED25519:
                        return 0;
                    case KeyType.SR25519:
                        return 1;
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

        public Account(KeyType keyType, byte[] publicKey) : base(publicKey)
        {
            KeyType = keyType;
            PrivateKey = null;

        }
    }
}
