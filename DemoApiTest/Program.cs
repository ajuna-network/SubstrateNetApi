using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json.Linq;
using NLog;
using StreamJsonRpc;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel.Calls;
using SubstrateNetApi.MetaDataModel.Values;
using SubstrateNetApi.TypeConverters;

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

            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));

            client.RegisterTypeConverter(new MogwaiStructTypeConverter());

            await client.ConnectAsync(cancellationToken);

            var systemName = await client.System.NameAsync(cancellationToken);
            var systemVersion = await client.System.VersionAsync(cancellationToken);
            var systemChain = await client.System.ChainAsync(cancellationToken);
            Console.WriteLine($"Connected to System: {systemName} Chain: {systemChain} Version: {systemVersion}.");

            //Console.WriteLine(client.MetaData.Encode());

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
            //var reqResult = await client.GetStorageAsync("Dmog", "Mogwais", "0xAD35415CB5B574819C8521B9192FFFDA772C0770FED9A55494293B2D728F104C", cancellationToken);

            //var reqResult = await client.GetMethodAsync<string>("system_name", cancellationToken);
            //var reqResult = await client.GetMethodAsync<string>("chain_getBlockHash", cancellationToken);
            //var reqResult = await client.GetMethodAsync<JArray>("system_peers", cancellationToken);

            // [Map] Key: T::AccountId, Hasher: Blake2_128Concat, Value: AccountInfo<T::Index, T::AccountData>
            //var reqResult = await client.GetStorageAsync("System", "Account", "0xD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D", cancellationToken);
            //var reqResult = await client.GetStorageAsync("System", "Account", Utils.Bytes2HexString(Utils.HexToByteArray("0xD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D")), cancellationToken);


            //string priKey0x = "0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e";
            //string pubKey0x = "0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e";
            ////var reqResult = Utils.Bytes2HexString(Utils.HexToByteArray("0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e"));
            //var reqResult = await client.GetStorageAsync("System", "Account", "0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e", cancellationToken);
            //var reqResult = await client.SubmitExtrinsicAsync("Dmog", "create_mogwai", null, Utils.HexToByteArray(pubKey0x), Utils.HexToByteArray(priKey0x), cancellationToken);

            //var reqResult = await client.GetMethodAsync<JArray>("author_pendingExtrinsics", cancellationToken);

            //var reqResult = await client.System.AccountNextIndexAsync("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY", cancellationToken);

            //var reqResult = await client.Chain.GetBlockAsync(cancellationToken);
            //var reqResult = await client.Chain.GetBlockAsync(cancellationToken);
            //var reqResult = await client.Chain.GetBlockAsync(Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9"), cancellationToken);
            //var reqResult = await client.Chain.GetBlockAsync(cancellationToken);

            //var systemChain = await client.System.ChainAsync(cancellationToken);

            //var reqResult = await client.Chain.GetHeaderAsync(new Hash("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9"), cancellationToken);

            // *************************** Final Test
            var reqResult = await client.SubmitExtrinsicAsync(ExtrinsicCall.DmogCreateMogwai(), accountZurich, 0, 64, cancellationToken);

            //var reqResult = await client.SubmitExtrinsicAsync(ExtrinsicCall.BalanceTransfer("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY", 100), accountZurich, 0, 64, cancellationToken);

            //Hash finalizedHead = await client.Chain.GetFinalizedHeadAsync(cancellationToken);
            //var reqResult = await client.Chain.GetBlockAsync(finalizedHead, cancellationToken);

            // Print result
            Console.WriteLine($"RESPONSE: '{reqResult}' [{reqResult.GetType().Name}]");

            //var subId = await client.Chain.SubscribeNewHeadAsync(cancellationToken);
            //Console.WriteLine(subId);
            //Console.ReadLine();

            // Serializer
            //Console.WriteLine(client.MetaData.Encode());

            //await client.CloseAsync(cancellationToken);

        }
    }
}