using System;
using System.Text;
using NUnit.Framework;

namespace SubstrateNetApi
{
    public class RequestGeneratorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetStorageTest()
        {
            Module module = new Module() { Name = "Sudo"};
            Item item = new Item() { Name = "Key" };
            var result = RequestGenerator.GetStorage(module, item, null);
            Assert.AreEqual("5C0D1176A568C1F92944340DBFED9E9C530EBCA703C85910E7164CB7D1C9E47B", result);
        }
    }
}