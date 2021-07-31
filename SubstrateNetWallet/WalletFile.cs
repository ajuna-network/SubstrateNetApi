using SubstrateNetApi.Model.Types;

namespace SubstrateNetWallet
{
    /// <summary>
    /// Wallet File description.
    /// </summary>
    public class WalletFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WalletFile"/> class.
        /// </summary>
        /// <param name="keyType">Type of the key.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="encryptedSeed">The encrypted seed.</param>
        /// <param name="salt">The salt.</param>
        public WalletFile(KeyType keyType, byte[] publicKey, byte[] encryptedSeed, byte[] salt)
        {
            KeyType = keyType;
            PublicKey = publicKey;
            EncryptedSeed = encryptedSeed;
            Salt = salt;
        }

        /// <summary>
        /// Gets the type of the key.
        /// </summary>
        /// <value>
        /// The type of the key.
        /// </value>
        public KeyType KeyType { get; }

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        public byte[] PublicKey { get; }

        /// <summary>
        /// Gets the encrypted seed.
        /// </summary>
        /// <value>
        /// The encrypted seed.
        /// </value>
        public byte[] EncryptedSeed { get; }

        /// <summary>
        /// Gets the salt.
        /// </summary>
        /// <value>
        /// The salt.
        /// </value>
        public byte[] Salt { get; }
    }
}