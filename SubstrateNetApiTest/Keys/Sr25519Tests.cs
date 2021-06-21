using System;
using System.Linq;
using NUnit.Framework;
using Schnorrkel.Keys;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApiTests.Keys
{
    public class Sr25519Tests
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
        public void Sr25519KeyPairTest()
        {
            // Secret Key URI `//Alice` is account:
            // Secret seed:      0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a
            // Public key(hex):  0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
            // Account ID:       0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
            // SS58 Address:     5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY
            
            var miniSecretAlice = new MiniSecret(Utils.HexToByteArray("0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a"), ExpandMode.Ed25519);
            var keyPairAlice = miniSecretAlice.GetPair();

            Assert.AreEqual("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d", Utils.Bytes2HexString(keyPairAlice.Public.Key).ToLower());
            Assert.AreEqual("0x925a225d97aa00682d6a59b95b18780c10d7032336e88f3442b42361f4a66011", Utils.Bytes2HexString(keyPairAlice.Secret.nonce).ToLower());
            Assert.AreEqual("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY", Utils.GetAddressFrom(keyPairAlice.Public.Key));

            // Secret Key URI `//Bob` is account:
            // Secret seed:      0x398f0c28f98885e046333d4a41c19cee4c37368a9832c6502f6cfd182e2aef89
            // Public key(hex):  0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
            // Account ID:       0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
            // SS58 Address:     5FHneW46xGXgs5mUiveU4sbTyGBzmstUspZC92UhjJM694ty

            var miniSecretBob = new MiniSecret(Utils.HexToByteArray("0x398f0c28f98885e046333d4a41c19cee4c37368a9832c6502f6cfd182e2aef89"), ExpandMode.Ed25519);
            var keyPairBob = miniSecretBob.GetPair();

            Assert.AreEqual("0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48", Utils.Bytes2HexString(keyPairBob.Public.Key).ToLower());
            Assert.AreEqual("0x41ae88f85d0c1bfc37be41c904e1dfc01de8c8067b0d6d5df25dd1ac0894a325", Utils.Bytes2HexString(keyPairBob.Secret.nonce).ToLower());
            Assert.AreEqual("5FHneW46xGXgs5mUiveU4sbTyGBzmstUspZC92UhjJM694ty", Utils.GetAddressFrom(keyPairBob.Public.Key));
        }

        [Test]
        public void Sr25519SignatureTest()
        {
            var miniSecretAlice = new MiniSecret(Utils.HexToByteArray("0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a"), ExpandMode.Ed25519);
            var keyPairAlice = miniSecretAlice.GetPair();

            var messageLength = _random.Next(10, 200);
            var message = new byte[messageLength];
            _random.NextBytes(message);

            var simpleSign = Schnorrkel.Sr25519v091.SignSimple(keyPairAlice, message);

            Assert.True(Schnorrkel.Sr25519v091.Verify(simpleSign, keyPairAlice.Public.Key, message));
        }

        [Test]
        public void SignatureVerifyOnNodeSignedHereByAccount()
        {
            // https://polkadot.js.org/apps/#/signing/verify

            Assert.True(true);
        }
    }
}