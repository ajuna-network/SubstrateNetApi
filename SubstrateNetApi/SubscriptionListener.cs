using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using StreamJsonRpc;
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
        public void AuthorSubmitAndWatchExtrinsic(string subscription, JObject result)
        {
            Logger.Debug(result);
        }

        /* 
         
        2020-10-04 12:36:10.2528|DEBUG|SubstrateNetApi.SubscriptionListener|{
  "broadcast": [
    "12D3KooWGvthLbtAHsNcvDTqUpuxRe6gcdMz6sQR8T44rBQmhH2x"
  ]
}

2020-10-04 12:36:11.3414|DEBUG|SubstrateNetApi.SubscriptionListener|{
  "inBlock": "0x3bb05fae43aa376a466fe2014a4da81d0397e6a4ea007cbffda6e4d7c7ce9062"
}

2020-10-04 12:36:25.3361|DEBUG|SubstrateNetApi.SubscriptionListener|{
  "finalized": "0x3bb05fae43aa376a466fe2014a4da81d0397e6a4ea007cbffda6e4d7c7ce9062"
}

         
         */
    }

}
