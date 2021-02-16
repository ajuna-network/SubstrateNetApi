using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Extrinsics;
using SubstrateNetApi.Model.Types;
using System;
using System.Collections.Generic;

namespace SubstrateNetApiTests.Extrinsic
{
    public class PayloadTest
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
        public void EncodeExtraTest()
        {
            var genesisHash = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var blockHash = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var era = new Era(2048, 99, false);

            var paramsList = new List<byte>();
            paramsList.Add(0xFF);
            paramsList.AddRange(Utils.HexToByteArray("d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d"));  // Utils.GetPublicKeyFrom("5FfBQ3kwXrbdyoqLPvcXRp7ikWydXawpNs2Ceu3WwFdhZ8W4");          
            CompactInteger amount = 100;
            paramsList.AddRange(amount.Encode());
            byte[] parameters = paramsList.ToArray();

            Method method = new Method(0x06, 0x00, parameters);
            byte[] methodBytes = Utils.StringValueArrayBytesArray("6, 0, 255, 212, 53, 147, 199, 21, 253, 211, 28, 97, 20, 26, 189, 4, 169, 159, 214, 130, 44, 133, 88, 133, 76, 205, 227, 154, 86, 132, 231, 165, 109, 162, 125, 145, 1");

            Assert.AreEqual(methodBytes, method.Encode());

            var genesis = new Hash();
            genesis.Create(genesisHash);

            var startEra = new Hash();
            startEra.Create(blockHash);

            SignedExtensions signedExtensions = new SignedExtensions(259, 1, genesis, startEra, era, 0, 0);

            var payload = new Payload(method, signedExtensions);

            byte[] payloadBytes = Utils.StringValueArrayBytesArray("6, 0, 255, 212, 53, 147, 199, 21, 253, 211, 28, 97, 20, 26, 189, 4, 169, 159, 214, 130, 44, 133, 88, 133, 76, 205, 227, 154, 86, 132, 231, 165, 109, 162, 125, 145, 1, 58, 6, 0, 0, 3, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0");

            Assert.AreEqual(payloadBytes, payload.Encode());
        }

    }
}
