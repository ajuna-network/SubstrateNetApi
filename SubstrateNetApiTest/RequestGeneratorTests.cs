using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Meta;

namespace SubstrateNetApiTests
{
    public class RequestGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetStorageTest()
        {
            var result = RequestGenerator.GetStorage("Sudo", "Key", Storage.Type.Plain);
            Assert.AreEqual("0x5C0D1176A568C1F92944340DBFED9E9C530EBCA703C85910E7164CB7D1C9E47B", result);
        }
    }
}