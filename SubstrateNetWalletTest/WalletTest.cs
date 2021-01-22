using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetWallet;
using System;
using System.IO;
using System.Reflection;
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
        public void CreateWalletTest()
        {
            // create new wallet with password and persist
            var wallet1 = new Wallet();

            wallet1.CreateAsync("aA1234dd");

            Assert.True(wallet1.IsCreated);
            
            Assert.True(wallet1.IsUnlocked);

            // read wallet
            var wallet2 = new Wallet();

            wallet2.Load("wallet");

            Assert.True(wallet2.IsCreated);

            Assert.False(wallet2.IsUnlocked);

            // unlock wallet with password
            wallet2.UnlockAsync("aA1234dd");

            Assert.True(wallet2.IsUnlocked);

            Assert.AreEqual(wallet1.Account.Address, wallet2.Account.Address);


            var wallet3 = new Wallet();

            Assert.False(wallet3.IsCreated);

            wallet3.Load("wallet");

            Assert.True(wallet3.IsCreated);

            Assert.False(wallet3.IsUnlocked);

            // unlock wallet with password
            wallet3.UnlockAsync("aA4321dd");

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

    }
}