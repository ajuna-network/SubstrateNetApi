using SubstrateNetWallet;
using System;
using System.IO;
using System.Threading.Tasks;

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

            await wallet.StartAsync("wss://node01.dotmog.com");

            if (!wallet.IsConnected)
            {
                return;
            }

            if (wallet.Load())
            {
                Console.WriteLine("wallet unlocked");
                wallet.Unlock("Aa123456");
            }
            else if (!wallet.IsCreated)
            {
                Console.WriteLine("wallet created");
                wallet.Create("Aa123456");
            }

            Console.WriteLine(wallet.Account.Address); 
            
            Console.ReadKey();

            Console.WriteLine(wallet.AccountInfo);

            await wallet.StopAsync();

        }

        private static void Wallet_ChainInfoUpdated(object sender, ChainInfo chainInfo)
        {
            Console.WriteLine(chainInfo);
        }
    }
}
