using SubstrateNetApi;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.TypeConverters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SubstrateNetApi.Model.Rpc;

namespace Sandbox
{
    class Program
    {
        private const string WEBSOCKETURL = "wss://mogiway-02.dotmog.com";

        private static async Task Main(string[] args)
        {
            //await ParseEventStringAsync(args);
            //await GetKeysPagedAsync(args);
            //TestKey(args);
            await StorageRuntimeVersion(args);

        }

        //

        private static async Task StorageRuntimeVersion(string[] args)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            var reqResult = await client.State.GetRuntimeVersionAsync();

            Console.WriteLine(reqResult);
        }

        private static void TestKey(string[] args)
        {
            var accountId = new AccountId();
            accountId.Create(Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM"));
            Console.WriteLine($"AccountId: {accountId}");
            Console.WriteLine($"Public Key: {Utils.Bytes2HexString(accountId.Bytes).ToLower()}");

            var str = "0x200101020304050607";

            var memory = Utils.HexToByteArray(str).AsMemory();
            List<U8> vecU8 = new List<U8>();
            var byteArray = memory.ToArray();
            var p = 0;
            var length = CompactInteger.Decode(byteArray, ref p);
            Console.WriteLine($"Length: {length}, p = {p}");
            for (var i = 0; i < length; i++)
            {
                var u8 = new U8();
                u8.Decode(byteArray, ref p);
                vecU8.Add(u8);
            }

            Console.WriteLine(JsonConvert.SerializeObject(vecU8));
        }

        private static async Task GetKeysPagedAsync(string[] args)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            var keys = await client.State.GetKeysPagedAsync(Utils.HexToByteArray("0x0c97543ac4e96dcb4706b30bdf6e92168b0d930c4954153694987c34a9823bbd"), 3,
                Utils.HexToByteArray("0x0c97543ac4e96dcb4706b30bdf6e92168b0d930c4954153694987c34a9823bbd048abc25e90483f36fa94d713b9bf2ea2aff9a42ef458696d3f8e340de66def4692856a8f51e1435f5d5df755a138c51"), CancellationToken.None);

            Console.WriteLine($"Key: {keys}");
        }

        private static async Task GetPairsAsync(string[] args)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            var keys = await client.State.GetPairsAsync(RequestGenerator.GetStorageKeyBytesHash("DotMogModule", "OwnedMogwaisCount"), CancellationToken.None);

