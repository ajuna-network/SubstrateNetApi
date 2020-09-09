using NUnit.Framework;
using Schnorrkel;
using SubstrateNetApi;
using System;

namespace SubstrateNetApiTests.Schnorrkel
{
    public class SchnorrkelTest
    {
        private Random _random;

        [OneTimeSetUp]
        public void Setup()
        {
            _random = new Random();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }

        [Test]
        public void SecretKeyPublicKeyTest()
        {
        }


        [Test]
        public void SignatureVerify11Test()
        {
            string pubKey0x = "0xd678b3e00c4238888bbf08dbbe1d7de77c3f1ca1fc71a5a283770f06f7cd1205";
            string secKey0x = "0xa81056d713af1ff17b599e60d287952e89301b5208324a0529b62dc7369c745defc9c8dd67b7c59b201bc164163a8978d40010c22743db142a47f2e064480d4b";
            
            byte[] pubKey = Utils.HexToByteArray(pubKey0x);
            byte[] secKey = Utils.HexToByteArray(secKey0x);

            int messageLength = _random.Next(10, 200);
            var message = new byte[messageLength];
            _random.NextBytes(message);

            //var message = signaturePayloadBytes.AsMemory().Slice(0, (int)payloadLength).ToArray();
            var simpleSign = Sr25519v011.SignSimple(pubKey, secKey, message);

            Assert.True(Sr25519v011.Verify(simpleSign, pubKey, message));
        }

        [Test]
        public void SignatureVerify91Test()
        {
            string pubKey0x = "0x586cc32614d6a3f219667db501ade545753058d43b14e6e971e9c9cc908ad843";
            string secKey0x = "0x19D51D513E4F0FFB854CC31A6949C348FA471DBFDA89E2B3B1E7B5B8E78357082288DCFE05240D96832CB642AE8C53CC1E6A08F62D49192F0FB61AC51A7D1977";

            byte[] pubKey = Utils.HexToByteArray(pubKey0x);
            byte[] secKey = Utils.HexToByteArray(secKey0x);

            int messageLength = _random.Next(10, 200);
            var message = new byte[messageLength];
            _random.NextBytes(message);

            var simpleSign = Sr25519v091.SignSimple(pubKey, secKey, message);

            Assert.True(Sr25519v091.Verify(simpleSign, pubKey, message));
        }
    }
}