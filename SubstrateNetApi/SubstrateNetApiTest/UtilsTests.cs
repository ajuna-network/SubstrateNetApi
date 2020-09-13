using System;
using System.Numerics;
using NUnit.Framework;
using SubstrateNetApi;

namespace SubstrateNetApiTests
{
    public class UtilsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetPublicKeyFromTest()
        {
            var address = "5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY";
            var bytes = Utils.GetPublicKeyFrom(address);
            Assert.AreEqual("D43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D", BitConverter.ToString(bytes).Replace("-", ""));
        }

        [Test]
        public void GetAddressFromTest()
        {
            var publicKeyString = "0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d";
            var publickey = Utils.HexToByteArray(publicKeyString);
            var address = Utils.GetAddressFrom(publickey);
            Assert.AreEqual("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY", address);
        }
    }
}