using System;
using NUnit.Framework;
using SubstrateNetApi;

namespace Schnorrkel
{
    public class SchnorrkelTests
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
        public void ShouldGenerateKeypair11Test()
        {
            var seed0x = "0xa81056d713af1ff17b599e60d287952e89301b5208324a0529b62dc7369c745defc9c8dd67b7c59b201bc164163a8978d40010c22743db142a47f2e064480d4b";

            byte[] seed = Utils.HexToByteArray(seed0x);

            var secretKey = SecretKey.FromBytes011(seed);

            Assert.AreEqual("0x15C2EA7AE2F5237E2FCB134CFAB0D2251166430A4146A920C5B6E5D88693AE0B", Utils.Bytes2HexString(secretKey.key.GetBytes()));
            Assert.AreEqual("0xEFC9C8DD67B7C59B201BC164163A8978D40010C22743DB142A47F2E064480D4B", Utils.Bytes2HexString(secretKey.nonce));
        }


        [Test]
        public void ShouldGenerateKeypair85Test()
        {
            var seed0x = "0xa81056d713af1ff17b599e60d287952e89301b5208324a0529b62dc7369c745defc9c8dd67b7c59b201bc164163a8978d40010c22743db142a47f2e064480d4b";

            byte[] seed = Utils.HexToByteArray(seed0x);

            var secretKey = SecretKey.FromBytes085(seed);
            var pubkey1 = Utils.Bytes2HexString(secretKey.nonce);

            Assert.AreEqual("0xA81056D713AF1FF17B599E60D287952E89301B5208324A0529B62DC7369C745D", Utils.Bytes2HexString(secretKey.key.GetBytes()));
            Assert.AreEqual("0xEFC9C8DD67B7C59B201BC164163A8978D40010C22743DB142A47F2E064480D4B", Utils.Bytes2HexString(secretKey.nonce));
        }

        [Test]
        public void SignatureVerify11Test()
        {
            var pubKey0x = "0xd678b3e00c4238888bbf08dbbe1d7de77c3f1ca1fc71a5a283770f06f7cd1205";
            var secKey0x = "0xa81056d713af1ff17b599e60d287952e89301b5208324a0529b62dc7369c745defc9c8dd67b7c59b201bc164163a8978d40010c22743db142a47f2e064480d4b";

            var pubKey = Utils.HexToByteArray(pubKey0x);
            var secKey = Utils.HexToByteArray(secKey0x);

            var messageLength = _random.Next(10, 200);
            var message = new byte[messageLength];
            _random.NextBytes(message);

            var simpleSign = Sr25519v011.SignSimple(pubKey, secKey, message);

            Assert.True(Sr25519v011.Verify(simpleSign, pubKey, message));
        }

        [Test]
        public void SignatureVerify91Test()
        {
            string pubKey0x = "0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d";
            string secKey0x = "0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011";

            var pubKey = Utils.HexToByteArray(pubKey0x);
            var secKey = Utils.HexToByteArray(secKey0x);

            var messageLength = _random.Next(10, 200);
            var message = new byte[messageLength];
            _random.NextBytes(message);

            var simpleSign = Sr25519v091.SignSimple(pubKey, secKey, message);

            Assert.True(Sr25519v091.Verify(simpleSign, pubKey, message));
        }

        [Test]
        public void SignatureVerifySignedOnNodeByAlice()
        {
            var publicKey = Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"); // Alice

            var messag1 = Utils.HexToByteArray("0xA81056D713AF1FF17B599E60D287952E89301B5208324A0529B62DC7369C745D");
            var signedByAlic1 =
                Utils.HexToByteArray(
                    "0x2afb94d9eaf26b7191790b60bbc23c6b6fdbc09991514ff6945af1e6fa972b29ef3e5d819068b266113081d78f0d6d3271a339e6c0acc409cab45f4201146180");
            Assert.True(Sr25519v091.Verify(signedByAlic1, publicKey, messag1));

            var messag2 =
                Utils.HexToByteArray(
                    "0x0400FF8EAF04151687736326C9FEA17E25FC5287613693C912909CB226AA4794F26A484913DC4F62090B18B6893C1431369461069EE3E9C1DA7F9F9A8C097C0CEBBEAC2BB9");
            var signedByAlic2 =
                Utils.HexToByteArray(
                    "0xa61e9de53de6e4af819e9e75a6c6495f3620fe7ffed386708584395e6787e32e7b209860a190247b64c38201a12e16c1e8cbdd2fb9b0723bd9e88e32d3763689");
            Assert.True(Sr25519v091.Verify(signedByAlic2, publicKey, messag2));

            var signedByAlic3 =
                Utils.HexToByteArray(
                    "0xa63c26a956af0218f800f0f0cf119f56139c824967b4e9f84ee3aa1d0c206a3eff7572a036568f89d8406df259c2bb93875725d284fb76187c8bb5fe0246cb8f");
            Assert.True(Sr25519v091.Verify(signedByAlic3, publicKey, messag2));
        }

        [Test]
        public void SignatureVerifyOnNodeSignedHereByAlice()
        {
            // https://polkadot.js.org/apps/#/signing/verify

            Assert.True(true);
        }
    }
}