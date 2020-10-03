/// <file> SubstrateNetApi\Modules\System.cs </file>
/// <copyright file="System.cs" company="mogwaicoin.org">
/// Copyright (c) 2020 mogwaicoin.org. All rights reserved.
/// </copyright>
/// <summary> Implements the author class. </summary>
using Newtonsoft.Json.Linq;
using SubstrateNetApi.MetaDataModel.Extrinsic;
using SubstrateNetApi.MetaDataModel.Values;
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

        public async Task<ExtrinsicModel[]> PendingExtrinsicAsync()
        {
            return await PendingExtrinsicAsync(CancellationToken.None);
        }

        public async Task<ExtrinsicModel[]> PendingExtrinsicAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<ExtrinsicModel[]>("author_pendingExtrinsics", null, token);
        }

    }
}
