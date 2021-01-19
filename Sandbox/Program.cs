﻿using Newtonsoft.Json.Linq;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel.Calls;
using SubstrateNetApi.MetaDataModel.Extrinsics;
using SubstrateNetApi.MetaDataModel.Values;
using SubstrateNetApi.TypeConverters;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {
        private const string WEBSOCKETURL = "wss://node01.dotmog.com";

        private static async Task Main(string[] args)
        {
            await EventhandlingTest(args);
        }

        private static async Task EventhandlingTest(string[] args)
        {
            Console.WriteLine(Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM").Length);
            using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
            client.RegisterTypeConverter(new MogwaiStructTypeConverter());
            await client.ConnectAsync(CancellationToken.None);

            Action<string, JObject> callBackSubscribeStorage = (subscriptionId, eventObject) =>
            {
                if (eventObject.TryGetValue("changes", out JToken value))
                {
                    var strArray = value.ToObject<JArray>();
                    var eventRecord = EventRecords.Decode(strArray[0][1].ToString(), client.MetaData);
                    Console.WriteLine(eventRecord.ToString());
                }

                
            };

            var systemEventsKeys = await client.GetStorageKeysAsync("System", "Events", CancellationToken.None);

            var subscriptionId = await client.State.SubscribeStorageAsync(systemEventsKeys,
               callBackSubscribeStorage
            );

            Thread.Sleep(60000);

            var reqResult = await client.State.UnsubscribeStorageAsync(subscriptionId, CancellationToken.None);
        }

        private static async Task TestAsync(string[] args)
        {
            Console.WriteLine(Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM").Length);
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
            Account accountZurich = new Account(
                KeyType.ED25519,
                Utils.HexToByteArray("0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e"),
                Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM"));

            var extrinsic = ExtrinsicCall.BalanceTransfer(new AccountId(Utils.GetPublicKeyFrom("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH")), new Balance(123000000000000));


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
            int m;

            // length
            Console.WriteLine($"length = {CompactInteger.Decode(memory.ToArray(), ref p)}");

            Console.WriteLine($"next? = {CompactInteger.Decode(memory.ToArray(), ref p)}");

            Console.WriteLine($"p = {p}");

        }
    
    
        private async static Task RunBlockCalls(CancellationToken cancellationToken)
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
                var blockNumber = new BlockNumber(i);
                Console.WriteLine(blockNumber.Encode());

                Console.WriteLine(Utils.Bytes2HexString(blockNumber.Encode()));

                var blockHash = await client.Chain.GetBlockHashAsync(new BlockNumber(i), cancellationToken);

                //var block = await client.Chain.GetBlockAsync(blockHash, cancellationToken);

                // Print result
                //Console.WriteLine($"{i} --> {block.Block.Extrinsics.Length}");
                Console.WriteLine($"{i} --> {blockHash.HexString}");
            }
            //Console.WriteLine(client.MetaData.Serialize());

            Console.ReadKey();

            // Close connection
            await client.CloseAsync(cancellationToken);
        }
    }
}
