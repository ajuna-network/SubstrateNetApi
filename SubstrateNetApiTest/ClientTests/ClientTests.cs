using NLog;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Exceptions;
using SubstrateNetApi.TypeConverters;
using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SubstrateNetApi.Model.Types;

namespace SubstrateNetApiTests.ClientTests
{
    class ClientTests
    {
        private const string WebSocketUrl = "wss://mogiway-01.dotmog.com";

        private SubstrateClient _substrateClient;

        [SetUp]
        public void Setup()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var console = new NLog.Targets.ConsoleTarget("logconsole");

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
            _substrateClient.RegisterTypeConverter(new GenericTypeConverter<MogwaiStruct>());
            Assert.Throws<ConverterAlreadyRegisteredException>(() => _substrateClient.RegisterTypeConverter(new GenericTypeConverter<MogwaiStruct>()));
        }

        [Test]
        public async Task GetMethodSystemNameTestAsync()
        {
            await _substrateClient.ConnectAsync();

            var result = await _substrateClient.GetMethodAsync<string>("system_name");
            Assert.AreEqual("DOTMog Node", result);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task GetMethodChainNameTestAsync()
        {
            await _substrateClient.ConnectAsync();

            var result = await _substrateClient.GetMethodAsync<string>("system_chain");
            Assert.AreEqual("DOTMog.com NET", result);

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
