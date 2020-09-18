using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubstrateNetApiTests.Extrinsic
{
    public class SignedExtensionsTest
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
        public void SerializeExtraTest()
        {

            SignedExtensions signedExtensions = new SignedExtensions();
            var blockHash = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var era = new byte[] { 58, 6 };
            signedExtensions.SetMortality(era, blockHash);
            signedExtensions.SetNonce(new CompactInteger(0));
            signedExtensions.SetChargeTransactionPayment(new CompactInteger(0));


            byte[] bytes = Utils.StringValueArrayBytesArray("58, 6, 0, 0");


            Assert.AreEqual(bytes, signedExtensions.GetExtra());
        }

    }
}