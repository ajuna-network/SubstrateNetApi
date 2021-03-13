using System;
using System.Text;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.Model.Types.Base;
using NLog.Config;
using NLog;
using System.Threading.Tasks;
using SubstrateNetApi.Exceptions;
using NLog.Targets;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Extrinsics;

namespace SubstrateNetApiTests
{
    public class CustomStorageTests
    {
        private const string WebSocketUrl = "wss://mogiway-01.dotmog.com";

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
        public async Task BasicTestAsync()
        {
            await _substrateClient.ConnectAsync();

            var reqResult = await _substrateClient.GetStorageAsync("Sudo", "Key");
            Assert.AreEqual("AccountId", reqResult.GetType().Name);
            Assert.IsTrue(reqResult is AccountId);

            var accountId = (AccountId)reqResult;
            Assert.AreEqual("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH", accountId.Value);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task ParameterizedTestAsync()
        {
            await _substrateClient.ConnectAsync();

            var countMogwais = await _substrateClient.GetStorageAsync("DotMogModule", "AllMogwaisCount");
            Assert.AreEqual("U64", countMogwais.GetType().Name);

            var request = await _substrateClient.GetStorageAsync("DotMogModule", "AllMogwaisArray", new[] { "0" });
            Assert.AreEqual("Hash", request.GetType().Name);
            Assert.IsTrue(request is Hash);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task MissingParameterTestAsync()
        {
            await _substrateClient.ConnectAsync();

            Assert.ThrowsAsync<MissingParameterException>(async () =>
                await _substrateClient.GetStorageAsync("DotMogModule", "AllMogwaisArray"));

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task InvalidStorageNameTestAsync()
        {
            await _substrateClient.ConnectAsync();

            Assert.ThrowsAsync<MissingModuleOrItemException>(async () =>
                await _substrateClient.GetStorageAsync("Invalid", "Name"));

            await _substrateClient.CloseAsync();
        }

        [Test]
        public void InvalidConnectionStateTest()
        {
            Assert.ThrowsAsync<ClientNotConnectedException>(async () =>
                await _substrateClient.GetStorageAsync("Invalid", "Name"));
        }

        [Test]
        public async Task MissingTypeConverterTestAsync()
        {
            await _substrateClient.ConnectAsync();

            Assert.ThrowsAsync<MissingConverterException>(async () =>
                await _substrateClient.GetStorageAsync("DotMogModule", "Mogwais", new[]
                {
                    "0xAD35415CB5B574819C8521B9192FFFDA772C0770FED9A55494293B2D728F104C"
                }));

            await _substrateClient.CloseAsync();
        }
    }
}