using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubstrateNetApiTests.Extrinsic
{
    public class ExtrinsicTests
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
        public void MethodSerializeTest()
        {
            Assert.True("0x0c040602".Equals(Utils.Bytes2HexString(new Method(0x06, 0x02).Serialize()), StringComparison.InvariantCultureIgnoreCase));

            Assert.True("0x2c0406000100000000000000".Equals(Utils.Bytes2HexString(new Method(0x06, 0x00, Utils.HexToByteArray("0x0100000000000000")).Serialize()), StringComparison.InvariantCultureIgnoreCase));

            var parameters = new List<byte>();
            parameters.Add(0xFF);
            parameters.AddRange(Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"));
            Assert.AreEqual("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d".ToUpper(), Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY")).ToUpper());
            parameters.AddRange(new CompactInteger(100).Encode());
            var balanceTransfer = new Method(0x04, 0x00, parameters.ToArray());
            Assert.True("0x98040400ffd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d9101".Equals(Utils.Bytes2HexString(balanceTransfer.Serialize()), StringComparison.InvariantCultureIgnoreCase));
        }

    }
}