            Console.WriteLine($"Key: {keys}");
        }

        private static async Task GetAllMogwaiHashs(string[] args)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            var keys = await client.GetStorageKeysAsync("DotMogModule", "Mogwais", CancellationToken.None);
            var keyString = Utils.Bytes2HexString(RequestGenerator.GetStorageKeyBytesHash("DotMogModule", "Mogwais")).ToLower();
            Console.WriteLine($"Key: {keyString}");
            foreach (var key in keys)
            {
                Console.WriteLine(key.ToString().Replace(keyString, ""));
            }
        }

        private static async Task AccountSubscriptionAsync(string[] args)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            Action<string, StorageChangeSet> callBackAccountChange = (subscriptionId, eventObject) =>
            {
                Console.WriteLine($"Subscription[{subscriptionId}]: {eventObject}");
                if (eventObject.Changes != null)
                {
                    try
                    {
                        var accountInfo = new AccountInfo(eventObject.Changes[0][1].ToString());
                        Console.WriteLine(accountInfo);
                    }
                    catch (NotImplementedException e)
                    {
                        Console.WriteLine($"##### {e}");
                    }
                }
            };

            var subscriptionId = await client.SubscribeStorageKeyAsync("System", "Account",
                new string[] { Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH")) },
                callBackAccountChange, CancellationToken.None);

            Thread.Sleep(60000);

            var reqResultUnsubscribe = await client.State.UnsubscribeStorageAsync(subscriptionId, CancellationToken.None);
        }

        private static async Task AllMogwaisCountSubscriptionAsync(string[] args)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            var subscriptionId = await client.SubscribeStorageKeyAsync("DotMogModule", "AllMogwaisCount", null,
                (id, storageChangeSet) => {
                    foreach (var change in storageChangeSet.Changes)
                    {
                        int p = 0;
                        var u64 = new U64();
                        u64.Decode(Utils.HexToByteArray(change[1]), ref p);
                        var result = u64.Value;
                        Console.WriteLine($"AllMogwaisCount = {result}");
                    }
                }, CancellationToken.None);

            Thread.Sleep(60000);

            var reqResultUnsubscribe = await client.State.UnsubscribeStorageAsync(subscriptionId, CancellationToken.None);
        }

        private static async Task OwnedMogwaisCountSubscriptionAsync(string[] args)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            var reqResult = await client.GetStorageAsync("DotMogModule", "OwnedMogwaisCount",
                new string[] { Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH")) },
                CancellationToken.None);

            Console.WriteLine($"DotMogModule.OwnedMogwaisCount = {reqResult}");

            Action<string, StorageChangeSet> callBackSubscribeStorage = (subscriptionId, eventObject) =>
            {
                Console.WriteLine($"Subscription[{subscriptionId}]: {eventObject}");
            };
            
            var keys = await client.GetStorageKeysAsync("DotMogModule", "OwnedMogwaisCount", CancellationToken.None);

            //var subscriptionId = await client.SubscribeStorageKeyAsync("DotMogModule", "OwnedMogwaisCount", new string[] { Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH")) },
            var subscriptionId = await client.State.SubscribeStorageAsync(keys,
               callBackSubscribeStorage,
               CancellationToken.None
            );

            Thread.Sleep(60000);

            var reqResultUnsubscribe = await client.State.UnsubscribeStorageAsync(subscriptionId, CancellationToken.None);
        }

        private static async Task ParseEventsAsync(string[] args)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var module in client.MetaData.Modules)
            {
                if (module.Events == null)
                {
                    continue;
                }

                foreach (var singleEvent in module.Events)
                {

                    if (singleEvent.EventArgs == null)
                    {
                        continue;
                    }

                    foreach (var eventArg in singleEvent.EventArgs)
                    {
                        if (!dict.ContainsKey(eventArg))
                        {
                            dict.Add(eventArg, $"{module.Name}.{singleEvent.Name}");
                        }
                    }
                }
            }

            foreach (var keyValue in dict)
            {
                Console.WriteLine($"case \"{keyValue.Key}\":\ndata.Add(({keyValue.Key}){keyValue.Key}.Decode(byteArray, ref p));\nbreak;");
            }
        }

        private static async Task EventhandlingTestAsync(string[] args)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            Action<string, StorageChangeSet> callBackSubscribeStorage = (subscriptionId, eventObject) =>
            {
                if (eventObject.Changes != null)
                {
                    try
                    {
                        var eventRecord = EventRecords.Decode(eventObject.Changes[0][1].ToString(), client.MetaData);
                        Console.WriteLine(eventRecord.ToString());
                    }
                    catch (NotImplementedException e)
                    {
                        Console.WriteLine($"##### {e}");
                    }
                }


            };

            var systemEventsKeys = await client.GetStorageKeysAsync("System", "Events", CancellationToken.None);

            var subscriptionId = await client.State.SubscribeStorageAsync(systemEventsKeys,
               callBackSubscribeStorage
            );

            Thread.Sleep(1200000);

            var reqResult = await client.State.UnsubscribeStorageAsync(subscriptionId, CancellationToken.None);
        }

        private static async Task TestAsync(string[] args)
        {
            //Console.WriteLine(Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM").Length);
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            var eventStr = "0x14" +
                "00" + // ******************* EVENT 1 begin
                "00000000" + // phase {ApplyExtrinsic: 0 u32}
                "0000" + // event { index: 0x0000
                "482d7c0900000000" + // weight: 159,133,000
                "02" + // class: Mandatory
                "00" + // paysFee: Yes
                "00" + // ******************* EVENT 1 end
                "00" + // ******************* EVENT 2 begin
                "01000000" + // phase {ApplyExtrinsic: 1 u32}
                "0000" + // event { index: 0x0000
                "0000000000000000" + // weight: 0,
                "02" + // class: Mandatory,
                "00" + // paysFee: Yes
                "00" + // ******************* EVENT 2 end
                "00" + // ******************* EVENT 3 begin
                "02000000" + // phase {ApplyExtrinsic: 1 u32}
                "2002" + // event { index: 0x2002
                "4d2b23d27e1f6e3733d7ebf3dc04f3d5d0010cd18038055f9bbbab48f460b61e" + // public-key
                "87a1395e8b61d529e7684d80373d52a23dd5de84061ab0a980ecbbcb3364457b" + // mogwai-id
                "00" + // ******************* EVENT 3 end
                "00" + // ******************* EVENT 4 begin
                "02000000" + // phase {ApplyExtrinsic
                "1106" + // event { index:
                "2e6bb353c70000000000000000000000" +
                "00" + // ******************* EVENT 4 end
                "00" + // ******************* EVENT 5 begin
                "02000000" + // phase {ApplyExtrinsic
                "0000" + // event { index:
                "1008f60500000000" +
                "00" + // class: Mandatory,
                "00" + // paysFee: Yes
                "00"; // ******************* EVENT 4 begin
            ;

            var eventRecords = EventRecords.Decode(eventStr, client.MetaData);

            Console.WriteLine(eventRecords);
        }

        private static void ParseExtrinsic(string[] args)
        {
            // Reference Substrate 3.0.0
            // Zurich to DotMog, 0.123 DMOG's
            // Nonce 0, Lifetime 64
            // 0x4502
            // 8400
            // 278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e --> 5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM
            // 00a1486b48665121686eddf7029d4f3b2ccf9335824d91df1ff11ffa739756717fe5570f204596fbd27c893981883b25ac797d3935405580d6144b356b469d6709f5020000060000
            // 4d2b23d27e1f6e3733d7ebf3dc04f3d5d0010cd18038055f9bbbab48f460b61e --> 5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH
            // 0b00b04e2bde6f




            //    {
            //        isSigned: true,
            //        method:
            //        {
            //            args:
            //            [
            //            {
            //                Id: 5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH
            //            },
            //            1.0000 DMOG
            //                ],
            //            method: transfer,
            //            section: balances
            //        },
            //        era:
            //        {
            //            MortalEra:
            //            {
            //                period: 128,
            //                phase: 50
            //            }
            //        },
            //        nonce: 2,
            //        signature: 0x351b31c6ad373f176a020acf168ca0412a3f410ef6d9936f46f4ce7bc893f76fcf3ff4389de0f86ad6b55df9af0d8ae3b868a544df107698056b7a93202faf00,
            //        signer:
            //        {
            //            Id: 5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM
            //        },
            //        tip: 0
            //    }
            //    ]
            //},
            //0x4902
            //8400
            //278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e
            //00
            //351b31c6ad373f176a020acf168ca0412a3f410ef6d9936f46f4ce7bc893f76fcf3ff4389de0f86ad6b55df9af0d8ae3b868a544df107698056b7a93202faf00 --> signature
            //26030800060000
            //4d2b23d27e1f6e3733d7ebf3dc04f3d5d0010cd18038055f9bbbab48f460b61e
            //0f0080c6a47e8d03



            Account accountZurich = Account.Build(
                KeyType.ED25519,
                Utils.HexToByteArray("0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e"),
                Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM"));

            var accountId = new AccountId();
            accountId.Create(Utils.GetPublicKeyFrom("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH"));

            var balance = new Balance();
            balance.Create(2000000000000);

            var extrinsic = ExtrinsicCall.BalanceTransfer(accountId, balance);


            Console.WriteLine(CompactInteger.Decode(Utils.HexToByteArray("0x490284")).ToString());
            Console.WriteLine(Utils.Bytes2HexString(new CompactInteger(146).Encode()));

            // 0x4902 Length 146
            // 84
            // 00
            // 278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e --> 5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM
            // 00fb6ec6a0e127329b564367527b3d6c4f28c197d2e205a4d37270e7fe5eee764e1d678e46e2c2d55a1d2cfd7869d24e40ba5f6bd9827c0b95d3db51bc633d050445032400060000
            // 4d2b23d27e1f6e3733d7ebf3dc04f3d5d0010cd18038055f9bbbab48f460b61e --> 5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH
            // 0f00806d8176de18
        }

        private static void ParseExtrinsicVecHeader(string[] args)
        {
            //Console.WriteLine(Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5GX1FSLUkzeUxdRPHrmc3hm8189WT2qQRbWUgy5vhZwgd2XQ")));

            var hexString = "0x" +
                "0d03" + // length
                "04" +
                "040004" +
                "2ad9b32392b71a2e5fdd210d0be84c152a3bc2571b6223e9100c64bfdd7878b1" + // parentHash
                "b6d91d00" + // number
                "9eee9fd4cf123d08a4fe2f17a1b38ce1a0ded9083c840f3124c567943da29232" + // stateRoot
                "4054e5b8541167df941d22e69bd16ad3bb8e7307830b7bc8f74954388281f095" + // extrinsicsRoot
                                                                                     // digest
                "08064241424534" +
                "02060000002f6af41f00000000" +
                "05424142450101" +
                "2899b12dbf3b94d2fa11bc739a49d485d1528b573497749b72e36a3aaf191e61089289ae084fbbd417435e43d30bcdc3854c3b6fc1965a809f5f34e7682c9c8f";



            //        method:
            //    {
            //    args:
            //        [
            //      [
            //        {
            //        parentHash: 0x2ad9b32392b71a2e5fdd210d0be84c152a3bc2571b6223e9100c64bfdd7878b1,
            //        number: 489,069,
            //        stateRoot: 0x9eee9fd4cf123d08a4fe2f17a1b38ce1a0ded9083c840f3124c567943da29232,
            //        extrinsicsRoot: 0x4054e5b8541167df941d22e69bd16ad3bb8e7307830b7bc8f74954388281f095,
            //        digest:
            //            {
            //            logs:
            //                [
            //              {
            //                PreRuntime:[
            //                 BABE,
            //                 0x02060000002f6af41f00000000
            //                ]
            //            },
            //            {
            //                Seal:[
            //                 BABE,
            //                 0x2899b12dbf3b94d2fa11bc739a49d485d1528b573497749b72e36a3aaf191e61089289ae084fbbd417435e43d30bcdc3854c3b6fc1965a809f5f34e7682c9c8f
            //              ]
            //            }
            //          ]
            //        }
            //        }
            //    ]
            //  ],
            //  method: setUncles,
            //  section: authorship
            //}


            var memory = Utils.HexToByteArray(hexString).AsMemory();

            int p = 0;
            //int m;

            // length
            Console.WriteLine($"length = {CompactInteger.Decode(memory.ToArray(), ref p)}");

            Console.WriteLine($"next? = {CompactInteger.Decode(memory.ToArray(), ref p)}");

            Console.WriteLine($"p = {p}");

        }

        private static async Task RunBlockCallsAsync(CancellationToken cancellationToken)
        {
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));

            client.RegisterTypeConverter(new MogwaiStructTypeConverter());

            await client.ConnectAsync(cancellationToken);

            var systemName = await client.System.NameAsync(cancellationToken);
            var systemVersion = await client.System.VersionAsync(cancellationToken);
            var systemChain = await client.System.ChainAsync(cancellationToken);
            Console.WriteLine($"Connected to System: {systemName} Chain: {systemChain} Version: {systemVersion}.");
            // 544133 CreateMogwai();
            for (uint i = 0; i < 10; i++)
            {
                var blockNumber = new BlockNumber();
                blockNumber.Create(i);
                Console.WriteLine(blockNumber.Encode());

                Console.WriteLine(Utils.Bytes2HexString(blockNumber.Encode()));

                var blockHash = await client.Chain.GetBlockHashAsync(blockNumber, cancellationToken);

                //var block = await client.Chain.GetBlockAsync(blockHash, cancellationToken);

                // Print result
                //Console.WriteLine($"{i} --> {block.Block.Extrinsics.Length}");
                Console.WriteLine($"{i} --> {blockHash.Value}");
            }
            //Console.WriteLine(client.MetaData.Serialize());

            Console.ReadKey();

            // Close connection
            await client.CloseAsync(cancellationToken);
        }
    }
}
