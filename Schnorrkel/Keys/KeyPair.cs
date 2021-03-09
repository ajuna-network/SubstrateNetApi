namespace Schnorrkel.Keys
{
    public class KeyPair
    {
        public PublicKey Public { get; set; }
        public SecretKey Secret { get; set; }

        public KeyPair(PublicKey publicKey, SecretKey secretKey)
        {
            this.Public = publicKey;
            this.Secret = secretKey;
        }
    }
}
