using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SubstrateNetApi.Model.Rpc;

namespace SubstrateNetApi.Modules
{
    /// <summary> A state. </summary>
    /// <remarks> 19.09.2020. </remarks>
    public class State
    {
        /// <summary> The client. </summary>
        private readonly SubstrateClient _client;

        /// <summary> Constructor. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="client"> The client. </param>
        internal State(SubstrateClient client)
        {
            _client = client;
        }

        public async Task<string> GetMetaDataAsync()
        {
            return await GetMetaDataAsync(CancellationToken.None);
        }

        public async Task<string> GetMetaDataAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<string>("state_getMetadata", null, token);
        }

        public async Task<JArray> GetPairsAsync(byte[] keyPrefix)
        {
            return await GetPairsAsync(keyPrefix, CancellationToken.None);
        }

        public async Task<JArray> GetPairsAsync(byte[] keyPrefix, CancellationToken token)
        {
            return await _client.InvokeAsync<JArray>("state_getPairs", new object[] {Utils.Bytes2HexString(keyPrefix)}, token);
        }

        public async Task<JArray> GetKeysPagedAsync(byte[] keyPrefix, uint pageCount, byte[] startKey)
        {
            return await GetKeysPagedAsync(keyPrefix, pageCount, startKey, CancellationToken.None);
        }

        public async Task<JArray> GetKeysPagedAsync(byte[] keyPrefix, uint pageCount, byte[] startKey,
            CancellationToken token)
        {
            return startKey.Length == 0
                ? await _client.InvokeAsync<JArray>("state_getKeysPaged",
                    new object[] {Utils.Bytes2HexString(keyPrefix), pageCount}, token)
                : await _client.InvokeAsync<JArray>("state_getKeysPaged",
                    new object[] {Utils.Bytes2HexString(keyPrefix), pageCount, Utils.Bytes2HexString(startKey)}, token);
        }

        public async Task<object> GetStorageAsync(byte[] parameters, CancellationToken token)
        {
            return await _client.InvokeAsync<object>("state_getStorage", new object[] { Utils.Bytes2HexString(parameters) }, token);
        }

        public async Task<object> GetStorageHashAsync(byte[] key, CancellationToken token)
        {
            return await _client.InvokeAsync<JArray>("state_getStorageHash", new object[] { Utils.Bytes2HexString(key) }, token);
        }

        public async Task<RuntimeVersion> GetRuntimeVersionAsync()
        {
            return await GetRuntimeVersionAsync(CancellationToken.None);
        }

        public async Task<RuntimeVersion> GetRuntimeVersionAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<RuntimeVersion>("state_getRuntimeVersion", null, token);
        }

        public async Task<string> SubscribeRuntimeVersionAsync()
        {
            return await SubscribeRuntimeVersionAsync(CancellationToken.None);
        }

        public async Task<string> SubscribeRuntimeVersionAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<string>("state_subscribeRuntimeVersion", null, token);
        }

        public async Task<bool> UnsubscribeRuntimeVersionAsync(string subscriptionId)
        {
            return await UnsubscribeRuntimeVersionAsync(subscriptionId, CancellationToken.None);
        }

        public async Task<bool> UnsubscribeRuntimeVersionAsync(string subscriptionId, CancellationToken token)
        {
            return await _client.InvokeAsync<bool>("state_unsubscribeRuntimeVersion", new object[] {subscriptionId},
                token);
        }

        public async Task<string> SubscribeStorageAsync(JArray keys, Action<string, StorageChangeSet> callback)
        {
            return await SubscribeStorageAsync(keys, callback, CancellationToken.None);
        }

        public async Task<string> SubscribeStorageAsync(JArray keys, Action<string, StorageChangeSet> callback,
            CancellationToken token)
        {
            var subscriptionId =
                await _client.InvokeAsync<string>("state_subscribeStorage", new object[] {keys}, token);
            _client.Listener.RegisterCallBackHandler(subscriptionId, callback);
            return subscriptionId;
        }

        public async Task<bool> UnsubscribeStorageAsync(string subscriptionId)
        {
            return await UnsubscribeStorageAsync(subscriptionId, CancellationToken.None);
        }

        public async Task<bool> UnsubscribeStorageAsync(string subscriptionId, CancellationToken token)
        {
            return await _client.InvokeAsync<bool>("state_unsubscribeStorage", new object[] {subscriptionId}, token);
        }
    }
}