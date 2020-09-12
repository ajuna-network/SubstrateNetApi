using Microsoft.VisualStudio.Threading;
using NLog;
using StreamJsonRpc;
using SubstrateNetApi.Exceptions;
using SubstrateNetApi.MetaDataModel;
using SubstrateNetApi.TypeConverters;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System = SubstrateNetApi.Modules.System;
using SubstrateNetApi.MetaDataModel.Values;

[assembly: InternalsVisibleTo("SubstrateNetApiTests")]

namespace SubstrateNetApi
{
    public class SubstrateClient : IDisposable
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly Uri _uri;

        private ClientWebSocket _socket;

        private JsonRpc _jsonRpc;

        private CancellationTokenSource _connectTokenSource;
        private CancellationTokenSource _requestTokenSource;

        private readonly Dictionary<string, ITypeConverter> _typeConverters = new Dictionary<string, ITypeConverter>();

        public MetaData MetaData { get; private set; }
        public Modules.System System { get; }

        public Modules.Chain Chain { get; }

        public SubstrateClient(Uri uri)
        {
            _uri = uri;
            System = new Modules.System(this);
            Chain = new Modules.Chain(this);
            RegisterTypeConverter(new U16TypeConverter());
            RegisterTypeConverter(new U32TypeConverter());
            RegisterTypeConverter(new U64TypeConverter());
            RegisterTypeConverter(new AccountIdTypeConverter());
            RegisterTypeConverter(new HashTypeConverter());
            RegisterTypeConverter(new AccountInfoConverter());
        }

        public void RegisterTypeConverter(ITypeConverter converter)
        {
            if (_typeConverters.ContainsKey(converter.TypeName))
                throw new ConverterAlreadyRegisteredException("Converter for specified type already registered.");

            _typeConverters.Add(converter.TypeName, converter);
        }

        public bool IsConnected => _socket?.State == WebSocketState.Open;

        public async Task ConnectAsync()
        {
            await ConnectAsync(CancellationToken.None);
        }

        public async Task ConnectAsync(CancellationToken token)
        {
            if (_socket != null && _socket.State == WebSocketState.Open)
                return;

            if (_socket == null || _socket.State != WebSocketState.None)
            {
                _jsonRpc?.Dispose();
                _socket?.Dispose();
                _socket = new ClientWebSocket();
            }

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
            return await GetStorageAsync(moduleName, itemName, CancellationToken.None);
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
            if (_socket?.State != WebSocketState.Open)
                throw new ClientNotConnectedException($"WebSocketState is not open! Currently {_socket?.State}!");

            if (!MetaData.TryGetModuleByName(moduleName, out Module module) || !module.TryGetStorageItemByName(itemName, out Item item))
                throw new MissingModuleOrItemException($"Module '{moduleName}' or Item '{itemName}' missing in metadata of '{MetaData.Origin}'!");

            string method = "state_getStorage";

            if (item.Function?.Key1 != null && parameter == null)
                throw new MissingParameterException($"{moduleName}.{itemName} needs a parameter of type '{item.Function?.Key1}'!");

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

            var resultString = await InvokeAsync<string>(method, new object[] {parameters}, token);

            string returnType = item.Function?.Value;

            if (!_typeConverters.ContainsKey(returnType))
                throw new MissingConverterException($"Unknown type '{returnType}' for result '{resultString}'!");

            return _typeConverters[returnType].Create(resultString);
        }

        public async Task<T> GetMethodAsync<T>(string method)
        {
            return await GetMethodAsync<T>(method, CancellationToken.None);
        }

        public async Task<T> GetMethodAsync<T>(string method, CancellationToken token)
        {
            return await InvokeAsync<T>(method, null, token);
        }

        public async Task<T> GetMethodAsync<T>(string method, string parameter, CancellationToken token)
        {
            return await InvokeAsync<T>(method, new object[] { parameter }, token);
        }       
        public async Task<object> SubmitExtrinsicAsync(string moduleName, string callName, string parameter, byte[] pubKey, byte[] priKey, CancellationToken token)
        {
            if (_socket?.State != WebSocketState.Open)
                throw new ClientNotConnectedException($"WebSocketState is not open! Currently {_socket?.State}!");

            if (!MetaData.TryGetModuleByName(moduleName, out Module module) || !module.TryGetCallByName(callName, out Call call))
                throw new MissingModuleOrItemException($"Module '{moduleName}' or Item '{callName}' missing in metadata of '{MetaData.Origin}'!");

            string method = "author_submitExtrinsic";

            if (call.Arguments?.Length > 0 && parameter == null)
                throw new MissingParameterException($"{moduleName}.{callName} needs {call.Arguments.Length} parameter(s)!");

            var accountInfoString = await GetStorageAsync("System", "Account", Utils.Bytes2HexString(pubKey), token);
            
            var accountInfo = new AccountInfo((string)accountInfoString);

            var parameters = "0x" + RequestGenerator.SubmitExtrinsic(MetaData.IndexOf(module), module.IndexOf(call), parameter, accountInfo.Nonce, pubKey, priKey);

            var resultString = await InvokeAsync<string>(method, new object[] { parameters }, token);

            //if (!_typeConverters.ContainsKey(returnType))
            //    throw new MissingConverterException($"Unknown type '{returnType}' for result '{resultString}'!");

            return resultString; // _typeConverters[returnType].Create(resultString);
        }

        internal async Task<T> InvokeAsync<T>(string method, object parameters, CancellationToken token)
        {
            if (_socket?.State != WebSocketState.Open)
                throw new ClientNotConnectedException($"WebSocketState is not open! Currently {_socket?.State}!");

            Logger.Debug($"Invoking request[{method}, params: {parameters}] {MetaData.Origin}");

            _requestTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, _requestTokenSource.Token);
            var resultString = await _jsonRpc.InvokeWithParameterObjectAsync<T>(method, parameters, linkedTokenSource.Token);
            linkedTokenSource.Dispose();
            _requestTokenSource.Dispose();
            _requestTokenSource = null;
            return resultString;
        }

        public async Task CloseAsync()
        {
            await CloseAsync(CancellationToken.None);
        }

        public async Task CloseAsync(CancellationToken token)
        {
            _connectTokenSource?.Cancel();
            _requestTokenSource?.Cancel();

            if (_socket != null && _socket.State == WebSocketState.Open)
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
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
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

                _disposedValue = true;
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
