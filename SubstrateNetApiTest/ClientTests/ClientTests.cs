using System;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Exceptions;
using SubstrateNetApi.Model.Types.Custom;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.TypeConverters;

namespace SubstrateNetApiTests.ClientTests
{
    internal class ClientTests
    {
        private const string WebSocketUrl = "wss://rpc.polkadot.io";

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
            Assert.Throws<ConverterAlreadyRegisteredException>(() =>
                _substrateClient.RegisterTypeConverter(new GenericTypeConverter<AccountData>()));
        }

        [Test]
        public async Task GetMethodSystemNameTestAsync()
        {
            await _substrateClient.ConnectAsync();

            var result = await _substrateClient.GetMethodAsync<string>("system_name");
            Assert.AreEqual("Parity Polkadot", result);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task GetMethodChainNameTestAsync()
        {
            await _substrateClient.ConnectAsync();

            var result = await _substrateClient.GetMethodAsync<string>("system_chain");
            Assert.AreEqual("Polkadot", result);

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