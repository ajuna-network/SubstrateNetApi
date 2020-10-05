using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using StreamJsonRpc;
using SubstrateNetApi.MetaDataModel.Rpc;
using SubstrateNetApi.MetaDataModel.Values;
using System;

namespace SubstrateNetApi
{
    public interface ISubscriptionListener
    {
        void ChainFinalizedHead(string subscription, Header result);
        void ChainNewHead(string subscription, Header result);
        void ChainAllHead(string subscription, Header result);
        void StateRuntimeVersion(string subscription, JObject result);

        void AuthorSubmitAndWatchExtrinsic(string subscription, ExtrinsicStatus result);

    }

    public class SubscriptionListener : ISubscriptionListener
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [JsonRpcMethod("chain_allHead")]
        public void ChainAllHead(string subscription, Header result)
        {
            Logger.Debug($"{subscription}: {result}");
        }

        [JsonRpcMethod("chain_newHead")]
        public void ChainNewHead(string subscription, Header result)
        {
            Logger.Debug(result);
        }

        [JsonRpcMethod("chain_finalizedHead")]
        public void ChainFinalizedHead(string subscription, Header result)
        {
            Logger.Debug(result);
        }

        [JsonRpcMethod("state_runtimeVersion")]
        public void StateRuntimeVersion(string subscription, JObject result)
        {
            Logger.Debug(result);
        }

        [JsonRpcMethod("author_extrinsicUpdate")]
        public void AuthorSubmitAndWatchExtrinsic(string subscription, ExtrinsicStatus result)
        {
            Logger.Debug(result);
        }
    }

}
