using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;
using StreamJsonRpc;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel.Calls;
using SubstrateNetApi.MetaDataModel.Extrinsics;
using SubstrateNetApi.MetaDataModel.Rpc;
using SubstrateNetApi.MetaDataModel.Values;
using SubstrateNetApi.TypeConverters;

namespace DemoApiTest
{
    class Program
    {
        private const string WEBSOCKETURL = "wss://node01.dotmog.com";

        private static async Task Main(string[] args)
        {
            var config = new LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new FileTarget("logfile") { 
                FileName = "log.txt", 
                DeleteOldFileOnStartup = true 
            };

            var logconsole = new ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

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
            Account accountAlice = new Account(
                KeyType.SR25519, 
                Utils.HexToByteArray("0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011"), 
                Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"));

            Account accountZurich = new Account(
                KeyType.ED25519,
                Utils.HexToByteArray("0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e"),
                Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM"));

            Account accountDMOG_GALxeh = new Account(
                KeyType.ED25519,
                Utils.HexToByteArray("0x3f997449154f8aaa134341b07c3710f63d57e73025105ca7e65a151d7fc3e2bf4b94e38b0c2ee21c367d4c9584204ce62edf5b4a6f675f10678cc56b6ea86e71"),
                Utils.GetPublicKeyFrom("5DmogGALxehCbUmm45XJoADcf9BU71ZK2zmqHDPFJD3VxknC"));

            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));

            client.RegisterTypeConverter(new MogwaiStructTypeConverter());

            await client.ConnectAsync(cancellationToken);

            var systemName = await client.System.NameAsync(cancellationToken);
            var systemVersion = await client.System.VersionAsync(cancellationToken);
            var systemChain = await client.System.ChainAsync(cancellationToken);
            Console.WriteLine($"Connected to System: {systemName} Chain: {systemChain} Version: {systemVersion}.");

            /***
             * Testing storage data ...
             */

            //var reqResult = await client.GetStorageAsync("Sudo", "Key", cancellationToken);

            // [Plain] Value: u64
            //var reqResult = await client.GetStorageAsync("Dmog", "AllMogwaisCount", cancellationToken);

            // [Map] Key: u64, Hasher: Blake2_128Concat, Value: T::Hash
            //var reqResult = await client.GetStorageAsync("DotMogModule", "AllMogwaisArray", "0", cancellationToken);

            // [Map] Key: T::Hash, Hasher: Identity, Value: Optional<T::AccountId>
            //var reqResult = await client.GetStorageAsync("Dmog", "MogwaiOwner", "0xAD35415CB5B574819C8521B9192FFFDA772C0770FED9A55494293B2D728F104C", cancellationToken);

            // [Map] Key: T::Hash, Hasher: Identity, Value: MogwaiStruct<T::Hash, BalanceOf<T>>
            //var reqResult = await client.GetStorageAsync("DotMogModule", "Mogwais", "0x17E26CA749780270EEC18507AB3C03854E75E264DB13EC1F90314C3AF02CCDF8", cancellationToken);

            //var reqResult = await client.GetMethodAsync<JArray>("system_peers", cancellationToken);

            // [Map] Key: T::AccountId, Hasher: Blake2_128Concat, Value: AccountInfo<T::Index, T::AccountData>
            var reqResult = await client.GetStorageAsync("System", "Account", Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH")), cancellationToken);
            //var reqResult = await client.GetStorageAsync("System", "Account", Utils.Bytes2HexString(Utils.HexToByteArray("0xD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D")), cancellationToken);

            //var reqResult = await client.GetMethodAsync<JArray>("author_pendingExtrinsics", cancellationToken);

            // *************************** Final Test
            //var reqResult = await client.SubmitExtrinsicAsync(DmogCall.CreateMogwai(), accountAlice, 0, 64, cancellationToken);
            //var reqResult = await client.Author.PendingExtrinsicAsync(cancellationToken);
            //var reqResult = await client.SubmitExtrinsicAsync(ExtrinsicCall.BalanceTransfer("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY", 100000000), accountDMOG_GALxeh, 0, 64, cancellationToken);

            // *** subscription test 1
            //var subscriptionId = await client.Chain
            //    .SubscribeAllHeadsAsync(
            //    (header) => Console.WriteLine($"CallBack: {header}"), 
            //    cancellationToken
            //);
            //Thread.Sleep(30000);
            //var reqResult = await client.Chain.UnsubscribeAllHeadsAsync(subscriptionId, cancellationToken);

            // *** subscription test 2
            //Action<ExtrinsicStatus> actionExtrinsicUpdate = (extrinsicUpdate) => Console.WriteLine($"CallBack: {extrinsicUpdate}");
            //var subscriptionId = await client.Author.SubmitAndWatchExtrinsicAsync(actionExtrinsicUpdate, ExtrinsicCall.BalanceTransfer("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY", 1000), accountDMOG_GALxeh, 0, 64, cancellationToken);
            //Thread.Sleep(60000);
            //var reqResult = await client.Author.UnwatchExtrinsicAsync(subscriptionId, cancellationToken);

            // *** subscription test 3


            //Hash finalizedHead = await client.Chain.GetFinalizedHeadAsync(cancellationToken);
            //var reqResult = await client.Chain.GetBlockAsync(finalizedHead, cancellationToken);

            // Print result
            Console.WriteLine($"RESPONSE: '{reqResult}' [{reqResult.GetType().Name}]");

            //Console.WriteLine(client.MetaData.Serialize());

            Console.ReadKey();

            // Close connection
            await client.CloseAsync(cancellationToken);

        }

    }
}