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
            return await GetBlockHashAsync(null, CancellationToken.None);
        }

        public async Task<string> GetBlockHashAsync(uint BlockNumber)
        {
            return await GetBlockHashAsync(BlockNumber, CancellationToken.None);
        }

        public async Task<string> GetBlockHashAsync(CancellationToken token)
        {
            return await GetBlockHashAsync(null, token);
        }

        public async Task<string> GetBlockHashAsync(uint? blockNumber, CancellationToken token)
        {

            var parameter = blockNumber.HasValue ? Utils.Bytes2HexString(BitConverter.GetBytes(blockNumber.Value)) : null;
          
            return await _client.InvokeAsync<string>("chain_getBlockHash", new object[] { parameter }, token);
        }
    }
}
