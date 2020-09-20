/// <file> SubstrateNetApi\Modules\Chain.cs </file>
/// <copyright file="Chain.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the chain class. </summary>
using SubstrateNetApi.MetaDataModel.Values;
using System;
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

        /// <summary> Gets block asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <returns> The block. </returns>
        public async Task<BlockData> BlockAsync() => await BlockAsync(null, CancellationToken.None);

        /// <summary> Gets block asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The block. </returns>
        public async Task<BlockData> BlockAsync(CancellationToken token) => await BlockAsync(null, token);

        /// <summary> Gets block asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="hash"> The hash. </param>
        /// <returns> The block. </returns>
        public async Task<BlockData> BlockAsync(byte[] hash) => await BlockAsync(hash, CancellationToken.None);

        /// <summary> Gets block asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="hash">  The hash. </param>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The block. </returns>
        public async Task<BlockData> BlockAsync(byte[] hash, CancellationToken token)
        {
            var parameter = hash != null ? Utils.Bytes2HexString(hash) : null;
            return await _client.InvokeAsync<BlockData>("chain_getBlock", new object[] { parameter }, token);
        }

        /// <summary> Gets block hash asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <returns> The block hash. </returns>
        public async Task<string> BlockHashAsync() => await BlockHashAsync(null, CancellationToken.None);

        /// <summary> Gets block hash asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="BlockNumber"> The block number. </param>
        /// <returns> The block hash. </returns>
        public async Task<string> BlockHashAsync(uint BlockNumber) => await BlockHashAsync(BlockNumber, CancellationToken.None);

        /// <summary> Gets block hash asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The block hash. </returns>
        public async Task<string> BlockHashAsync(CancellationToken token) => await BlockHashAsync(null, token);

        /// <summary> Gets block hash asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="blockNumber"> The block number. </param>
        /// <param name="token">       A token that allows processing to be cancelled. </param>
        /// <returns> The block hash. </returns>
        public async Task<string> BlockHashAsync(uint? blockNumber, CancellationToken token)
        {
            var parameter = blockNumber.HasValue ? Utils.Bytes2HexString(BitConverter.GetBytes(blockNumber.Value)) : null;
            return await _client.InvokeAsync<string>("chain_getBlockHash", new object[] { parameter }, token);
        }

        /// <summary> Subscribe all heads asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <returns> The subscribe all heads. </returns>
        public async Task<string> SubscribeAllHeadsAsync() => await SubscribeAllHeadsAsync(CancellationToken.None);

        /// <summary> Subscribe all heads asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The subscribe all heads. </returns>
        public async Task<string> SubscribeAllHeadsAsync(CancellationToken token) => await _client.InvokeAsync<string>("chain_subscribeAllHeads", null, token);

        /// <summary> Subscribe new head asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <returns> The subscribe new head. </returns>
        public async Task<string> SubscribeNewHeadAsync() => await SubscribeNewHeadAsync(CancellationToken.None);

        /// <summary> Subscribe new head asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The subscribe new head. </returns>
        public async Task<string> SubscribeNewHeadAsync(CancellationToken token) => await _client.InvokeAsync<string>("chain_subscribeNewHead", null, token);

        /// <summary> Subscribe finalised heads asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <returns> The subscribe finalised heads. </returns>
        public async Task<string> SubscribeFinalisedHeadsAsync() => await SubscribeFinalisedHeadsAsync(CancellationToken.None);

        /// <summary> Subscribe finalised heads asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The subscribe finalised heads. </returns>
        public async Task<string> SubscribeFinalisedHeadsAsync(CancellationToken token) => await _client.InvokeAsync<string>("chain_subscribeFinalisedHeads", null, token);

        /// <summary> Subscribe finalized heads asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <returns> The subscribe finalized heads. </returns>
        public async Task<string> SubscribeFinalizedHeadsAsync() => await SubscribeFinalizedHeadsAsync(CancellationToken.None);

        /// <summary> Subscribe finalized heads asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The subscribe finalized heads. </returns>
        public async Task<string> SubscribeFinalizedHeadsAsync(CancellationToken token) => await _client.InvokeAsync<string>("chain_subscribeFinalizedHeads", null, token);

        /// <summary> Subscribe new heads asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <returns> The subscribe new heads. </returns>
        public async Task<string> SubscribeNewHeadsAsync() => await SubscribeNewHeadsAsync(CancellationToken.None);

        /// <summary> Subscribe new heads asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The subscribe new heads. </returns>
        public async Task<string> SubscribeNewHeadsAsync(CancellationToken token) => await _client.InvokeAsync<string>("chain_subscribeNewHeads", null, token);

        /// <summary> Subscribe runtime version asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <returns> The subscribe runtime version. </returns>
        public async Task<string> SubscribeRuntimeVersionAsync() => await SubscribeRuntimeVersionAsync(CancellationToken.None);

        /// <summary> Subscribe runtime version asynchronous. </summary>
        /// <remarks> 20.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The subscribe runtime version. </returns>
        public async Task<string> SubscribeRuntimeVersionAsync(CancellationToken token) => await _client.InvokeAsync<string>("chain_subscribeRuntimeVersion", null, token);
    }
}
