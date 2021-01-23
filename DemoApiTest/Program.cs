using Microsoft.VisualStudio.Threading;
using NLog;
using NLog.Config;
using NLog.Targets;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.TypeConverters;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DemoApiTest
{
    class Program
    {
        private const string WEBSOCKETURL = "wss://node01.dotmog.com";

        private static async Task Main(string[] args)
        {
            var config = new LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new FileTarget("logfile")
            {
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

            // [Plain] Value: u64
            //var reqResult = await client.GetStorageAsync("DotMogModule", "OwnedMogwaisCount", Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM")), cancellationToken);
            //var reqResult = await client.GetStorageAsync("DotMogModule", "OwnedMogwaisArray", new string[] { Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM")), "2" } , cancellationToken);

            // [Map] Key: u64, Hasher: Blake2_128Concat, Value: T::Hash
            //var reqResult = await client.GetStorageAsync("DotMogModule", "AllMogwaisArray", "0", cancellationToken);

            // [Map] Key: T::Hash, Hasher: Identity, Value: Optional<T::AccountId>
            //var reqResult = await client.GetStorageAsync("Dmog", "MogwaiOwner", "0xAD35415CB5B574819C8521B9192FFFDA772C0770FED9A55494293B2D728F104C", cancellationToken);

            // [Map] Key: T::Hash, Hasher: Identity, Value: MogwaiStruct<T::Hash, BalanceOf<T>>
            //var reqResult = await client.GetStorageAsync("DotMogModule", "Mogwais", "0x17E26CA749780270EEC18507AB3C03854E75E264DB13EC1F90314C3AF02CCDF8", cancellationToken);

            //var reqResult = await client.GetMethodAsync<JArray>("system_peers", cancellationToken);

            // [Map] Key: T::AccountId, Hasher: Blake2_128Concat, Value: AccountInfo<T::Index, T::AccountData>
            //var reqResult = await client.GetStorageAsync("System", "Account", Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5FfzQe73TTQhmSQCgvYocrr6vh1jJXEKB8xUB6tExfpKVCEZ")), cancellationToken);
            //var reqResult = await client.GetStorageAsync("System", "Account", Utils.Bytes2HexString(Utils.HexToByteArray("0xD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D")), cancellationToken);
            //var reqResult = await client.GetStorageAsync("System", "Account", Utils.Bytes2HexString(accountZurich.PublicKey), cancellationToken);

            // 455455
            // 0x98d7f5fe3efd88cd28d928c418c9ddc8dee254a2e11925a1a78b2ca6c2aac6d5
            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0x98d7f5fe3efd88cd28d928c418c9ddc8dee254a2e11925a1a78b2ca6c2aac6d5"), cancellationToken);
            //var reqResult = await client.Chain.GetBlockHashAsync(new BlockNumber(455455), cancellationToken);

            // 486587
            // 0x387b43b09e88adc971bfc64fdd8e84dcfd0c4dcfe5f30c6b7444bf3ad3717445
            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0x387b43b09e88adc971bfc64fdd8e84dcfd0c4dcfe5f30c6b7444bf3ad3717445"), cancellationToken);

            // 792,193 ---> 0x0cf64c1e0e45b2fba6fd524e180737f5e1bb46e0691783d6963b2e26253f8592 Create Mogwai
            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0x0cf64c1e0e45b2fba6fd524e180737f5e1bb46e0691783d6963b2e26253f8592"), cancellationToken);

            // 797,188 --> 0x7c0c2cb4f04487f9914e0d910c3a0f3bf1292a39f131ddb44947b3dd04b8c154 Balances Transfer
            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0x7c0c2cb4f04487f9914e0d910c3a0f3bf1292a39f131ddb44947b3dd04b8c154"), cancellationToken);

            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0xe7b99ee484e6369dd3c2a66d6306bffde5048ddf2090e990faae83e66f5275f4"), cancellationToken);

            // 489070
            // 0x76d50aa9a8cf86f7c1e5b40c2a02607dc63e3a3fc1077f7172280b443b16252d
            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0x76d50aa9a8cf86f7c1e5b40c2a02607dc63e3a3fc1077f7172280b443b16252d"), cancellationToken);

            //var reqResult = await client.Chain.GetHeaderAsync(new Hash("0x76d50aa9a8cf86f7c1e5b40c2a02607dc63e3a3fc1077f7172280b443b16252d"), cancellationToken);

            //var reqResult = await client.Chain.GetHeaderAsync(new Hash("0x0cf64c1e0e45b2fba6fd524e180737f5e1bb46e0691783d6963b2e26253f8592"), cancellationToken);

            //var reqResult = await client.GetMethodAsync<JArray>("author_pendingExtrinsics", cancellationToken);

            // *************************** Final Test
            //var reqResult = await client.Author.SubmitExtrinsicAsync(DotMogCall.CreateMogwai(), accountZurich, 0, 64, cancellationToken);
            //var reqResult = await client.Author.PendingExtrinsicAsync(cancellationToken);
            //var reqResult = await client.Author.SubmitExtrinsicAsync(ExtrinsicCall.BalanceTransfer("5GX1FSLUkzeUxdRPHrmc3hm8189WT2qQRbWUgy5vhZwgd2XQ", 9999), accountZurich, 0, 64, cancellationToken);

            // *** subscription test 1
            //var subscriptionId = await client.Chain
            //    .SubscribeAllHeadsAsync(
            //    (subscriptionId, header) => Console.WriteLine($"CallBack[{subscriptionId}]: {header}"), 
            //    cancellationToken
            //);
            //Thread.Sleep(30000);
            //var reqResult = await client.Chain.UnsubscribeAllHeadsAsync(subscriptionId, cancellationToken);

            // *** subscription test 2
            //Action<string, ExtrinsicStatus> actionExtrinsicUpdate = (subscriptionId, extrinsicUpdate) => Console.WriteLine($"CallBack[{subscriptionId}]: {extrinsicUpdate}");
            //var subscriptionId = await client.Author.SubmitAndWatchExtrinsicAsync(actionExtrinsicUpdate, ExtrinsicCall.BalanceTransfer("5GX1FSLUkzeUxdRPHrmc3hm8189WT2qQRbWUgy5vhZwgd2XQ", 1234), accountZurich, 0, 64, cancellationToken);
            //Thread.Sleep(60000);
            //var reqResult = await client.Author.UnwatchExtrinsicAsync(subscriptionId, cancellationToken);

            // *** subscription test 3
            //var subscriptionId = await client.State.SubscribeStorageAsync(
            //    (subscriptionId, anything) => Console.WriteLine($"CallBack[{subscriptionId}]: {anything}"), 
            //    cancellationToken
            //);
            //Thread.Sleep(60000);
            //var reqResult = await client.State.UnsubscribeStorageAsync(subscriptionId, cancellationToken);

            // *** subscription test 4 event subscription
            Action<string, StorageChangeSet> callBackSubscribeStorage = (subscriptionId, eventObject) => Console.WriteLine($"CallBack[{subscriptionId}]: {eventObject}");
            var systemEventsKeys = await client.GetStorageKeysAsync("System", "Events", CancellationToken.None);
            var subscriptionId = await client.State.SubscribeStorageAsync(systemEventsKeys,
               callBackSubscribeStorage
            );
            Thread.Sleep(60000);
            var reqResult = await client.State.UnsubscribeStorageAsync(subscriptionId, cancellationToken);

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