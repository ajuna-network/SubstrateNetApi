using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types;
using SubstrateNetWallet;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SubstrateNetWalletTest
{
    public class Tests
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

            wallet2.Load("wallet");

            Assert.True(wallet2.IsCreated);

            Assert.False(wallet2.IsUnlocked);

            // unlock wallet with password
            await wallet2.UnlockAsync("aA1234dd");

            Assert.True(wallet2.IsUnlocked);

            Assert.AreEqual(wallet1.Account.Address, wallet2.Account.Address);


            var wallet3 = new Wallet();

            Assert.False(wallet3.IsCreated);

            wallet3.Load("wallet");

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
        public async Task ConnectionTestAsync()
        {
            // create new wallet with password and persist
            var wallet = new Wallet();
            await wallet.StartAsync("wss://node01.dotmog.com");

            Assert.True(wallet.IsConnected);

            Assert.AreEqual("Substrate Node", wallet.ChainInfo.Name);
            Assert.AreEqual("2.0.0-37f7720d9-x86_64-linux-gnu", wallet.ChainInfo.Version);
            Assert.AreEqual("DOT Mog Testnet", wallet.ChainInfo.Chain);

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

            Assert.AreEqual("5FfzQe73TTQhmSQCgvYocrr6vh1jJXEKB8xUB6tExfpKVCEZ", wallet.Account.Address);
        }

        [Test]
        public async Task CheckStorageCallsAsync()
        {
            // create new wallet with password and persist
            var wallet = new Wallet();

            await wallet.StartAsync("wss://node01.dotmog.com");

            Assert.True(wallet.IsConnected);

            wallet.Load("dev_wallet");

            await wallet.UnlockAsync("aA1234dd");
            Assert.True(wallet.IsUnlocked);

            Assert.AreEqual("Substrate Node", wallet.ChainInfo.Name);
            Assert.AreEqual("2.0.0-37f7720d9-x86_64-linux-gnu", wallet.ChainInfo.Version);
            Assert.AreEqual("DOT Mog Testnet", wallet.ChainInfo.Chain);

            Assert.AreEqual("5FfzQe73TTQhmSQCgvYocrr6vh1jJXEKB8xUB6tExfpKVCEZ", wallet.Account.Address);

            Thread.Sleep(1000);

            Assert.AreEqual("1124998929864629549", wallet.AccountInfo.AccountData.Free.Value.ToString());

            var countMogwais = (ulong)await wallet.Client.GetStorageAsync("DotMogModule", "OwnedMogwaisCount", new string[] { Utils.Bytes2HexString(wallet.Account.PublicKey) });

            Assert.AreEqual(1, countMogwais);

            await wallet.StopAsync();

            Assert.False(wallet.IsConnected);
        }

        [Test]
        public async Task CheckRaisedChainInfoUpdatedAsync()
        {
            var wallet = new Wallet();

            await wallet.StartAsync("wss://node01.dotmog.com");

            Assert.True(wallet.IsConnected);

            wallet.Load("dev_wallet");

            ChainInfo test = null; 

            wallet.ChainInfoUpdated += delegate (object sender, ChainInfo chainInfo)
            {
                test = chainInfo;
            };

            Thread.Sleep(6000);

            Assert.IsNotNull(test);

            Assert.AreEqual("Substrate Node", test.Name);
        }

        [Test]
        public async Task CheckRaisedAccountInfoUpdatedAsync()
        {
            // create new wallet with password and persist
            var wallet = new Wallet();

            await wallet.StartAsync("wss://node01.dotmog.com");

            Assert.True(wallet.IsConnected);

            wallet.Load("dev_wallet");

            Assert.True(wallet.IsCreated);

            await wallet.UnlockAsync("aA1234dd");

            Assert.True(wallet.IsUnlocked);

            AccountInfo test = null;

            wallet.AccountInfoUpdated += delegate (object sender, AccountInfo accountInfo)
            {
                test = accountInfo;
            };

            Thread.Sleep(1000);

            Assert.IsNotNull(test);

            Assert.AreEqual(1, test.Nonce);

            Assert.AreEqual("1124998929864629549", test.AccountData.Free.ToString());
        }

        [Test]
        public async Task CheckRaisedOwnedMogwaisCountAsync()
        {
            var wallet = new Wallet();

            await wallet.StartAsync("wss://node01.dotmog.com");

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
                    int p = 0;
                    test = U64.Decode(Utils.HexToByteArray(eventObject.Changes[0][1]), ref p).Value;
                }
            };

            await wallet.Client.SubscribeStorageKeyAsync("DotMogModule", "OwnedMogwaisCount",
                    new string[] { Utils.Bytes2HexString(wallet.Account.PublicKey)},
                    callOwnedMogwaisCount);

            Thread.Sleep(1000);

            Assert.AreEqual(1, test);
        }
    }

}