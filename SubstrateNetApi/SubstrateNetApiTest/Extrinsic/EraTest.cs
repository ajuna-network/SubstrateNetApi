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
        public void EraEncodeDecodeTest()
        {
            var era1 = Era.Decode(new byte[] { 58, 6});
            Assert.AreEqual(2048, era1.Period);
            Assert.AreEqual(99, era1.Phase);
            Assert.AreEqual(new byte[] { 58, 6 }, era1.Encode());

            var era2 = Era.Decode(Utils.HexToByteArray("0x4503"));
            Assert.AreEqual(64, era2.Period);
            Assert.AreEqual(52, era2.Phase);
            Assert.AreEqual(new byte[] { 69, 3 }, era2.Encode());

            var era3 = Era.Decode(Utils.HexToByteArray("0xF502"));
            Assert.AreEqual(64, era3.Period);
            Assert.AreEqual(47, era3.Phase);
            Assert.AreEqual(new byte[] { 245, 2 }, era3.Encode());
        }

        [Test]
        public void EraBeginTest()
        {
            var era = new Era(64, 49, false);
            Assert.AreEqual(1585, era.EraStart(1587));
        }
    }
}