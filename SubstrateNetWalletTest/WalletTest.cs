using NUnit.Framework;
using SubstrateNetWallet;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SubstrateNetWalletTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            SystemInteraction.ReadData = f => File.ReadAllText(Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), f));
            SystemInteraction.DataExists = f => File.Exists(Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), f));
            SystemInteraction.ReadPersistent = f => File.ReadAllText(Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), f));
            SystemInteraction.PersistentExists = f => File.Exists(Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), f));
            SystemInteraction.Persist = (f, c) => File.WriteAllText(Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), f), c);
        }

        [Test]
        public void CreateWalletTest()
        {
            // create new wallet with password and persist
            var wallet1 = new Wallet("1234", "wallet.dat");
            
            Assert.True(wallet1.IsCreated);
            Assert.True(wallet1.IsUnlocked);

            // read wallet
            var wallet2 = new Wallet("wallet.dat");

            Assert.True(wallet2.IsCreated);
            Assert.False(wallet2.IsUnlocked);

            // unlock wallet with password
            wallet2.Unlock("1234");

            Assert.True(wallet2.IsUnlocked);

            Assert.AreEqual(wallet1.Account.Address, wallet2.Account.Address);
        }

        [Test]
        public async Task ConnectionTestAsync()
        {
            // create new wallet with password and persist
            var wallet = new Wallet("1234", "wallet.dat");
            await wallet.ConnectAsync("wss://node01.dotmog.com");

            Assert.True(wallet.IsConnected);

            Assert.AreEqual("Substrate Node", wallet.ChainInfo.Name);
            Assert.AreEqual("2.0.0-37f7720d9-x86_64-linux-gnu", wallet.ChainInfo.Version);
            Assert.AreEqual("DOT Mog Testnet", wallet.ChainInfo.Chain);

            await wallet.DisconnectAsync();

            Assert.False(wallet.IsConnected);
        }


    }
}