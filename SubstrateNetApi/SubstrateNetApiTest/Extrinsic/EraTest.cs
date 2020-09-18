using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel;
using SubstrateNetApi.MetaDataModel.Extrinsic;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubstrateNetApiTests.Extrinsic
{
    public class EraTest
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
        public void EraSerializeTest()
        {
            var era1 = Era.Deserialize(new byte[] { 58, 6});
            Assert.AreEqual(2048, era1.Period);
            Assert.AreEqual(99, era1.Phase);
            Assert.AreEqual(new byte[] { 58, 6 }, era1.Serialize());

            var era2 = Era.Deserialize(Utils.HexToByteArray("0x4503"));
            Assert.AreEqual(64, era2.Period);
            Assert.AreEqual(52, era2.Phase);
            Assert.AreEqual(new byte[] { 69, 3 }, era2.Serialize());
        }
    }
}