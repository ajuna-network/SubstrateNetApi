using System;
using System.Text;
using NUnit.Framework;

namespace SubstrateNetApi
{
    public class HashExtensionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void XXHash128()
        {
            var bytes1 = Encoding.ASCII.GetBytes("Sudo");
            var hashBytes1 = HashExtension.XXHash128(bytes1);
            Assert.AreEqual("5C0D1176A568C1F92944340DBFED9E9C", BitConverter.ToString(hashBytes1).Replace("-", ""));

            var bytes2 = Encoding.ASCII.GetBytes("Key");
            var hashBytes2 = HashExtension.XXHash128(bytes2);
            Assert.AreEqual("530EBCA703C85910E7164CB7D1C9E47B", BitConverter.ToString(hashBytes2).Replace("-", ""));
        }

        [Test]
        public void Blake2Test()
        {
            var bytes = Utils.HexToByteArray("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");
            var hashBytes = HashExtension.Blake2(bytes, 128);
            Assert.AreEqual("DE1E86A9A8C739864CF3CC5EC2BEA59F", BitConverter.ToString(hashBytes).Replace("-", ""));
        }

        [Test]
        public void Blake2ConcatTest()
        {
            var bytes = Utils.HexToByteArray("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");
            var hashBytes = HashExtension.Blake2Concat(bytes);
            Assert.AreEqual("DE1E86A9A8C739864CF3CC5EC2BEA59FD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D", BitConverter.ToString(hashBytes).Replace("-", ""));
        }
    }
}