using SubstrateNetApi;
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
            var cts = new CancellationTokenSource();
            await RunBlockCalls(cts.Token);
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
