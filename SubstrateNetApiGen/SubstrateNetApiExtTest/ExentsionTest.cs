using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using Schnorrkel.Keys;
using SubstrateNetApi;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.Extrinsics;
using SubstrateNetApi.Model.FrameSystem;
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
            await _substrateClient.ConnectAsync(false, CancellationToken.None);

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


            await _substrateClient.ConnectAsync(false, CancellationToken.None);

            var result = await _substrateClient.System.AccountNextIndexAsync(address, CancellationToken.None);
            Assert.AreEqual(0, result);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task GetFinalizedHeadAsyncTestAsync()
        {
            await _substrateClient.ConnectAsync(false, CancellationToken.None);

            var result1 = await _substrateClient.Chain.GetFinalizedHeadAsync(CancellationToken.None);
            Assert.AreEqual("0x", result1.Value.Substring(0,2));

            var result2 = await _substrateClient.Chain.GetFinalizedTestHeadAsync(CancellationToken.None);
            Assert.AreEqual("0x", result2.Substring(0, 2));

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task GetTotalIssuanceStorageTestAsync()
        {
            var lowValue = BigInteger.Parse("12000000100000000000000");

            await _substrateClient.ConnectAsync(false, CancellationToken.None);

            var result = await _substrateClient.BalancesStorage.TotalIssuance(CancellationToken.None);
            Assert.IsTrue(lowValue <= result.Value);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task GetAccountStorageTestAsync()
        {
            var highValue = BigInteger.Parse("1000000000000000000000");
            var lowValue = BigInteger.Parse("10000000000000000");

            var accountId32 = new AccountId32();
            accountId32.Create("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");

            await _substrateClient.ConnectAsync(false, CancellationToken.None);

            var result = await _substrateClient.SystemStorage.Account(accountId32, CancellationToken.None);
            Assert.IsTrue(highValue >= result.Data.Free.Value);
            Assert.IsTrue(lowValue < result.Data.Free.Value);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task GetAccountSubscribeStorageTestAsync()
        {
            var extrinsic_wait = 5000;

            var highValue = BigInteger.Parse("1000000000000000000000");
            var lowValue = BigInteger.Parse("10000000000000000");

            var accountId32 = new AccountId32();
            accountId32.Create("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");

            await _substrateClient.ConnectAsync(false, CancellationToken.None);

            AccountInfo accountInfo = null;
            Action<string, StorageChangeSet> callOwnedMogwaisCount = (subscriptionId, eventObject) =>
            {
                if (eventObject.Changes != null)
                {
                    var p = 0;
                    accountInfo = new AccountInfo();
                    accountInfo.Decode(Utils.HexToByteArray(eventObject.Changes[0][1]), ref p);

                }
            };

            var subscription = await _substrateClient.SubscribeStorageKeyAsync(SystemStorage.AccountParams(accountId32), callOwnedMogwaisCount, CancellationToken.None);

            Assert.IsTrue(subscription.Length > 0);

            Thread.Sleep(extrinsic_wait);

            Assert.IsNotNull(accountInfo);

            Assert.IsTrue(highValue >= accountInfo.Data.Free.Value);
            Assert.IsTrue(lowValue < accountInfo.Data.Free.Value);

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
        public async Task GetStateTestAsync()
        {
            var cts = new CancellationTokenSource();
            await _substrateClient.ConnectAsync(false, cts.Token);

            var runtime0 = await _substrateClient.State.GetRuntimeVersionAsync();
            var runtime1 = await _substrateClient.State.GetRuntimeVersionAsync(cts.Token);

            Assert.AreEqual("RuntimeVersion", runtime0.GetType().Name);
            Assert.AreEqual("RuntimeVersion", runtime1.GetType().Name);
            var runtimeVersion = runtime0 as RuntimeVersion;

            Assert.AreEqual("node", runtimeVersion.SpecName);
            Assert.AreEqual("substrate-node", runtimeVersion.ImplName);
            Assert.AreEqual(267, runtimeVersion.SpecVersion);

            var metaData0 = await _substrateClient.State.GetMetaDataAsync();
            var metaData1 = await _substrateClient.State.GetMetaDataAsync(cts.Token);

            Assert.AreEqual("String", metaData0.GetType().Name);
            Assert.AreEqual("String", metaData1.GetType().Name);


            await _substrateClient.CloseAsync();
        }

        [Test]
        public void MethodTest()
        {
            var bobAccountId32 = new AccountId32();
            bobAccountId32.Create("0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48");

            var enumMultiAddress = new EnumMultiAddress();
            enumMultiAddress.Create(MultiAddress.Id, bobAccountId32);

            Assert.AreEqual("0x008eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48", Utils.Bytes2HexString(enumMultiAddress.Encode()).ToLower());

            var amount = new BaseCom<U128>();
            amount.Create(new CompactInteger(new BigInteger(100000000000)));

            var extrinsicMethod = SubstrateNetApi.Model.PalletBalances.BalancesCalls.Transfer(enumMultiAddress, amount);
            Assert.AreEqual("0x0600008eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a480700e8764817", Utils.Bytes2HexString(extrinsicMethod.Encode()).ToLower());
        }

        [Test]
        public async Task BalanceTransferWithPendingTestAsync()
        {
            var extrinsic_wait = 5000;

            var cts = new CancellationTokenSource();
            await _substrateClient.ConnectAsync(false, cts.Token);

            var bobAccountId32 = new AccountId32();
            bobAccountId32.Create("0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48");

            var result = await _substrateClient.SystemStorage.Account(bobAccountId32, CancellationToken.None);
            var startValueBob = result.Data.Free.Value;

            var enumMultiAddress = new EnumMultiAddress();
            enumMultiAddress.Create(MultiAddress.Id, bobAccountId32);

            var amount = new BaseCom<U128>();
            amount.Create(new CompactInteger(new BigInteger(100000000000)));

            var extrinsicMethod = SubstrateNetApi.Model.PalletBalances.BalancesCalls.Transfer(enumMultiAddress, amount);

            var test = await _substrateClient.Author.SubmitExtrinsicAsync(extrinsicMethod, Alice, 0, 64, cts.Token);

            var extrinsics = await _substrateClient.Author.PendingExtrinsicAsync();
            Assert.AreEqual(1, extrinsics.Length);

            Thread.Sleep(extrinsic_wait);

            var endValueBob = startValueBob + amount.Value.Value;

            var freeAfter = await _substrateClient.SystemStorage.Account(bobAccountId32, CancellationToken.None);
            Assert.AreEqual(endValueBob, freeAfter.Data.Free.Value);
        }

        [Test]
        public async Task BalanceTransferWatchTestAsync()
        {
            var extrinsic_wait = 5000;

            var cts = new CancellationTokenSource();
            await _substrateClient.ConnectAsync(false, cts.Token);

            var bobAccountId32 = new AccountId32();
            bobAccountId32.Create("0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48");

            var result = await _substrateClient.SystemStorage.Account(bobAccountId32, CancellationToken.None);
            var startValueBob = result.Data.Free.Value;

            var enumMultiAddress = new EnumMultiAddress();
            enumMultiAddress.Create(MultiAddress.Id, bobAccountId32);

            var amount = new BaseCom<U128>();
            amount.Create(new CompactInteger(new BigInteger(100000000000)));

            var extrinsicMethod = SubstrateNetApi.Model.PalletBalances.BalancesCalls.Transfer(enumMultiAddress, amount);

            // Alice sends bob some coins ...
            var subscription = await _substrateClient.Author.SubmitAndWatchExtrinsicAsync(ActionExtrinsicUpdate, extrinsicMethod, Alice, 0, 64, cts.Token);

            Thread.Sleep(extrinsic_wait);
            
            var endValueBob = startValueBob + amount.Value.Value;

            var freeAfter = await _substrateClient.SystemStorage.Account(bobAccountId32, CancellationToken.None);
            Assert.AreEqual(endValueBob, freeAfter.Data.Free.Value);
        }

    }
}