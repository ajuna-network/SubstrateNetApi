using Newtonsoft.Json.Linq;
using SubstrateNetApi.MetaDataModel.Values;
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

        public async Task<BlockData> GetBlockAsync()
        {
            return await GetBlockAsync(null, CancellationToken.None);
        }

        public async Task<BlockData> GetBlockAsync(CancellationToken token)
        {
            return await GetBlockAsync(null, token);
        }

        public async Task<BlockData> GetBlockAsync(byte[] hash)
        {
            return await GetBlockAsync(hash, CancellationToken.None);
        }

        public async Task<BlockData> GetBlockAsync(byte[] hash, CancellationToken token)
        {

            var parameter = hash != null ? Utils.Bytes2HexString(hash) : null;

            return await _client.InvokeAsync<BlockData>("chain_getBlock", new object[] { parameter }, token);
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
