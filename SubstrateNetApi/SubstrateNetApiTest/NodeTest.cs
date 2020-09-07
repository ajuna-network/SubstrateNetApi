using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SubstrateNetApi
{
    public class Tests
    {

        private const string WEBSOCKETURL = "wss://boot.worldofmogwais.com";

        private SubstrateClient substrateClient;

        [SetUp]
        public void Setup()
        {
            substrateClient = new SubstrateClient(new Uri(WEBSOCKETURL));
        }

        [Test]
        public async Task BasicTestAsync()
        {
            await substrateClient.ConnectAsync();

            var reqResult = await substrateClient.TryRequestAsync("Sudo", "Key");
            Assert.AreEqual("AccountId", reqResult.GetType().Name);
            Assert.IsTrue(reqResult is AccountId);

            var accountId = (AccountId) reqResult;
            Assert.AreEqual("5GYZnHJ4dCtTDoQj4H5H9E727Ykv8NLWKtPAupEc3uJ89BGr", accountId.Address);

            substrateClient.Disconnect();
        }
    }
}