using SubstrateNetApi.Model.Types;
using SubstrateNetWallet;
using System;
using System.IO;
using System.Threading.Tasks;
using SubstrateNetApi.Model.Rpc;

namespace DemoWalletTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SystemInteraction.ReadData = f => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.DataExists = f => File.Exists(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.ReadPersistent = f => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.PersistentExists = f => File.Exists(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.Persist = (f, c) => File.WriteAllText(Path.Combine(Environment.CurrentDirectory, f), c);

            // create new wallet with password and persist
            var wallet = new Wallet();

            wallet.ChainInfoUpdated += Wallet_ChainInfoUpdated;

            wallet.AccountInfoUpdated += Wallet_AccountInfoUpdated;

            await wallet.StartAsync("wss://mogiway-01.dotmog.com");

            if (!wallet.IsConnected)
            {
                return;
            }

            if (wallet.Load())
            {
                Console.WriteLine("wallet unlocked");
                await wallet.UnlockAsync("Aa123456");
            }
            else if (!wallet.IsCreated)
            {
                Console.WriteLine("wallet created");
                await wallet.CreateAsync("Aa123456");
            }

            Console.WriteLine(wallet.Account.Value);

            Console.ReadKey();

            await wallet.StopAsync();

        }

        private static void Wallet_AccountInfoUpdated(object sender, AccountInfo accountInfo)
        {
            Console.WriteLine($"CallBack[AccountInfo]: {accountInfo}");
        }

        private static void Wallet_ChainInfoUpdated(object sender, ChainInfo chainInfo)
        {
            Console.WriteLine($"CallBack[ChainInfo]: {chainInfo}");
        }
    }
}
