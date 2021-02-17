using System;
using Newtonsoft.Json;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types
{
    public enum KeyType
    {
        Ed25519,
        Sr25519
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
                    case KeyType.Ed25519:
                        return 0;
                    case KeyType.Sr25519:
                        return 1;
                    default:
                        throw new Exception($"Unknown key type found '{KeyType}'.");
                }
            }
        }

        [JsonIgnore] public byte[] PrivateKey { get; private set; }

        public void Create(KeyType keyType, byte[] privateKey, byte[] publicKey)
        {
            KeyType = keyType;
            PrivateKey = privateKey;
            base.Create(publicKey);
        }

        public void Create(KeyType keyType, byte[] publicKey)
        {
            Create(keyType, null, publicKey);
        }

        public static Account Build(KeyType keyType, byte[] privateKey, byte[] publicKey)
        {
            var account = new Account();
            account.Create(keyType, privateKey, publicKey);
            return account;
        }
    }
}