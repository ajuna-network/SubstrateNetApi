using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using NLog;
using StreamJsonRpc;
using SubstrateNetApi;

namespace DemoApiTest
{
    class Program
    {
        private const string WEBSOCKETURL = "wss://boot.worldofmogwais.com";

        private static async Task Main(string[] args)
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt", DeleteOldFileOnStartup = true };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;

            // Add this to your C# console app's Main method to give yourself
            // a CancellationToken that is canceled when the user hits Ctrl+C.
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("Canceling...");
                cts.Cancel();
                e.Cancel = true;
            };

            try
            {
                Console.WriteLine("Press Ctrl+C to end.");
                await MainAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                // This is the normal way we close.
            }
        }

        static async Task MainAsync(CancellationToken cancellationToken)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));

            await client.ConnectAsync(cancellationToken);

            /***
             * Testing storage data ...
             */

            //var reqResult = await client.GetStorageAsync("Sudo", "Key", cancellationToken);

            // [Plain] Value: u64
            //var reqResult = await client.GetStorageAsync("Dmog", "AllMogwaisCount", cancellationToken);

            // [Map] Key: u64, Hasher: Blake2_128Concat, Value: T::Hash
            //var reqResult = await client.GetStorageAsync("Dmog", "AllMogwaisArray", "0", cancellationToken);

            // [Map] Key: T::Hash, Hasher: Identity, Value: Optional<T::AccountId>
            //var reqResult = await client.GetStorageAsync("Dmog", "MogwaiOwner", "0xAD35415CB5B574819C8521B9192FFFDA772C0770FED9A55494293B2D728F104C", cancellationToken);

            // [Map] Key: T::Hash, Hasher: Identity, Value: MogwaiStruct<T::Hash, BalanceOf<T>>
            var reqResult = await client.GetStorageAsync("Dmog", "Mogwais", "0xAD35415CB5B574819C8521B9192FFFDA772C0770FED9A55494293B2D728F104C", cancellationToken);

            // Print result
            Console.WriteLine($"RESPONSE: '{reqResult}' [{reqResult.GetType().Name}]");

            // Serializer
            //Console.WriteLine(client.MetaData.Serialize());

            //await client.CloseAsync(cancellationToken);
        }
    }
}