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
            var module = new Module {Name = "Sudo"};
            var item = new Item {Name = "Key"};
            var result = RequestGenerator.GetStorage(module, item);
            Assert.AreEqual("0x5C0D1176A568C1F92944340DBFED9E9C530EBCA703C85910E7164CB7D1C9E47B", result);
        }
    }
}