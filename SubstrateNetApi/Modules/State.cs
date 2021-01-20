/// <file> SubstrateNetApi\Modules\System.cs </file>
/// <copyright file="System.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the state class. </summary>
using Newtonsoft.Json.Linq;
using SubstrateNetApi.Model.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<string> GetMetaDataAsync() => await GetMetaDataAsync(CancellationToken.None);
        public async Task<string> GetMetaDataAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<string>("state_getMetadata", null, token);
        }

        public async Task<string> SubscribeRuntimeVersionAsync() => await SubscribeRuntimeVersionAsync(CancellationToken.None);
        public async Task<string> SubscribeRuntimeVersionAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<string>("state_subscribeRuntimeVersion", null, token);
        }

        public async Task<bool> UnsubscribeRuntimeVersionAsync(string subscriptionId) => await UnsubscribeRuntimeVersionAsync(subscriptionId, CancellationToken.None);
        public async Task<bool> UnsubscribeRuntimeVersionAsync(string subscriptionId, CancellationToken token)
        {
            return await _client.InvokeAsync<bool>("state_unsubscribeRuntimeVersion", new object[] { subscriptionId }, token);
        }
        public async Task<string> SubscribeStorageAsync(JArray keys, Action<string, StorageChangeSet> callback) => await SubscribeStorageAsync(keys, callback, CancellationToken.None);
        public async Task<string> SubscribeStorageAsync(JArray keys, Action<string, StorageChangeSet> callback, CancellationToken token)
        {
            var subscriptionId = await _client.InvokeAsync<string>("state_subscribeStorage", new object[] { keys }, token);
            _client.Listener.RegisterCallBackHandler(subscriptionId, callback);
            return subscriptionId;
        }

        public async Task<bool> UnsubscribeStorageAsync(string subscriptionId) => await UnsubscribeStorageAsync(subscriptionId, CancellationToken.None);
        public async Task<bool> UnsubscribeStorageAsync(string subscriptionId, CancellationToken token)
        {
            return await _client.InvokeAsync<bool>("state_unsubscribeStorage", new object[] { subscriptionId }, token);
        }
    }
}
