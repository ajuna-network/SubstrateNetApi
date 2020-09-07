using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using StreamJsonRpc;

namespace SubstrateNetApi
{
    public class SubstrateClient
    {
        private Uri uri;

        private readonly ClientWebSocket socket;

        private JsonRpc jsonRpc;

        private CancellationTokenSource cts;

        public MetaData MetaData { get; private set; }

        public SubstrateClient(Uri uri)
        {
            this.uri = uri;
            this.socket = new ClientWebSocket();

        }

        public async Task ConnectAsync()
        {
            // create a new cancellation token
            cts = new CancellationTokenSource();

            await socket.ConnectAsync(uri, cts.Token);

            jsonRpc = new JsonRpc(new WebSocketMessageHandler(socket));
            jsonRpc.StartListening();

            var result = await jsonRpc.InvokeWithCancellationAsync<string>("state_getMetadata", null, cts.Token);

            var metaDataParser = new MetaDataParser(uri.OriginalString, result);
            MetaData = metaDataParser.MetaData;
        }

        public async Task<object> TryRequestAsync(string moduleName, string itemName)
        {
            if (socket.State != WebSocketState.Open)
            {
                throw new Exception($"WebSocketState is not open! Currently {socket.State}!");
            }

            object result;
            if (MetaData.TryGetModuleByName(moduleName, out Module module) && module.Storage.TryGetStorageItemByName(itemName, out Item item))
            {
                string method = "state_getStorage";
                string parameters = "0x" + RequestGenerator.GetStorage(module, item);
                string value = item.Function.Value;

                Console.WriteLine($"Invoking request[{method}, params: {parameters}, value: {value}");
                var resultString = await jsonRpc.InvokeWithParameterObjectAsync<string>(method, new object[] { parameters }, cts.Token);
                switch (value)
                {
                    case "u16":
                        result = BitConverter.ToUInt16(Utils.HexToByteArray(resultString), 0);
                        break;
                    case "u32":
                        result = BitConverter.ToUInt32(Utils.HexToByteArray(resultString), 0);
                        break;
                    case "u64":
                        result = BitConverter.ToUInt64(Utils.HexToByteArray(resultString), 0);
                        break;
                    case "T::AccountId":
                        result = new AccountId(resultString);
                        break;
                    default:
                        throw new Exception($"Unknown type '{value}' for result '{resultString}'!");
                }
            }
            else
            {
                throw new Exception($"Module '{moduleName}' or Item '{itemName}' missing in metadata of '{MetaData.Origin}'!");
            }

            return result;
        }

        public void Disconnect()
        {
            cts.Cancel();
        }
    }
}
