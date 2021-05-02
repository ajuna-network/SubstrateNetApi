using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApiTests
{
    public class SubscriptionTest
    {
        private const string WebSocketUrl = "wss://rpc.polkadot.io";

        private SubstrateClient _substrateClient;

        [OneTimeSetUp]
        public void Setup()
        {
            var config = new LoggingConfiguration();

            // Targets where to log to: File and Console
            var logconsole = new ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);

            // Apply config           
            LogManager.Configuration = config;

            _substrateClient = new SubstrateClient(new Uri(WebSocketUrl));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _substrateClient.Dispose();
        }

        [Test]
        public async Task BasicSubscriptionTestAsync()
        {
            await _substrateClient.ConnectAsync();

            Header currentHeader = null;

            Action<string, Header> callAllHeads = (subscriptionId, eventObject) =>
            {
                currentHeader = eventObject;
            };

            await _substrateClient.Chain.SubscribeAllHeadsAsync(callAllHeads, CancellationToken.None);

            Thread.Sleep(1000);

            var value1 = currentHeader.Number.Value;

            Assert.IsTrue(value1 > 1);

            Thread.Sleep(10000);

            Assert.IsTrue(value1 < currentHeader.Number.Value);

            await _substrateClient.CloseAsync();
        }
    }
}