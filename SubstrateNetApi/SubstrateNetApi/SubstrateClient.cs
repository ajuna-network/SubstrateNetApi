using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using NLog;
using StreamJsonRpc;

namespace SubstrateNetApi
{
    public class SubstrateClient : IDisposable
    {
        private static ILogger Logger = LogManager.GetCurrentClassLogger();
        private Uri uri;

        private readonly ClientWebSocket socket;

        private JsonRpc jsonRpc;

        private CancellationTokenSource _connectTokenSource;
        private CancellationTokenSource _requestTokenSource;

        public MetaData MetaData { get; private set; }

        public SubstrateClient(Uri uri)
        {
            this.uri = uri;
            this.socket = new ClientWebSocket();
        }

        public async Task ConnectAsync()
        {
            await ConnectAsync(CancellationToken.None);
        }

        public async Task ConnectAsync(CancellationToken token)
        {
            _connectTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, _connectTokenSource.Token);
            await socket.ConnectAsync(uri, linkedTokenSource.Token);
            linkedTokenSource.Dispose();
            _connectTokenSource.Dispose();
            _connectTokenSource = null;
            Logger.Debug("Connected to Websocket.");
            
            jsonRpc = new JsonRpc(new WebSocketMessageHandler(socket));
            jsonRpc.StartListening();
            Logger.Debug("Listening to websocket.");

            _requestTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_requestTokenSource.Token, token);
            var result = await jsonRpc.InvokeWithCancellationAsync<string>("state_getMetadata", null, linkedTokenSource.Token);
            linkedTokenSource.Dispose();
            _requestTokenSource.Dispose();
            _requestTokenSource = null;

            var metaDataParser = new MetaDataParser(uri.OriginalString, result);
            MetaData = metaDataParser.MetaData;
            Logger.Debug("MetaData parsed.");
        }

        public async Task<object> RequestAsync(string moduleName, string itemName)
        {
            return await RequestAsync(moduleName, itemName, null, CancellationToken.None);
        }

        public async Task<object> RequestAsync(string moduleName, string itemName, CancellationToken token)
        {
            return await RequestAsync(moduleName, itemName, null, token);
        }

        public async Task<object> RequestAsync(string moduleName, string itemName, string parameter)
        {
            return await RequestAsync(moduleName, itemName, parameter, CancellationToken.None);
        }

        public async Task<object> RequestAsync(string moduleName, string itemName, string parameter, CancellationToken token)
        {
            if (socket.State != WebSocketState.Open)
            {
                throw new Exception($"WebSocketState is not open! Currently {socket.State}!");
            }

            object result;
            if (MetaData.TryGetModuleByName(moduleName, out Module module) && module.Storage.TryGetStorageItemByName(itemName, out Item item))
            {
                string method = "state_getStorage";

                if (item.Function?.Key1 != null && parameter == null)
                {
                    throw new Exception($"{moduleName}.{itemName} needs a parameter oof type '{item.Function?.Key1}'!");
                }

                string parameters;
                if (item.Function?.Key1 != null)
                {
                    byte[] key1Bytes = Utils.KeyTypeToBytes(item.Function.Key1, parameter);
                    parameters = "0x" + RequestGenerator.GetStorage(module, item, key1Bytes);
                }
                else
                {
                    parameters = "0x" + RequestGenerator.GetStorage(module, item);
                }

                string value = item.Function.Value;
                Logger.Debug($"Invoking request[{method}, params: {parameters}, value: {value}] {MetaData.Origin}");

                _requestTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, _requestTokenSource.Token);
                var resultString = await jsonRpc.InvokeWithParameterObjectAsync<string>(method, new object[] { parameters }, linkedTokenSource.Token);
                linkedTokenSource.Dispose();
                _requestTokenSource.Dispose();
                _requestTokenSource = null;

                byte[] bytes = Utils.HexToByteArray(resultString);
                Logger.Debug($"=> {resultString} [{bytes.Length}]");

                switch (value)
                {
                    case "u16":
                        result = BitConverter.ToUInt16(bytes, 0);
                        break;
                    case "u32":
                        result = BitConverter.ToUInt32(bytes, 0);
                        break;
                    case "u64":
                        result = BitConverter.ToUInt64(bytes, 0);
                        break;
                    case "T::AccountId":
                        result = new AccountId(resultString);
                        break;
                    case "T::Hash":
                        result = new Hash(resultString);
                        break;
                    case "MogwaiStruct<T::Hash, BalanceOf<T>>":
                        result = new MogwaiStruct(resultString);
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

        public async Task CloseAsync()
        {
            await CloseAsync(CancellationToken.None);
        }

        public async Task CloseAsync(CancellationToken token)
        {
            _connectTokenSource?.Cancel();
            _requestTokenSource?.Cancel();

            if (socket.State == WebSocketState.Open)
            {
                var closeTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(closeTokenSource.Token, token);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", linkedTokenSource.Token);
                linkedTokenSource.Dispose();
                closeTokenSource.Dispose();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    new JoinableTaskFactory(new JoinableTaskContext()).Run(CloseAsync);
                    _connectTokenSource?.Dispose();
                    _requestTokenSource?.Dispose();
                    jsonRpc?.Dispose();
                    socket?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SubstrateClient()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
