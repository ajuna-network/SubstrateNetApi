namespace SubstrateNetWallet
{
    public class WalletFile
    {
        public byte[] PublicKey { get; }

        public byte[] EncryptedSeed { get; }

        public byte[] Salt { get; }

        public WalletFile(byte[] publicKey, byte[] encryptedSeed, byte[] salt)
        {
            PublicKey = publicKey;
            EncryptedSeed = encryptedSeed;
            Salt = salt;
        }
    }
}
