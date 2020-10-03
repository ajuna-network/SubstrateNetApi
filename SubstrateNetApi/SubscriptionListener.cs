using Newtonsoft.Json.Linq;
using NLog;
using StreamJsonRpc;

namespace SubstrateNetApi
{
    public class SubscriptionListener
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [JsonRpcMethod("chain_finalizedHead")]
        public void ChainFinalizedHead(string subscription, JObject result)
        {
            Logger.Debug(result);
        }

        [JsonRpcMethod("chain_newHead")]
        public void ChainNewHead(string subscription, JObject result)
        {
            Logger.Debug(result);
        }

        [JsonRpcMethod("chain_allHead")]
        public void ChainAllHead(string subscription, JObject result)
        {
            Logger.Debug(result);
        }

        [JsonRpcMethod("state_runtimeVersion")]
        public void StateRuntimeVersion(string subscription, JObject result)
        {
            Logger.Debug(result);
        }
    }
}
