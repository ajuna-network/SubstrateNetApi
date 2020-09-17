using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Linq;

namespace SubstrateNetApiTests.Ed25519
{
    public class Ed25519Tests
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
        public void Ed25519KeyPairTest()
        {
            string priKey0x = "0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e";
            string pubKey0x = "0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e";

            byte[] priKey = Utils.HexToByteArray(priKey0x);
            byte[] pubKey = Utils.HexToByteArray(pubKey0x);
            byte[] seed = priKey.Take(32).ToArray();

            Chaos.NaCl.Ed25519.KeyPairFromSeed(out pubKey, out priKey, seed);

            Assert.AreEqual("0xF5E5767CF153319517630F226876B86C8160CC583BC013744C6BF255F5CC0EE5", Utils.Bytes2HexString(seed));
            Assert.AreEqual("0xF5E5767CF153319517630F226876B86C8160CC583BC013744C6BF255F5CC0EE5278117FC144C72340F67D0F2316E8386CEFFBF2B2428C9C51FEF7C597F1D426E", Utils.Bytes2HexString(priKey));
            Assert.AreEqual("0x278117FC144C72340F67D0F2316E8386CEFFBF2B2428C9C51FEF7C597F1D426E", Utils.Bytes2HexString(pubKey));

            var account = new AccountId("0x278117FC144C72340F67D0F2316E8386CEFFBF2B2428C9C51FEF7C597F1D426E");
            Assert.AreEqual("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM", account.Address);
        }

        [Test]
        public void Ed25519KeyPairTest2()
        {
            // ZURICH
            var seed = "0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5";
        }

        [Test]
        public void Ed25519SignatureTest()
        {
            string priKey0x = "0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e";
            string pubKey0x = "0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e";

            byte[] priKey = Utils.HexToByteArray(priKey0x);
            byte[] pubKey = Utils.HexToByteArray(pubKey0x);

            int messageLength = _random.Next(10, 200);
            var message = new byte[messageLength];
            _random.NextBytes(message);

            //var message = signaturePayloadBytes.AsMemory().Slice(0, (int)payloadLength).ToArray();
            var simpleSign = Chaos.NaCl.Ed25519.Sign(message, priKey);

            Assert.True(Chaos.NaCl.Ed25519.Verify(simpleSign, message, pubKey));
        }
    }
}