using System;
using System.Threading;
using System.Threading.Tasks;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.Extrinsics;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;

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

        public async Task<Hash> SubmitExtrinsicAsync(GenericExtrinsicCall callArguments, Account account, uint tip, uint lifeTime)
        {
            var extrinsic = await _client.GetExtrinsicParametersAsync(callArguments, account, tip, lifeTime, signed:true, CancellationToken.None);

            return await SubmitExtrinsicAsync(Utils.Bytes2HexString(extrinsic.Encode()));
        }

        public async Task<Hash> SubmitExtrinsicAsync(GenericExtrinsicCall callArguments, Account account, uint tip, uint lifeTime, CancellationToken token)
        {
            var extrinsic = await _client.GetExtrinsicParametersAsync(callArguments, account, tip, lifeTime, signed: true, token);
            
            return await SubmitExtrinsicAsync(Utils.Bytes2HexString(extrinsic.Encode()), token);
        }

        public async Task<Hash> SubmitExtrinsicAsync(string parameters)
        {
            return await SubmitExtrinsicAsync(parameters, CancellationToken.None);
        }

        public async Task<Hash> SubmitExtrinsicAsync(string parameters, CancellationToken token)
        {
            return await _client.InvokeAsync<Hash>("author_submitExtrinsic", new object[] {parameters}, token);
        }

        public async Task<string> SubmitAndWatchExtrinsicAsync(Action<string, ExtrinsicStatus> callback,
            GenericExtrinsicCall callArguments, Account account, uint tip, uint lifeTime)
        {
            var extrinsic = await _client.GetExtrinsicParametersAsync(callArguments, account, tip, lifeTime, signed: true, CancellationToken.None);

            return await SubmitAndWatchExtrinsicAsync(callback, Utils.Bytes2HexString(extrinsic.Encode()));
        }

        public async Task<string> SubmitAndWatchExtrinsicAsync(Action<string, ExtrinsicStatus> callback,
            GenericExtrinsicCall callArguments, Account account, uint tip, uint lifeTime, CancellationToken token)
        {
            var extrinsic = await _client.GetExtrinsicParametersAsync(callArguments, account, tip, lifeTime, signed: true, token);

            return await SubmitAndWatchExtrinsicAsync(callback, Utils.Bytes2HexString(extrinsic.Encode()), token);
        }

        public async Task<string> SubmitAndWatchExtrinsicAsync(Action<string, ExtrinsicStatus> callback,
            string parameters)
        {
            return await SubmitAndWatchExtrinsicAsync(callback, parameters, CancellationToken.None);
        }

        public async Task<string> SubmitAndWatchExtrinsicAsync(Action<string, ExtrinsicStatus> callback,
            string parameters, CancellationToken token)
        {
            var subscriptionId =
                await _client.InvokeAsync<string>("author_submitAndWatchExtrinsic", new object[] {parameters}, token);
            _client.Listener.RegisterCallBackHandler(subscriptionId, callback);
            return subscriptionId;
        }

        public async Task<bool> UnwatchExtrinsicAsync(string subscriptionId)
        {
            return await UnwatchExtrinsicAsync(subscriptionId, CancellationToken.None);
        }

        public async Task<bool> UnwatchExtrinsicAsync(string subscriptionId, CancellationToken token)
        {
            var result =
                await _client.InvokeAsync<bool>("author_unwatchExtrinsic", new object[] {subscriptionId}, token);
            if (result) _client.Listener.UnregisterHeaderHandler(subscriptionId);
            return result;
        }
    }
}