/// <file> SubstrateNetApi\Modules\System.cs </file>
/// <copyright file="System.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the system class. </summary>
using System.Threading;
using System.Threading.Tasks;

namespace SubstrateNetApi.Modules
{
    /// <summary> A system. </summary>
    /// <remarks> 19.09.2020. </remarks>
    public class System
    {
        /// <summary> The client. </summary>
        private readonly SubstrateClient _client;

        /// <summary> Constructor. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="client"> The client. </param>
        internal System(SubstrateClient client)
        {
            _client = client;
        }

        /// <summary> Chain asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <returns> The chain. </returns>
        public async Task<string> ChainAsync()
        {
            return await ChainAsync(CancellationToken.None);
        }

        /// <summary> Chain asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The chain. </returns>
        public async Task<string> ChainAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<string>("system_chain", null, token);
        }

        /// <summary> Name asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <returns> The name. </returns>
        public async Task<string> NameAsync()
        {
            return await NameAsync(CancellationToken.None);
        }

        /// <summary> Name asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The name. </returns>
        public async Task<string> NameAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<string>("system_name", null, token);
        }

        /// <summary> Version asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <returns> The version. </returns>
        public async Task<string> VersionAsync()
        {
            return await VersionAsync(CancellationToken.None);
        }

        /// <summary> Version asynchronous. </summary>
        /// <remarks> 19.09.2020. </remarks>
        /// <param name="token"> A token that allows processing to be cancelled. </param>
        /// <returns> The version. </returns>
        public async Task<string> VersionAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<string>("system_version", null, token);
        }
    }
}
