//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.FrameSupport;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.NodeRuntime;
using SubstrateNetApi.Model.PrimitiveTypes;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace SubstrateNetApi.Model.PalletProxy
{
    
    
    public sealed class ProxyStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateNetApi.SubstrateClient _client;
        
        public ProxyStorage(SubstrateNetApi.SubstrateClient client)
        {
            this._client = client;
        }
        
        /// <summary>
        /// >> Proxies
        /// </summary>
        public async Task<BaseTuple<SubstrateNetApi.Model.FrameSupport.BoundedVec,SubstrateNetApi.Model.Types.Primitive.U128>> Proxies(SubstrateNetApi.Model.SpCore.AccountId32 key, CancellationToken token)
        {
            var keyParams = new IType[] { key };
            var parameters = RequestGenerator.GetStorage("Proxy", "Proxies", Storage.Type.Map, new[] {Storage.Hasher.Twox64Concat}, keyParams);
            return await _client.GetStorageAsync<BaseTuple<SubstrateNetApi.Model.FrameSupport.BoundedVec,SubstrateNetApi.Model.Types.Primitive.U128>>(parameters, token);
        }
        
        /// <summary>
        /// >> Announcements
        /// </summary>
        public async Task<BaseTuple<SubstrateNetApi.Model.FrameSupport.BoundedVec,SubstrateNetApi.Model.Types.Primitive.U128>> Announcements(SubstrateNetApi.Model.SpCore.AccountId32 key, CancellationToken token)
        {
            var keyParams = new IType[] { key };
            var parameters = RequestGenerator.GetStorage("Proxy", "Announcements", Storage.Type.Map, new[] {Storage.Hasher.Twox64Concat}, keyParams);
            return await _client.GetStorageAsync<BaseTuple<SubstrateNetApi.Model.FrameSupport.BoundedVec,SubstrateNetApi.Model.Types.Primitive.U128>>(parameters, token);
        }
    }
    
    public sealed class ProxyCalls
    {
        
        // Substrate client for the storage calls.
        private SubstrateNetApi.SubstrateClient _client;
        
        public ProxyCalls(SubstrateNetApi.SubstrateClient client)
        {
            this._client = client;
        }
        
        /// <summary>
        /// >> proxy
        /// </summary>
        public GenericExtrinsicCall Proxy(SubstrateNetApi.Model.SpCore.AccountId32 real, BaseOpt<SubstrateNetApi.Model.NodeRuntime.EnumProxyType> force_proxy_type, SubstrateNetApi.Model.NodeRuntime.EnumNodeCall call)
        {
            return new GenericExtrinsicCall(30, "Proxy", 0, "proxy", real, force_proxy_type, call);
        }
        
        /// <summary>
        /// >> add_proxy
        /// </summary>
        public GenericExtrinsicCall AddProxy(SubstrateNetApi.Model.SpCore.AccountId32 @delegate, SubstrateNetApi.Model.NodeRuntime.EnumProxyType proxy_type, SubstrateNetApi.Model.Types.Primitive.U32 delay)
        {
            return new GenericExtrinsicCall(30, "Proxy", 1, "add_proxy", @delegate, proxy_type, delay);
        }
        
        /// <summary>
        /// >> remove_proxy
        /// </summary>
        public GenericExtrinsicCall RemoveProxy(SubstrateNetApi.Model.SpCore.AccountId32 @delegate, SubstrateNetApi.Model.NodeRuntime.EnumProxyType proxy_type, SubstrateNetApi.Model.Types.Primitive.U32 delay)
        {
            return new GenericExtrinsicCall(30, "Proxy", 2, "remove_proxy", @delegate, proxy_type, delay);
        }
        
        /// <summary>
        /// >> remove_proxies
        /// </summary>
        public GenericExtrinsicCall RemoveProxies()
        {
            return new GenericExtrinsicCall(30, "Proxy", 3, "remove_proxies");
        }
        
        /// <summary>
        /// >> anonymous
        /// </summary>
        public GenericExtrinsicCall Anonymous(SubstrateNetApi.Model.NodeRuntime.EnumProxyType proxy_type, SubstrateNetApi.Model.Types.Primitive.U32 delay, SubstrateNetApi.Model.Types.Primitive.U16 index)
        {
            return new GenericExtrinsicCall(30, "Proxy", 4, "anonymous", proxy_type, delay, index);
        }
        
        /// <summary>
        /// >> kill_anonymous
        /// </summary>
        public GenericExtrinsicCall KillAnonymous(SubstrateNetApi.Model.SpCore.AccountId32 spawner, SubstrateNetApi.Model.NodeRuntime.EnumProxyType proxy_type, SubstrateNetApi.Model.Types.Primitive.U16 index, BaseCom<SubstrateNetApi.Model.Types.Primitive.U32> height, BaseCom<SubstrateNetApi.Model.Types.Primitive.U32> ext_index)
        {
            return new GenericExtrinsicCall(30, "Proxy", 5, "kill_anonymous", spawner, proxy_type, index, height, ext_index);
        }
        
        /// <summary>
        /// >> announce
        /// </summary>
        public GenericExtrinsicCall Announce(SubstrateNetApi.Model.SpCore.AccountId32 real, SubstrateNetApi.Model.PrimitiveTypes.H256 call_hash)
        {
            return new GenericExtrinsicCall(30, "Proxy", 6, "announce", real, call_hash);
        }
        
        /// <summary>
        /// >> remove_announcement
        /// </summary>
        public GenericExtrinsicCall RemoveAnnouncement(SubstrateNetApi.Model.SpCore.AccountId32 real, SubstrateNetApi.Model.PrimitiveTypes.H256 call_hash)
        {
            return new GenericExtrinsicCall(30, "Proxy", 7, "remove_announcement", real, call_hash);
        }
        
        /// <summary>
        /// >> reject_announcement
        /// </summary>
        public GenericExtrinsicCall RejectAnnouncement(SubstrateNetApi.Model.SpCore.AccountId32 @delegate, SubstrateNetApi.Model.PrimitiveTypes.H256 call_hash)
        {
            return new GenericExtrinsicCall(30, "Proxy", 8, "reject_announcement", @delegate, call_hash);
        }
        
        /// <summary>
        /// >> proxy_announced
        /// </summary>
        public GenericExtrinsicCall ProxyAnnounced(SubstrateNetApi.Model.SpCore.AccountId32 @delegate, SubstrateNetApi.Model.SpCore.AccountId32 real, BaseOpt<SubstrateNetApi.Model.NodeRuntime.EnumProxyType> force_proxy_type, SubstrateNetApi.Model.NodeRuntime.EnumNodeCall call)
        {
            return new GenericExtrinsicCall(30, "Proxy", 9, "proxy_announced", @delegate, real, force_proxy_type, call);
        }
    }
    
    /// <summary>
    /// >> ProxyExecuted
    /// </summary>
    public sealed class EventProxyExecuted : BaseTuple<BaseTuple<BaseTuple,  SubstrateNetApi.Model.SpRuntime.EnumDispatchError>>
    {
    }
    
    /// <summary>
    /// >> AnonymousCreated
    /// </summary>
    public sealed class EventAnonymousCreated : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.NodeRuntime.EnumProxyType, SubstrateNetApi.Model.Types.Primitive.U16>
    {
    }
    
    /// <summary>
    /// >> Announced
    /// </summary>
    public sealed class EventAnnounced : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.PrimitiveTypes.H256>
    {
    }
    
    /// <summary>
    /// >> ProxyAdded
    /// </summary>
    public sealed class EventProxyAdded : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.NodeRuntime.EnumProxyType, SubstrateNetApi.Model.Types.Primitive.U32>
    {
    }
    
    public enum ProxyErrors
    {
        
        /// <summary>
        /// >> TooMany
        /// </summary>
        TooMany,
        
        /// <summary>
        /// >> NotFound
        /// </summary>
        NotFound,
        
        /// <summary>
        /// >> NotProxy
        /// </summary>
        NotProxy,
        
        /// <summary>
        /// >> Unproxyable
        /// </summary>
        Unproxyable,
        
        /// <summary>
        /// >> Duplicate
        /// </summary>
        Duplicate,
        
        /// <summary>
        /// >> NoPermission
        /// </summary>
        NoPermission,
        
        /// <summary>
        /// >> Unannounced
        /// </summary>
        Unannounced,
        
        /// <summary>
        /// >> NoSelfProxy
        /// </summary>
        NoSelfProxy,
    }
}
