using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using Schnorrkel.Keys;
using SubstrateNetApi;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.PalletBalances;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.SpRuntime;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Custom;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.TypeConverters;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace ExentsionTest
{
    public class ExentsionTest
    {
        private const string WebSocketUrl = "ws://127.0.0.1:9944";

        private SubstrateClientExt _substrateClient;


        // Secret Key URI `//Alice` is account:
        // Secret seed:      0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a
        // Public key(hex):  0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
        // Account ID:       0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
        // SS58 Address:     5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY
        public MiniSecret MiniSecretAlice => new MiniSecret(Utils.HexToByteArray("0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a"), ExpandMode.Ed25519);
        public Account Alice => Account.Build(KeyType.Sr25519, MiniSecretAlice.ExpandToSecret().ToBytes(), MiniSecretAlice.GetPair().Public.Key);

        // Secret Key URI `//Bob` is account:
        // Secret seed:      0x398f0c28f98885e046333d4a41c19cee4c37368a9832c6502f6cfd182e2aef89
        // Public key(hex):  0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
        // Account ID:       0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
        // SS58 Address:     5FHneW46xGXgs5mUiveU4sbTyGBzmstUspZC92UhjJM694ty
        public MiniSecret MiniSecretBob => new MiniSecret(Utils.HexToByteArray("0x398f0c28f98885e046333d4a41c19cee4c37368a9832c6502f6cfd182e2aef89"), ExpandMode.Ed25519);
        public Account Bob => Account.Build(KeyType.Sr25519, MiniSecretBob.ExpandToSecret().ToBytes(), MiniSecretBob.GetPair().Public.Key);


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

            _substrateClient = new SubstrateClientExt(new Uri(WebSocketUrl));
            
            // add your generic type converters here
            //_substrateClient.RegisterTypeConverter(new GenericTypeConverter<EnumType<BoardState>>());
            
        }

        [TearDown]
        public void TearDown()
        {
            _substrateClient.Dispose();
        }

        [Test]
        public async Task GetMethodChainNameTestAsync()
        {
            await _substrateClient.ConnectLightAsync(CancellationToken.None);

            var result = await _substrateClient.GetMethodAsync<string>("system_chain");
            Assert.AreEqual("Development", result);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task GetAccountNextIndexTestAsync()
        {

            var accountId32 = new AccountId32();
            accountId32.Create("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");
            var address = Utils.GetAddressFrom(accountId32.Bytes);


            await _substrateClient.ConnectLightAsync(CancellationToken.None);

            var result = await _substrateClient.System.AccountNextIndexAsync(address, CancellationToken.None);
            Assert.AreEqual(0, result);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task GetTotalIssuanceStorageTestAsync()
        {
            await _substrateClient.ConnectLightAsync(CancellationToken.None);

            var result = await _substrateClient.PalletBalancesStorage.TotalIssuance(CancellationToken.None);
            Assert.AreEqual("12000000100000000000000", result.Value.ToString());

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task GetAccountStorageTestAsync()
        {
            await _substrateClient.ConnectLightAsync(CancellationToken.None);

            var accountId32 = new AccountId32();
            accountId32.Create("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");

            var result = await _substrateClient.FrameSystemStorage.Account(accountId32, CancellationToken.None);
            Assert.AreEqual("1000000000000000000000", result.Data.Free.Value.ToString());

            await _substrateClient.CloseAsync();
        }

        /// <summary>
        /// Simple extrinsic tester
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="extrinsicUpdate"></param>
        static void ActionExtrinsicUpdate(string subscriptionId, ExtrinsicStatus extrinsicUpdate)
        {
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
        }

        [Test]
        public async Task BalanceTransferTestAsync()
        {
            var extrinsic_wait = 5000;

            var cts = new CancellationTokenSource();
            await _substrateClient.ConnectLightAsync(cts.Token);

            var bobAccountId32 = new AccountId32();
            bobAccountId32.Create("0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48");

            var result = await _substrateClient.FrameSystemStorage.Account(bobAccountId32, CancellationToken.None);
            Assert.AreEqual("1000000000000000000000", result.Data.Free.Value.ToString());

            var enumMultiAddress = new EnumMultiAddress();
            enumMultiAddress.Create(MultiAddress.Id, bobAccountId32);

            var amount = new BaseCom<U128>();
            amount.Create(new CompactInteger(new BigInteger(100000000000)));

            var extrinsicCall = new PalletBalancesCall().Transfer(enumMultiAddress, amount);

            // Alice sends bob some coins ...
            _ = await _substrateClient.Author.SubmitAndWatchExtrinsicAsync(ActionExtrinsicUpdate, extrinsicCall, Alice, 0, 64, cts.Token);
            Thread.Sleep(extrinsic_wait);

            var freeAfter = await _substrateClient.FrameSystemStorage.Account(bobAccountId32, CancellationToken.None);
            Assert.AreEqual("1000000000000000000000", freeAfter.Data.Free.Value.ToString());
        }

    }
}