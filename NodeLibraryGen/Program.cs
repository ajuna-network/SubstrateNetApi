using SubstrateNetApi;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.Modules;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NodeLibraryGen
{
    class Program
    {
        private const string Websocketurl = "ws://127.0.0.1:9944";

        static async Task Main(string[] args)
        {
            //using var client = new SubstrateClient(new Uri(Websocketurl));
            //await client.ConnectLightAsync(CancellationToken.None);
            //var result = await client.State.GetMetaDataAsync(CancellationToken.None);

            string result = File.ReadAllText("metadata.txt");

            var mdv14 = new RuntimeMetadata();
            mdv14.Create(result);

            //Console.WriteLine(mdv14);

            foreach(var type in mdv14.RuntimeMetadataData.Types.Value)
            {
                Console.WriteLine(type.Id);
            }

        }
    }
}
