using SubstrateNetApi.Model.Types;

namespace SubstrateNetWallet
{
    public class WalletFile
    {
        public WalletFile(KeyType keyType, byte[] publicKey, byte[] encryptedSeed, byte[] salt)
        {
            KeyType = keyType;
            PublicKey = publicKey;
            EncryptedSeed = encryptedSeed;
            Salt = salt;
        }

        public KeyType KeyType { get; }

        public byte[] PublicKey { get; }

        public byte[] EncryptedSeed { get; }

        public byte[] Salt { get; }
    }
}