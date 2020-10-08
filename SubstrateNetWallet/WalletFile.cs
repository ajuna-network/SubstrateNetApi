namespace SubstrateNetWallet
{
    public class WalletFile
    {
        public byte[] encryptedSeed;

        public byte[] salt;

        public WalletFile(byte[] encryptedSeed, byte[] salt)
        {
            this.encryptedSeed = encryptedSeed;
            this.salt = salt;
        }
    }
}
