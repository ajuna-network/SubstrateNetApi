/// <file> SubstrateNetApi\Modules\System.cs </file>
/// <copyright file="System.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the state class. </summary>
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
    }
}
