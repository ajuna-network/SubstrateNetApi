using System;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetWallet;

namespace SubstrateNetWalletTest
{
    public class WalletTest
    {
        [SetUp]
        public void Setup()
        {
            SystemInteraction.ReadData = f => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.DataExists = f => File.Exists(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.ReadPersistent = f => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.PersistentExists = f => File.Exists(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.Persist = (f, c) => File.WriteAllText(Path.Combine(Environment.CurrentDirectory, f), c);
        }

        [Test]
        public void IsValidPasswordTest()
        {
            var wallet = new Wallet();

            Assert.False(wallet.IsValidPassword("12345678"));
            Assert.False(wallet.IsValidPassword("ABCDEFGH"));
            Assert.False(wallet.IsValidPassword("abcdefgh"));
            Assert.False(wallet.IsValidPassword("ABCDefgh"));
            Assert.False(wallet.IsValidPassword("1BCDefg"));

            Assert.True(wallet.IsValidPassword("ABCDefg1"));
        }

        [Test]
        public void IsValidWalletNameTest()
        {
            var wallet = new Wallet();

            Assert.False(wallet.IsValidWalletName("1234"));
            Assert.False(wallet.IsValidWalletName("ABC_/"));

            Assert.True(wallet.IsValidWalletName("wal_let"));
            Assert.True(wallet.IsValidWalletName("1111111"));
        }

        [Test]
        public async Task CreateWalletTestAsync()
        {
            // create new wallet with password and persist
            var wallet1 = new Wallet();

            await wallet1.CreateAsync("aA1234dd");

            Assert.True(wallet1.IsCreated);

            Assert.True(wallet1.IsUnlocked);

            // read wallet
            var wallet2 = new Wallet();

            wallet2.Load();

            Assert.True(wallet2.IsCreated);

            Assert.False(wallet2.IsUnlocked);

            // unlock wallet with password
            await wallet2.UnlockAsync("aA1234dd");

            Assert.True(wallet2.IsUnlocked);

            Assert.AreEqual(wallet1.Account.Value, wallet2.Account.Value);


            var wallet3 = new Wallet();

            Assert.False(wallet3.IsCreated);

            wallet3.Load();

            Assert.True(wallet3.IsCreated);

            Assert.False(wallet3.IsUnlocked);

            // unlock wallet with password
            await wallet3.UnlockAsync("aA4321dd");

            Assert.False(wallet3.IsUnlocked);

            var wallet4 = new Wallet();
            wallet4.Load("dev_wallet");

            Assert.True(wallet4.IsCreated);
        }

        [Test]
        public async Task CreateMnemonicWalletTestAsync()
        {
            var mnemonic = "donor rocket find fan language damp yellow crouch attend meat hybrid pulse";

            // create new wallet with password and persist
            var wallet1 = new Wallet();

            await wallet1.CreateAsync("aA1234dd", mnemonic, "mnemonic_wallet");

            Assert.True(wallet1.IsCreated);

            Assert.True(wallet1.IsUnlocked);

            // read wallet
            var wallet2 = new Wallet();

            wallet2.Load("mnemonic_wallet");

            Assert.True(wallet2.IsCreated);

            Assert.False(wallet2.IsUnlocked);

            // unlock wallet with password
            await wallet2.UnlockAsync("aA1234dd");

            Assert.True(wallet2.IsUnlocked);

            Assert.AreEqual(wallet1.Account.Value, wallet2.Account.Value);
        }

        [Test]
        public async Task ConnectionTestAsync()
        {
            // create new wallet with password and persist
            var wallet = new Wallet();
            await wallet.StartAsync();

            Assert.True(wallet.IsConnected);

            Assert.AreEqual("DOTMog.com NET", wallet.ChainInfo.Chain);

            await wallet.StopAsync();

            Assert.False(wallet.IsConnected);
        }


        [Test]
        public async Task CheckAccountAsync()
        {
            var wallet = new Wallet();
            wallet.Load("dev_wallet");

            Assert.True(wallet.IsCreated);

            await wallet.UnlockAsync("aA1234dd");

            Assert.True(wallet.IsUnlocked);

            Assert.AreEqual("5FfzQe73TTQhmSQCgvYocrr6vh1jJXEKB8xUB6tExfpKVCEZ", wallet.Account.Value);
        }

        [Test]
        public async Task CheckStorageCallsAsync()
        {
            // create new wallet with password and persist
            var wallet = new Wallet();

            await wallet.StartAsync();

            Assert.True(wallet.IsConnected);

            wallet.Load("dev_wallet");

            await wallet.UnlockAsync("aA1234dd");
            Assert.True(wallet.IsUnlocked);

            Assert.AreEqual("5FfzQe73TTQhmSQCgvYocrr6vh1jJXEKB8xUB6tExfpKVCEZ", wallet.Account.Value);

            Thread.Sleep(1000);

            Assert.True(BigInteger.Parse("400000000000000") < wallet.AccountInfo.AccountData.Free.Value);
            Assert.True(BigInteger.Parse("600000000000000") > wallet.AccountInfo.AccountData.Free.Value);

            var blockNumber = new BlockNumber();
            blockNumber.Create(10);
            var blockHash = await wallet.Client.Chain.GetBlockHashAsync(blockNumber);

            var header = await wallet.Client.Chain.GetHeaderAsync(blockHash);
            Assert.AreEqual(10, header.Number.Value);

            var countMogwais = (PrimU64) await wallet.Client.GetStorageAsync("DotMogModule", "OwnedMogwaisCount",
                new[] {Utils.Bytes2HexString(wallet.Account.Bytes)});
            Assert.AreEqual(1, countMogwais.Value);


            await wallet.StopAsync();

            Assert.False(wallet.IsConnected);
        }

        [Test]
        public async Task CheckRaisedChainInfoUpdatedAsync()
        {
            var wallet = new Wallet();

            await wallet.StartAsync();

            Assert.True(wallet.IsConnected);

            Assert.IsTrue(wallet.Load("dev_wallet"));

            ChainInfo test = null;
            
            void OnChainInfoUpdated(object sender, ChainInfo chainInfo)
            {
                test = chainInfo;
            }

            wallet.ChainInfoUpdated += OnChainInfoUpdated;

            Thread.Sleep(5000);

            Assert.IsNotNull(test);

            Assert.AreEqual("DOTMog Node", test.Name);

            wallet.ChainInfoUpdated -= OnChainInfoUpdated;
        }

        [Test]
        public async Task CheckRaisedAccountInfoUpdatedAsync()
        {
            // create new wallet with password and persist
            var wallet = new Wallet();

            await wallet.StartAsync();

            Assert.True(wallet.IsConnected);

            wallet.Load("dev_wallet");

            Assert.True(wallet.IsCreated);

            await wallet.UnlockAsync("aA1234dd");

            Assert.True(wallet.IsUnlocked);

            Assert.AreEqual("5FfzQe73TTQhmSQCgvYocrr6vh1jJXEKB8xUB6tExfpKVCEZ", wallet.Account.Value);

            AccountInfo test = null;

            wallet.AccountInfoUpdated += delegate(object sender, AccountInfo accountInfo)
            {
                test = accountInfo;
            };

            Thread.Sleep(3000);

            Assert.IsNotNull(test);

            Assert.AreEqual(2, test.Nonce.Value);

            Assert.True(BigInteger.Parse("400000000000000") < test.AccountData.Free.Value);
            Assert.True(BigInteger.Parse("600000000000000") > test.AccountData.Free.Value);
        }

        [Test]
        public async Task CheckRaisedOwnedMogwaisCountAsync()
        {
            var wallet = new Wallet();

            await wallet.StartAsync();

            Assert.True(wallet.IsConnected);

            wallet.Load("dev_wallet");

            Assert.True(wallet.IsCreated);

            await wallet.UnlockAsync("aA1234dd");

            Assert.True(wallet.IsUnlocked);

            ulong test = 0;

            Action<string, StorageChangeSet> callOwnedMogwaisCount = (subscriptionId, eventObject) =>
            {
                Console.WriteLine($"Subscription[{subscriptionId}]: {eventObject}");
                if (eventObject.Changes != null)
                {
                    var p = 0;
                    var u64 = new PrimU64();
                    u64.Decode(Utils.HexToByteArray(eventObject.Changes[0][1]), ref p);
                    test = u64.Value;
                }
            };

            await wallet.Client.SubscribeStorageKeyAsync("DotMogModule", "OwnedMogwaisCount",
                new[] {Utils.Bytes2HexString(wallet.Account.Bytes)},
                callOwnedMogwaisCount);

            Thread.Sleep(1000);

            Assert.AreEqual(1, test);
        }
    }
}