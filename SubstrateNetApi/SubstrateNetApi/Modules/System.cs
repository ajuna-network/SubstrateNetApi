using System.Threading;
using System.Threading.Tasks;

namespace SubstrateNetApi.Modules
{
    public class System
    {
        private readonly SubstrateClient _client;

        internal System(SubstrateClient client)
        {
            _client = client;
        }

        public async Task<string> ChainAsync()
        {
            return await ChainAsync(CancellationToken.None);
        }

        public async Task<string> ChainAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<string>("system_chain", null, token);
        }

        public async Task<string> NameAsync()
        {
            return await NameAsync(CancellationToken.None);
        }

        public async Task<string> NameAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<string>("system_name", null, token);
        }

        public async Task<string> VersionAsync()
        {
            return await VersionAsync(CancellationToken.None);
        }

        public async Task<string> VersionAsync(CancellationToken token)
        {
            return await _client.InvokeAsync<string>("system_version", null, token);
        }
    }
}
