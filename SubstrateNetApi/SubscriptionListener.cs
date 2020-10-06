using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using StreamJsonRpc;
using SubstrateNetApi.MetaDataModel.Rpc;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi
{
    public class SubscriptionListener
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, object> _headerCallbacks = new Dictionary<string, object>();

        private readonly Dictionary<string, List<object>> _pendingHeaders = new Dictionary<string, List<object>>();

        public void RegisterHeaderHandler<T>(string subscription, Action<T> callback)
        {
            if (!_headerCallbacks.ContainsKey(subscription))
            {
                _headerCallbacks.Add(subscription, callback);
            }
            if (_pendingHeaders.ContainsKey(subscription))
            {
                foreach (var h in _pendingHeaders[subscription])
                {
                    callback((T)h);
                }
                _pendingHeaders.Remove(subscription);
            }
        }

        public void UnregisterHeaderHandler(string subscription)
        {
            if (_headerCallbacks.ContainsKey(subscription))
            {
                _headerCallbacks.Remove(subscription);
            }
        }

        private void GenericCallBack<T>(string subscription, T result)
        {
            Logger.Debug($"{subscription}: {result}");
            if (_headerCallbacks.ContainsKey(subscription))
            {
                ((Action<T>)_headerCallbacks[subscription])(result);
            }
            else
            {
                if (!_pendingHeaders.ContainsKey(subscription))
                {
                    _pendingHeaders.Add(subscription, new List<object>());
                }
                _pendingHeaders[subscription].Add(result);
            }
        }

        [JsonRpcMethod("chain_allHead")]
        public void ChainAllHead(string subscription, Header result)
        {
            GenericCallBack(subscription, result);
        }

        [JsonRpcMethod("chain_newHead")]
        public void ChainNewHead(string subscription, Header result)
        {
            GenericCallBack(subscription, result);
        }

        [JsonRpcMethod("chain_finalizedHead")]
        public void ChainFinalizedHead(string subscription, Header result)
        {
            GenericCallBack(subscription, result);
        }

        [JsonRpcMethod("state_runtimeVersion")]
        public void StateRuntimeVersion(string subscription, JObject result)
        {
            GenericCallBack(subscription, result);
        }

        [JsonRpcMethod("author_extrinsicUpdate")]
        public void AuthorSubmitAndWatchExtrinsic(string subscription, ExtrinsicStatus result)
        {
            GenericCallBack(subscription, result);
        }
    }

}
