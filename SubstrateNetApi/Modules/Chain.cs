/// <file> SubstrateNetApi\Modules\Chain.cs </file>
/// <copyright file="Chain.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the chain class. </summary>
using SubstrateNetApi.MetaDataModel.Extrinsics;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace SubstrateNetApi.Modules
{
    /// <summary> A chain. </summary>
    /// <remarks> 19.09.2020. </remarks>
    public class Chain
    {
        /// <summary> The client. </summary>
        private readonly SubstrateClient _client;

        /// <summary> Constructor. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="client"> The client. </param>
        internal Chain(SubstrateClient client)
        {
            _client = client;
        }

        /// Get header of a relay chain block.
        public async Task<Header> GetHeaderAsync(CancellationToken token) => await GetHeaderAsync(null, token);

        /// Get header of a relay chain block.
        public async Task<Header> GetHeaderAsync(Hash hash = null) => await GetHeaderAsync(hash, CancellationToken.None);

        /// Get header of a relay chain block.
        public async Task<Header> GetHeaderAsync(Hash hash, CancellationToken token)
        {
            var parameter = hash != null ? hash.HexString : null;
            return await _client.InvokeAsync<Header>("chain_getHeader", new object[] { parameter }, token);
        }

        /// <summary> Gets block asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <returns> The block. </returns>
        public async Task<BlockData> GetBlockAsync() => await GetBlockAsync(null, CancellationToken.None);

        /// <summary> Gets block asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The block. </returns>
        public async Task<BlockData> GetBlockAsync(CancellationToken token) => await GetBlockAsync(null, token);

        /// <summary> Gets block asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="hash"> The hash. </param>
        /// <returns> The block. </returns>
        public async Task<BlockData> GetBlockAsync(Hash hash) => await GetBlockAsync(hash, CancellationToken.None);

        /// <summary> Gets block asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="hash">  The hash. </param>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The block. </returns>
        public async Task<BlockData> GetBlockAsync(Hash hash, CancellationToken token)
        {
            var parameter = hash != null ? hash.HexString : null;
            var result = await _client.InvokeAsync<BlockData>("chain_getBlock", new object[] { parameter }, token);

            for (int i = 0; i < result.Block.Extrinsics.Length; i++)
            {
                result.Block.Extrinsics[i] = Extrinsic.GetTypedExtrinsic(result.Block.Extrinsics[i], _client.MetaData);
            }

            return result;
        }

        /// <summary> Gets block hash asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <returns> The block hash. </returns>
        public async Task<Hash> GetBlockHashAsync() => await GetBlockHashAsync(null, CancellationToken.None);

        /// <summary> Gets block hash asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="BlockNumber"> The block number. </param>
        /// <returns> The block hash. </returns>
        public async Task<Hash> GetBlockHashAsync(BlockNumber blockNumber) => await GetBlockHashAsync(blockNumber, CancellationToken.None);

        /// <summary> Gets block hash asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The block hash. </returns>
        public async Task<Hash> GetBlockHashAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<Hash>("chain_getBlockHash", new object[] { null }, token);
        }

        /// <summary> Gets block hash asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="blockNumber"> The block number. </param>
        /// <param name="token">       A token that allows processing to be cancelled. </param>
        /// <returns> The block hash. </returns>
        public async Task<Hash> GetBlockHashAsync(BlockNumber blockNumber, CancellationToken token)
        {
            return await _client.InvokeAsync<Hash>("chain_getBlockHash", new object[] { Utils.Bytes2HexString(blockNumber.Encode()) }, token);
        }

        /// Get hash of the last finalized block in the canon chain.
        public async Task<Hash> GetFinalizedHeadAsync() => await GetFinalizedHeadAsync(CancellationToken.None);

        /// Get hash of the last finalized block in the canon chain.
        public async Task<Hash> GetFinalizedHeadAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<Hash>("chain_getFinalizedHead", null, token);
        }

        public async Task<string> SubscribeAllHeadsAsync(Func<string, Header, Task> callback) => await SubscribeAllHeadsAsync(callback, CancellationToken.None);
        public async Task<string> SubscribeAllHeadsAsync(Func<string, Header, Task> callback, CancellationToken token)
        {
            var subscriptionId = await _client.InvokeAsync<string>("chain_subscribeAllHeads", null, token);
            _client.Listener.RegisterCallBackHandler(subscriptionId, callback);
            return subscriptionId;
        }

        public async Task<bool> UnsubscribeAllHeadsAsync(string subscriptionId) => await UnsubscribeAllHeadsAsync(subscriptionId, CancellationToken.None);
        public async Task<bool> UnsubscribeAllHeadsAsync(string subscriptionId, CancellationToken token)
        {
            var result = await _client.InvokeAsync<bool>("chain_unsubscribeAllHeads", new object[] { subscriptionId }, token);
            if (result)
            {
                _client.Listener.UnregisterHeaderHandler(subscriptionId);
            }
            return result;
        }

        public async Task<string> SubscribeNewHeadsAsync(Func<string, Header, Task> callback) => await SubscribeNewHeadsAsync(callback, CancellationToken.None);
        public async Task<string> SubscribeNewHeadsAsync(Func<string, Header, Task> callback, CancellationToken token)
        {
            var subscriptionId = await _client.InvokeAsync<string>("chain_subscribeNewHeads", null, token);
            _client.Listener.RegisterCallBackHandler(subscriptionId, callback);
            return subscriptionId;
        }

        public async Task<bool> UnubscribeNewHeadsAsync(string subscriptionId) => await UnsubscribeNewHeadsAsync(subscriptionId, CancellationToken.None);
        public async Task<bool> UnsubscribeNewHeadsAsync(string subscriptionId, CancellationToken token)
        {
            var result = await _client.InvokeAsync<bool>("chain_unsubscribeNewHeads", new object[] { subscriptionId }, token);
            if (result)
            {
                _client.Listener.UnregisterHeaderHandler(subscriptionId);
            }
            return result;
        }

        public async Task<string> SubscribeFinalizedHeadsAsync(Func<string, Header, Task> callback) => await SubscribeFinalizedHeadsAsync(callback, CancellationToken.None);
        public async Task<string> SubscribeFinalizedHeadsAsync(Func<string, Header, Task> callback, CancellationToken token)
        {
            var subscriptionId = await _client.InvokeAsync<string>("chain_subscribeFinalizedHeads", null, token);
            _client.Listener.RegisterCallBackHandler(subscriptionId, callback);
            return subscriptionId;
        }

        public async Task<bool> UnsubscribeFinalizedHeadsAsync(string subscriptionId) => await UnsubscribeFinalizedHeadsAsync(subscriptionId, CancellationToken.None);
        public async Task<bool> UnsubscribeFinalizedHeadsAsync(string subscriptionId, CancellationToken token)
        {
            var result = await _client.InvokeAsync<bool>("chain_unsubscribeFinalizedHeads", new object[] { subscriptionId }, token);
            if (result)
            {
                _client.Listener.UnregisterHeaderHandler(subscriptionId);
            }
            return result;
        }

    }
}
