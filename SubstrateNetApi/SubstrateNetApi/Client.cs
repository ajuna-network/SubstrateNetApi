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
    public class Client
    {
        private Uri uri;

        private readonly ClientWebSocket socket;

        private JsonRpc jsonRpc;

        private readonly CancellationTokenSource cts;

        public Client(Uri uri)
        {
            this.uri = uri;
            this.socket = new ClientWebSocket();
            this.cts = new CancellationTokenSource();
        }

        public void ConnectAsync()
        {

            var task = socket.ConnectAsync(new Uri("wss://boot.worldofmogwais.com"), cts.Token);
            task.Wait();
            jsonRpc = new JsonRpc(new WebSocketMessageHandler(socket));
            jsonRpc.StartListening();
        }

        public bool TryRequest(MetaData md, string v1, string v2, out object result)
        {
      
            if (md.TryGetModuleByName(v1, out Module module) && module.Storage.TryGetStorageItemByName(v2, out Item item))
            {
                string method = "state_getStorage";
                string parameters = "0x" + RequestGenerator.GetStorage(module, item);
                string value = item.Function.Value;

                Console.WriteLine($"Invoking request[{method}, params: {parameters}, value: {value}");
                var resultString = RequestWithParamtersAsync(method, parameters);

                switch (value)
                {
                    case "u16":
                        result = BitConverter.ToUInt16(Utils.HexToByteArray(resultString),0);
                        return true;
                    case "u32":
                        result = BitConverter.ToUInt32(Utils.HexToByteArray(resultString), 0);
                        return true;
                    case "u64":
                        result = BitConverter.ToUInt64(Utils.HexToByteArray(resultString), 0);
                        return true;
                    case "T::AccountId":
                        result = new AccountId(resultString);
                        return true;
                    default:
                        throw new Exception($"Unknown type '{value}' for result '{resultString}'!");
                }
            } 
            else
            {
                throw new Exception($"Module '{v1}' or Item '{v2}' missing in metadata of '{md.Origin}'!");
            }
        }

        public string RequestAsync(string methode)
        {

            var task = jsonRpc.InvokeWithCancellationAsync<string>(methode, null, cts.Token);
            task.Wait();
            return task.Result;

        }

        public string RequestWithParamtersAsync(string methode, string parameter)
        {

            var task = jsonRpc.InvokeWithParameterObjectAsync<string>(methode, new object[] { parameter }, cts.Token);
            task.Wait();
            return task.Result;

        }

        public void Disconnect()
        {
            cts.Cancel();
        }
    }
}
