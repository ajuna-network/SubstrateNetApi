using System;
using NUnit.Framework;

namespace SubstrateNetApi
{
    public class Tests
    {

        private const string WEBSOCKETURL = "wss://boot.worldofmogwais.com";

        private Client client;

        [SetUp]
        public void Setup()
        {

            client = new Client(new Uri(WEBSOCKETURL));
        }

        [Test]
        public void BasicTest()
        {
            client.ConnectAsync();
            var result = client.RequestAsync("state_getMetadata");

            var metaDataParser = new MetaDataParser(WEBSOCKETURL, result);
            var metaData = metaDataParser.MetaData;

            if (client.TryRequest(metaData, "Sudo", "Key", out object reqResult))
            {
                Console.WriteLine($"RESPONSE: {reqResult} [{reqResult.GetType().Name}]");
                Assert.AreEqual("AccountId", reqResult.GetType().Name);
                Assert.IsTrue(reqResult is AccountId);
                var accountId = (AccountId) reqResult;
                Assert.AreEqual("5GYZnHJ4dCtTDoQj4H5H9E727Ykv8NLWKtPAupEc3uJ89BGr", accountId.Address);
            }
            else
            {
                Assert.IsTrue(false);
            }

            client.Disconnect();
        }
    }
}