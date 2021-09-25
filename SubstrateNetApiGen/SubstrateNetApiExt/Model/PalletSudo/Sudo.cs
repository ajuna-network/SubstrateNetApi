//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.NodeRuntime;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.SpRuntime;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace SubstrateNetApi.Model.PalletSudo
{
    
    
    public sealed class SudoStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateNetApi.SubstrateClient _client;
        
        public SudoStorage(SubstrateNetApi.SubstrateClient client)
        {
            this._client = client;
        }
        
        /// <summary>
        /// >> Key
        /// </summary>
        public async Task<SubstrateNetApi.Model.SpCore.AccountId32> Key(CancellationToken token)
        {
            var parameters = RequestGenerator.GetStorage("Sudo", "Key", Storage.Type.Plain);
            return await _client.GetStorageAsync<SubstrateNetApi.Model.SpCore.AccountId32>(parameters, token);
        }
    }
    
    public sealed class SudoCalls
    {
        
        // Substrate client for the storage calls.
        private SubstrateNetApi.SubstrateClient _client;
        
        public SudoCalls(SubstrateNetApi.SubstrateClient client)
        {
            this._client = client;
        }
        
        /// <summary>
        /// >> sudo
        /// </summary>
        public GenericExtrinsicCall Sudo(SubstrateNetApi.Model.NodeRuntime.EnumNodeCall call)
        {
            return new GenericExtrinsicCall(19, "Sudo", 0, "sudo", call);
        }
        
        /// <summary>
        /// >> sudo_unchecked_weight
        /// </summary>
        public GenericExtrinsicCall SudoUncheckedWeight(SubstrateNetApi.Model.NodeRuntime.EnumNodeCall call, SubstrateNetApi.Model.Types.Primitive.U64 weight)
        {
            return new GenericExtrinsicCall(19, "Sudo", 1, "sudo_unchecked_weight", call, weight);
        }
        
        /// <summary>
        /// >> set_key
        /// </summary>
        public GenericExtrinsicCall SetKey(SubstrateNetApi.Model.SpRuntime.EnumMultiAddress @new)
        {
            return new GenericExtrinsicCall(19, "Sudo", 2, "set_key", @new);
        }
        
        /// <summary>
        /// >> sudo_as
        /// </summary>
        public GenericExtrinsicCall SudoAs(SubstrateNetApi.Model.SpRuntime.EnumMultiAddress who, SubstrateNetApi.Model.NodeRuntime.EnumNodeCall call)
        {
            return new GenericExtrinsicCall(19, "Sudo", 3, "sudo_as", who, call);
        }
    }
    
    /// <summary>
    /// >> Sudid
    /// </summary>
    public sealed class EventSudid : BaseTuple<BaseTuple<BaseTuple,  SubstrateNetApi.Model.SpRuntime.EnumDispatchError>>
    {
    }
    
    /// <summary>
    /// >> KeyChanged
    /// </summary>
    public sealed class EventKeyChanged : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32>
    {
    }
    
    /// <summary>
    /// >> SudoAsDone
    /// </summary>
    public sealed class EventSudoAsDone : BaseTuple<BaseTuple<BaseTuple,  SubstrateNetApi.Model.SpRuntime.EnumDispatchError>>
    {
    }
    
    public enum SudoErrors
    {
        
        /// <summary>
        /// >> RequireSudo
        /// </summary>
        RequireSudo,
    }
}
