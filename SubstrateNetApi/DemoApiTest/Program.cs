using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using StreamJsonRpc;
using SubstrateNetApi;

namespace DemoApiTest
{
    class Program
    {
        private const string WEBSOCKETURL = "wss://boot.worldofmogwais.com";

        private static async Task Main(string[] args)
        {
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
            SubstrateClient client = new SubstrateClient(new Uri(WEBSOCKETURL));
            
            await client.ConnectAsync();

            var reqResult = await client.TryRequestAsync("Sudo", "Key");

            var accountId = (AccountId)reqResult;

            Console.WriteLine(accountId);

            client.Disconnect();


        }
    }
}