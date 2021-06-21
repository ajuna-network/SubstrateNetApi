using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using Schnorrkel.Keys;
using SubstrateNetApi;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationTest
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

        [Test]
        public async Task NewGameTestAsync()
        {
            // Secret Key URI `//Alice` is account:
            // Secret seed:      0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a
            // Public key(hex):  0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
            // Account ID:       0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
            // SS58 Address:     5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY

            var miniSecretAlice = new MiniSecret(Utils.HexToByteArray("0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a"), ExpandMode.Ed25519);
            var accountAlice = Account.Build(KeyType.Sr25519, miniSecretAlice.ExpandToSecret().ToBytes(), miniSecretAlice.GetPair().Public.Key);

            // Secret Key URI `//Bob` is account:
            // Secret seed:      0x398f0c28f98885e046333d4a41c19cee4c37368a9832c6502f6cfd182e2aef89
            // Public key(hex):  0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
            // Account ID:       0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
            // SS58 Address:     5FHneW46xGXgs5mUiveU4sbTyGBzmstUspZC92UhjJM694ty

            var miniSecretBob = new MiniSecret(Utils.HexToByteArray("0x398f0c28f98885e046333d4a41c19cee4c37368a9832c6502f6cfd182e2aef89"), ExpandMode.Ed25519);
            var accountBob = Account.Build(KeyType.Sr25519, miniSecretBob.ExpandToSecret().ToBytes(), miniSecretBob.GetPair().Public.Key);

            Assert.AreEqual("0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011", Utils.Bytes2HexString(accountAlice.PrivateKey));
            Assert.AreEqual("5FHneW46xGXgs5mUiveU4sbTyGBzmstUspZC92UhjJM694ty", accountBob.Value);


            using var client = new SubstrateClient(new Uri(WebSocketUrl));
            var cts = new CancellationTokenSource();
            await client.ConnectAsync(cts.Token);


            Action<string, ExtrinsicStatus> actionExtrinsicUpdate = (subscriptionId, extrinsicUpdate) => {

                switch (extrinsicUpdate.ExtrinsicState)
                {
                    case ExtrinsicState.None:
                        Assert.IsTrue(true);
                        Assert.IsTrue(extrinsicUpdate.InBlock.Value.Length > 0 || extrinsicUpdate.Finalized.Value.Length > 0);
                        break;
                    case ExtrinsicState.Future:
                        Assert.IsTrue(false);
                        break;
                    case ExtrinsicState.Ready:
                        Assert.IsTrue(true);
                        break;
                    case ExtrinsicState.Dropped:
                        Assert.IsTrue(false);
                        break;
                    case ExtrinsicState.Invalid:
                        Assert.IsTrue(false);
                        break;
                }

            };
            
            _ = await client.Author.SubmitAndWatchExtrinsicAsync(actionExtrinsicUpdate, ExtrinsicCall.BalanceTransfer(accountBob.Value, 100000000000), accountAlice, 0, 64, cts.Token);
            Thread.Sleep(10000);
            
        }
    }
}