using NLog;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Exceptions;
using SubstrateNetApi.TypeConverters;
using System;
using System.Threading.Tasks;

namespace SubstrateNetApiTests.ClientTests
{
    class ClientTests
    {
        private const string WebSocketUrl = "wss://boot.worldofmogwais.com";

        private SubstrateClient _substrateClient;

        [SetUp]
        public void Setup()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);

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
        public async Task MultipleConnectionsTestAsync()
        {
            await _substrateClient.ConnectAsync();
            Assert.IsTrue(_substrateClient.IsConnected);
            await _substrateClient.CloseAsync();
            Assert.IsFalse(_substrateClient.IsConnected);
            await _substrateClient.ConnectAsync();
            Assert.IsTrue(_substrateClient.IsConnected);
            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task ConnectWhileConnectedTestAsync()
        {
            await _substrateClient.ConnectAsync();
            Assert.IsTrue(_substrateClient.IsConnected);
            await _substrateClient.ConnectAsync();
            Assert.IsTrue(_substrateClient.IsConnected);
            await _substrateClient.CloseAsync();
            Assert.IsFalse(_substrateClient.IsConnected);
        }

        [Test]
        public void MultipleConverterTest()
        {
            _substrateClient.RegisterTypeConverter(new MogwaiStructTypeConverter());
            Assert.Throws<ConverterAlreadyRegisteredException>(() => _substrateClient.RegisterTypeConverter(new MogwaiStructTypeConverter()));
        }

        [Test]
        public async Task GetMethodTestAsync()
        {
            await _substrateClient.ConnectAsync();

            var result = await _substrateClient.GetMethodAsync<string>("system_name");
            Assert.AreEqual("Substrate Node", result);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public void GetMethodNotConnectedTest()
        {
            Assert.ThrowsAsync<ClientNotConnectedException>(async () =>
                await _substrateClient.GetMethodAsync<string>("system_name"));
        }
    }
}
