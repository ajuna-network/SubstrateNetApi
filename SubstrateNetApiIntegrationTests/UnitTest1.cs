using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using SubstrateNetApi;
using System;
using System.Threading.Tasks;

namespace SubstrateNetApiIntegrationTests
{
    public class Tests
    {
        private const string WebSocketUrl = "ws://127.0.0.1:9944";

        private SubstrateClient _substrateClient;

        [SetUp]
        public void Setup()
        {
            var config = new LoggingConfiguration();

            // Targets where to log to: File and Console
            var console = new ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, console);

            // Apply config           
            LogManager.Configuration = config;

            _substrateClient = new SubstrateClient(new Uri(WebSocketUrl));
        }

        [TearDown]
        public void TearDown()
        {
            _substrateClient.Dispose();
        }

        [Test]
        public async Task GetMethodChainNameTestAsync()
        {
            await _substrateClient.ConnectAsync();

            var result = await _substrateClient.GetMethodAsync<string>("system_chain");
            Assert.AreEqual("Development", result);

            await _substrateClient.CloseAsync();
        }
    }
}