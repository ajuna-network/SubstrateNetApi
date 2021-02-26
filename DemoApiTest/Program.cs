using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using SubstrateNetApi;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.TypeConverters;

namespace DemoApiTest
{
    internal class Program
    {
        private const string Websocketurl = "wss://mogiway-01.dotmog.com";

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

        private static async Task MainAsync(CancellationToken cancellationToken)
        {
            var accountAlice = Account.Build(
                KeyType.Sr25519,
                Utils.HexToByteArray(
                    "0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011"),
                Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"));

            var accountZurich = Account.Build(
                KeyType.Ed25519,
                Utils.HexToByteArray(
                    "0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e"),
                Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM"));

            var accountDMOG_GALxeh = Account.Build(
                KeyType.Ed25519,
                Utils.HexToByteArray(
                    "0x3f997449154f8aaa134341b07c3710f63d57e73025105ca7e65a151d7fc3e2bf4b94e38b0c2ee21c367d4c9584204ce62edf5b4a6f675f10678cc56b6ea86e71"),
                Utils.GetPublicKeyFrom("5DmogGALxehCbUmm45XJoADcf9BU71ZK2zmqHDPFJD3VxknC"));

            using var client = new SubstrateClient(new Uri(Websocketurl));

            // add chain specific types here
            client.RegisterTypeConverter(new GenericTypeConverter<MogwaiStruct>());
            client.RegisterTypeConverter(new GenericTypeConverter<MogwaiBios>());
            client.RegisterTypeConverter(new GenericTypeConverter<GameEvent>());

            await client.ConnectAsync(cancellationToken);

            /***
             * Basic ...
             */

            //var reqResult = await client.System.NameAsync(cancellationToken);
            //var systemVersion = await client.System.VersionAsync(cancellationToken);
            //var systemChain = await client.System.ChainAsync(cancellationToken);
            //var reqResult = await client.State.GetRuntimeVersionAsync(cancellationToken);
            //Console.WriteLine($"Connected to System: {systemName} Chain: {systemChain} Version: {systemVersion}.");
            Console.WriteLine(
                $"Running: {client.RuntimeVersion.SpecName}[{client.RuntimeVersion.SpecVersion}] transaction_version: {client.RuntimeVersion.TransactionVersion}.");

            // TODO: Implement all rpc standard functions from substrate node
            //var reqResult = await client.GetMethodAsync<JArray>("system_peers", cancellationToken);

            /***
             * Testing storage data ...
             */
            var address = "5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH";
            var mogwaiId = "0xc6e023f423709bc1a955f2913ad71333e0563453ad0347d09c012bcd6590c8b5";
            var mogwaiIdGen1 = "0xe2d3965c287d92c7cf45dc3ff832e8060607cc8eb7f85ae598b4030338f59587";

            // [Plain] Value: T::AccountId
            var reqResult = await client.GetStorageAsync("Sudo", "Key", cancellationToken);

            // [Plain] Value: u64
            //var reqResult = await client.GetStorageAsync("DotMogModule", "AllMogwaisCount", cancellationToken);

            // [Plain] Value: u64
            //var reqResult = await client.GetStorageAsync("DotMogModule", "OwnedMogwaisCount", new [] {Utils.Bytes2HexString(Utils.GetPublicKeyFrom(address))}, cancellationToken);
            //var reqResult = await client.GetStorageAsync("DotMogModule", "OwnedMogwaisArray", new [] { Utils.Bytes2HexString(Utils.GetPublicKeyFrom(address)), "1" } , cancellationToken);

            // [Map] Key: u64, Hasher: BlakeTwo128Concat, Value: T::Hash
            //var reqResult = await client.GetStorageAsync("DotMogModule", "AllMogwaisArray", new[]{"0"}, cancellationToken);

            // [Map] Key: T::Hash, Hasher: Identity, Value: Optional<T::AccountId>
            //var reqResult = await client.GetStorageAsync("DotMogModule", "MogwaiOwner", new string[] { mogwaiId }, cancellationToken);

            // [Map] Key: T::Hash, Hasher: Identity, Value: MogwaiStruct<T::Hash, T::BlockNumber, BalanceOf<T>>
            //var reqResult = await client.GetStorageAsync("DotMogModule", "Mogwais", new [] {mogwaiId}, cancellationToken);

            // [Map] Key: T::Hash, Hasher: Identity, Value: MogwaiBios<T::Hash, T::BlockNumber, BalanceOf<T>>
            //var reqResult = await client.GetStorageAsync("DotMogModule", "MogwaisBios", new [] { mogwaiIdGen1 }, cancellationToken);

            // [Map] Key: T::AccountId, Hasher: BlakeTwo128Concat, Value: Vec<u8>
            //var reqResult = await client.GetStorageAsync("DotMogModule", "AccountConfig", 
            //    new[] { Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM")) }, cancellationToken);

            // [Map] Key: T::AccountId, Hasher: BlakeTwo128Concat, Value: AccountInfo<T::Index, T::AccountData>
            //var reqResult = await client.GetStorageAsync("System", "Account", new [] {Utils.Bytes2HexString(Utils.GetPublicKeyFrom(address))}, cancellationToken);

            // [Map] Key: T::Hash, Hasher: Identity, Value: MogwaiBios<T::Hash, T::BlockNumber, BalanceOf<T>>
            //var mogwaiIdEgg1 = "0x5129a01b84073771324030bec439dc9218c87a4d73631fbdd828867a8855babf";
            //var reqResult = await client.GetStorageAsync("DotMogModule", "GameEventsOfMogwai", new [] { mogwaiIdEgg1 }, cancellationToken);

            //var gameEventId = "0xb9e4454a9a5c1d4c75dc73c9932515456a4c7542e15b0b6bd57142b1336e562c";
            //var reqResult = await client.GetStorageAsync("DotMogModule", "GameEvents", new[] { gameEventId }, cancellationToken);

            //var hash = new Hash();
            //hash.Create("0x21E1FF2794042872FF8233AAC9D38F6D565BE8A197A112C366D3D40B1321204E");
            //var reqResult = await client.Chain.GetHeaderAsync(hash, cancellationToken);

            //var hash = new Hash();
            //hash.Create("0xf7ed3ff62195438d16616cae55cb450e1fdb6e8602190a335b0684fc50f33311");
            //var reqResult = await client.Chain.GetBlockAsync(hash, cancellationToken);

            // ****************************************************************************************************************************************

            // 455455
            // 0x98d7f5fe3efd88cd28d928c418c9ddc8dee254a2e11925a1a78b2ca6c2aac6d5
            //var reqResult = await client.Chain.GetBlockHashAsync(new BlockNumber(455455), cancellationToken);

            // 486587
            // 0x387b43b09e88adc971bfc64fdd8e84dcfd0c4dcfe5f30c6b7444bf3ad3717445
            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0x9b1c6c66107ced561edff29ec83d530ffbfb2d21ec326fef0fc8ffe60ee685f9"), cancellationToken);

            // 792,193 ---> 0x0cf64c1e0e45b2fba6fd524e180737f5e1bb46e0691783d6963b2e26253f8592 Create Mogwai
            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0x0cf64c1e0e45b2fba6fd524e180737f5e1bb46e0691783d6963b2e26253f8592"), cancellationToken);

            // 797,188 --> 0x7c0c2cb4f04487f9914e0d910c3a0f3bf1292a39f131ddb44947b3dd04b8c154 Balances Transfer
            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0x7c0c2cb4f04487f9914e0d910c3a0f3bf1292a39f131ddb44947b3dd04b8c154"), cancellationToken);

            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0xe7b99ee484e6369dd3c2a66d6306bffde5048ddf2090e990faae83e66f5275f4"), cancellationToken);

            // 489070
            // 0x76d50aa9a8cf86f7c1e5b40c2a02607dc63e3a3fc1077f7172280b443b16252d
            //var reqResult = await client.Chain.GetBlockAsync(new Hash("0x2a6fa42837069b0b41613855c667daf2fb5418dcdd915db6a0cac68810083296"), cancellationToken);

            //var reqResult = await client.Chain.GetHeaderAsync(new Hash("0x76d50aa9a8cf86f7c1e5b40c2a02607dc63e3a3fc1077f7172280b443b16252d"), cancellationToken);

            //var reqResult = await client.GetMethodAsync<JArray>("author_pendingExtrinsics", cancellationToken);

            // *** test 0 simple extrinsic tests
            //var reqResult = await client.Author.SubmitExtrinsicAsync(DotMogCall.CreateMogwai(), accountZurich, 0, 64, cancellationToken);
            //var reqResult = await client.Author.PendingExtrinsicAsync(cancellationToken);
            //var balanceTransfer = ExtrinsicCall.BalanceTransfer("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH", BigInteger.Parse("100000000000"));
            //var reqResult = await client.Author.SubmitExtrinsicAsync(balanceTransfer, accountZurich, 0, 64, cancellationToken);

            // *** test 1 new head subscription
            //var subscriptionId = await client.Chain
            //    .SubscribeAllHeadsAsync(
            //    (subscriptionId, header) => Console.WriteLine($"CallBack[{subscriptionId}]: {header}"), 
            //    cancellationToken
            //);
            //Thread.Sleep(30000);
            //var reqResult = await client.Chain.UnsubscribeAllHeadsAsync(subscriptionId, cancellationToken);

            // *** test 2 submit extrinsic
            //Action<string, ExtrinsicStatus> actionExtrinsicUpdate = (subscriptionId, extrinsicUpdate) => Console.WriteLine($"CallBack[{subscriptionId}]: {extrinsicUpdate}");
            //var subscriptionId = await client.Author.SubmitAndWatchExtrinsicAsync(actionExtrinsicUpdate, ExtrinsicCall.BalanceTransfer("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH", 100000000000), accountZurich, 0, 64, cancellationToken);
            //Thread.Sleep(60000);
            //var reqResult = await client.Author.UnwatchExtrinsicAsync(subscriptionId, cancellationToken);

            // *** test 3  full stoarge test
            // ???

            // *** test 4 event subscription
            //Action<string, StorageChangeSet> callBackSubscribeStorage = (subscriptionId, eventObject) => Console.WriteLine($"CallBack[{subscriptionId}]: {eventObject}");
            //var systemEventsKeys = await client.GetStorageKeysAsync("System", "Events", CancellationToken.None);
            //var subscriptionId = await client.State.SubscribeStorageAsync(systemEventsKeys,
            //   callBackSubscribeStorage
            //);
            //Thread.Sleep(60000);
            //var reqResult = await client.State.UnsubscribeStorageAsync(subscriptionId, cancellationToken);

            //Hash finalizedHead = await client.Chain.GetFinalizedHeadAsync(cancellationToken);
            //var reqResult = await client.Chain.GetBlockAsync(finalizedHead, cancellationToken);

            // Print result
            Console.WriteLine($"RESPONSE: '{reqResult}' [{reqResult?.GetType().Name}]");

            //Console.WriteLine(client.MetaData.Serialize());

            // Close connection
            await client.CloseAsync(cancellationToken);

            Console.ReadKey();
        }
    }
}