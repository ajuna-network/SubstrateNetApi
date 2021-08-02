using System;
using System.Text;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Meta;

namespace SubstrateNetApiTests
{
    public class HashExtensionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void XXHash128()
        {
            var bytes1 = Encoding.ASCII.GetBytes("Sudo");
            var hashBytes1 = HashExtension.Twox128(bytes1);
            Assert.AreEqual("5C0D1176A568C1F92944340DBFED9E9C", BitConverter.ToString(hashBytes1).Replace("-", ""));

            var bytes2 = Encoding.ASCII.GetBytes("Key");
            var hashBytes2 = HashExtension.Twox128(bytes2);
            Assert.AreEqual("530EBCA703C85910E7164CB7D1C9E47B", BitConverter.ToString(hashBytes2).Replace("-", ""));
        }

        [Test]
        public void BlakeTwo128Test()
        {
            var bytes = Utils.HexToByteArray("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");
            var hashBytes = HashExtension.Hash(Storage.Hasher.BlakeTwo128, bytes);
            Assert.AreEqual(
                "DE1E86A9A8C739864CF3CC5EC2BEA59F", 
                BitConverter.ToString(hashBytes).Replace("-", ""));
        }

        [Test]
        public void BlakeTwo256Test()
        {
            var bytes = Utils.HexToByteArray("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");
            var hashBytes = HashExtension.Hash(Storage.Hasher.BlakeTwo256, bytes);
            Assert.AreEqual(
                "2E3FB4C297A84C5CEBC0E78257D213D0927CCC7596044C6BA013DD05522AACBA", 
                BitConverter.ToString(hashBytes).Replace("-", ""));
        }

        [Test]
        public void BlakeTwo128ConcatTest()
        {
            var bytes = Utils.HexToByteArray("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");
            var hashBytes = HashExtension.Hash(Storage.Hasher.BlakeTwo128Concat, bytes);
            Assert.AreEqual(
                "DE1E86A9A8C739864CF3CC5EC2BEA59FD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D",
                BitConverter.ToString(hashBytes).Replace("-", ""));
        }

        [Test]
        public void Twox128Test()
        {
            var bytes = Utils.HexToByteArray("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");
            var hashBytes = HashExtension.Hash(Storage.Hasher.Twox128, bytes);
            Assert.AreEqual(
                "518366B5B1BC7C99BAE0BA710AF1AC66",
                BitConverter.ToString(hashBytes).Replace("-", ""));
        }

        [Test]
        public void Twox256Test()
        {
            var bytes = Utils.HexToByteArray("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");
            var hashBytes = HashExtension.Hash(Storage.Hasher.Twox256, bytes);
            Assert.AreEqual(
                "518366B5B1BC7C99BAE0BA710AF1AC66ECC0FD2F7C15BBE1EB86DBF45C7899E8",
                BitConverter.ToString(hashBytes).Replace("-", ""));
        }

        [Test]
        public void Twox64ConcatTest()
        {
            var bytes = Utils.HexToByteArray("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");
            var hashBytes = HashExtension.Hash(Storage.Hasher.Twox64Concat, bytes);
            Assert.AreEqual(
                "518366B5B1BC7C99D43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D",
                BitConverter.ToString(hashBytes).Replace("-", ""));
        }
    }
}