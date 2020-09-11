using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubstrateNetApi.Modules
{
    public class Chain
    {
        private readonly SubstrateClient _client;

        internal Chain(SubstrateClient client)
        {
            _client = client;
        }

        public async Task<string> GetBlockHashAsync()
        {
            return await GetBlockHashAsync(CancellationToken.None);
        }

        public async Task<string> GetBlockHashAsync(CancellationToken token)
        {
            return await GetBlockHashAsync(0, true, CancellationToken.None);
        }

        public async Task<string> GetBlockHashAsync(uint BlockNumber)
        {
            return await GetBlockHashAsync(BlockNumber, CancellationToken.None);
        }

        public async Task<string> GetBlockHashAsync(uint BlockNumber, CancellationToken token)
        {
            return await GetBlockHashAsync(BlockNumber, false, CancellationToken.None);
        }

        private async Task<string> GetBlockHashAsync(uint BlockNumber, bool BlockNumberNull, CancellationToken token)
        {
            var parameter = Utils.Bytes2HexString(BitConverter.GetBytes(BlockNumber));
            return await _client.InvokeAsync<string>("chain_getBlockHash", BlockNumberNull ? null : new object[] { parameter }, token);
        }
    }
}
