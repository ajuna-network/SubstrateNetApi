/// <file> SubstrateNetApi\Modules\System.cs </file>
/// <copyright file="System.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the author class. </summary>
using SubstrateNetApi.MetaDataModel.Calls;
using SubstrateNetApi.MetaDataModel.Extrinsics;
using SubstrateNetApi.MetaDataModel.Rpc;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubstrateNetApi.Modules
{
    /// <summary> A author. </summary>
    /// <remarks> 19.09.2020. </remarks>
    public class Author
    {
        /// <summary> The client. </summary>
        private readonly SubstrateClient _client;

        /// <summary> Constructor. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="client"> The client. </param>
        internal Author(SubstrateClient client)
        {
            _client = client;
        }

        public async Task<Extrinsic[]> PendingExtrinsicAsync()
        {
            return await PendingExtrinsicAsync(CancellationToken.None);
        }

        public async Task<Extrinsic[]> PendingExtrinsicAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<Extrinsic[]>("author_pendingExtrinsics", null, token);
        }

        public async Task<Hash> SubmitExtrinsicAsync(GenericExtrinsicCall callArguments, Account account, uint tip, uint lifeTime) => await SubmitExtrinsicAsync(await _client.GetExtrinsicParametersAsync(callArguments, account, tip, lifeTime, CancellationToken.None));
        public async Task<Hash> SubmitExtrinsicAsync(GenericExtrinsicCall callArguments, Account account, uint tip, uint lifeTime, CancellationToken token) => await SubmitExtrinsicAsync(await _client.GetExtrinsicParametersAsync(callArguments, account, tip, lifeTime, token), token);
        public async Task<Hash> SubmitExtrinsicAsync(string parameters) => await SubmitExtrinsicAsync(parameters, CancellationToken.None);
        public async Task<Hash> SubmitExtrinsicAsync(string parameters, CancellationToken token)
        {
            return await _client.InvokeAsync<Hash>("author_submitExtrinsic", new object[] { parameters }, token);
        }

        public async Task<string> SubmitAndWatchExtrinsicAsync(Action<string, ExtrinsicStatus> callback, GenericExtrinsicCall callArguments, Account account, uint tip, uint lifeTime) => await SubmitAndWatchExtrinsicAsync(callback, await _client.GetExtrinsicParametersAsync(callArguments, account, tip, lifeTime, CancellationToken.None));
        public async Task<string> SubmitAndWatchExtrinsicAsync(Action<string, ExtrinsicStatus> callback, GenericExtrinsicCall callArguments, Account account, uint tip, uint lifeTime, CancellationToken token) => await SubmitAndWatchExtrinsicAsync(callback, await _client.GetExtrinsicParametersAsync(callArguments, account, tip, lifeTime, token), token);
        public async Task<string> SubmitAndWatchExtrinsicAsync(Action<string, ExtrinsicStatus> callback, string parameters) => await SubmitAndWatchExtrinsicAsync(callback, parameters, CancellationToken.None);
        public async Task<string> SubmitAndWatchExtrinsicAsync(Action<string, ExtrinsicStatus> callback, string parameters, CancellationToken token)
        {
            var subscriptionId = await _client.InvokeAsync<string>("author_submitAndWatchExtrinsic", new object[] { parameters }, token);
            _client.Listener.RegisterCallBackHandler(subscriptionId, callback);
            return subscriptionId;
        }

        public async Task<bool> UnwatchExtrinsicAsync(string subscriptionId) => await UnwatchExtrinsicAsync(subscriptionId, CancellationToken.None);
        public async Task<bool> UnwatchExtrinsicAsync(string subscriptionId, CancellationToken token)
        {
            var result = await _client.InvokeAsync<bool>("author_unwatchExtrinsic", new object[] { subscriptionId }, token);
            if (result)
            {
                _client.Listener.UnregisterHeaderHandler(subscriptionId);
            }
            return result;
        }
    }
}
