using Microsoft.VisualStudio.Threading;
using NLog;
using StreamJsonRpc;
using SubstrateNetApi.MetaDataModel;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SubstrateNetApi
{
    public class SubstrateClient : IDisposable
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly Uri _uri;

        private readonly ClientWebSocket _socket;

        private JsonRpc _jsonRpc;

        private CancellationTokenSource _connectTokenSource;
        private CancellationTokenSource _requestTokenSource;

        public MetaData MetaData { get; private set; }

        public SubstrateClient(Uri uri)
        {
            _uri = uri;
            _socket = new ClientWebSocket();
        }

        public async Task ConnectAsync()
        {
            await ConnectAsync(CancellationToken.None);
        }

        public async Task ConnectAsync(CancellationToken token)
        {
            _connectTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, _connectTokenSource.Token);
            await _socket.ConnectAsync(_uri, linkedTokenSource.Token);
            linkedTokenSource.Dispose();
            _connectTokenSource.Dispose();
            _connectTokenSource = null;
            Logger.Debug("Connected to Websocket.");

            _jsonRpc = new JsonRpc(new WebSocketMessageHandler(_socket));
            _jsonRpc.StartListening();
            Logger.Debug("Listening to websocket.");

            _requestTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_requestTokenSource.Token, token);
            var result = await _jsonRpc.InvokeWithCancellationAsync<string>("state_getMetadata", null, linkedTokenSource.Token);
            linkedTokenSource.Dispose();
            _requestTokenSource.Dispose();
            _requestTokenSource = null;

            var metaDataParser = new MetaDataParser(_uri.OriginalString, result);
            MetaData = metaDataParser.MetaData;
            Logger.Debug("MetaData parsed.");
        }

        public async Task<object> GetStorageAsync(string moduleName, string itemName)
        {
            return await GetStorageAsync(moduleName, itemName, null, CancellationToken.None);
        }

        public async Task<object> GetStorageAsync(string moduleName, string itemName, CancellationToken token)
        {
            return await GetStorageAsync(moduleName, itemName, null, token);
        }

        public async Task<object> GetStorageAsync(string moduleName, string itemName, string parameter)
        {
            return await GetStorageAsync(moduleName, itemName, parameter, CancellationToken.None);
        }

        public async Task<object> GetStorageAsync(string moduleName, string itemName, string parameter, CancellationToken token)
        {
            if (_socket.State != WebSocketState.Open)
            {
                throw new Exception($"WebSocketState is not open! Currently {_socket.State}!");
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

                string value = item?.Function?.Value;
                Logger.Debug($"Invoking request[{method}, params: {parameters}, value: {value}] {MetaData.Origin}");

                _requestTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, _requestTokenSource.Token);
                var resultString = await _jsonRpc.InvokeWithParameterObjectAsync<string>(method, new object[] { parameters }, linkedTokenSource.Token);
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

            if (_socket.State == WebSocketState.Open)
            {
                var closeTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(closeTokenSource.Token, token);
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close requested.", linkedTokenSource.Token);
                linkedTokenSource.Dispose();
                closeTokenSource.Dispose();
                Logger.Debug("Client closed.");
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
                    _jsonRpc?.Dispose();
                    _socket?.Dispose();
                    Logger.Debug("Client disposed.");
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
