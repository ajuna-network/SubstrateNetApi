//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.PalletIndices
{
    
    
    /// <summary>
    /// >> 135 - Variant[pallet_indices.pallet.Call]
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class PalletIndicesCall
    {
        
        /// <summary>
        /// >> claim
        /// </summary>
        public GenericExtrinsicCall Claim(SubstrateNetApi.Model.Types.Primitive.U32 index)
        {
            return new GenericExtrinsicCall("Indices", "claim", index);
        }
        
        /// <summary>
        /// >> transfer
        /// </summary>
        public GenericExtrinsicCall Transfer(SubstrateNetApi.Model.SpCore.AccountId32 @new, SubstrateNetApi.Model.Types.Primitive.U32 index)
        {
            return new GenericExtrinsicCall("Indices", "transfer", @new, index);
        }
        
        /// <summary>
        /// >> free
        /// </summary>
        public GenericExtrinsicCall Free(SubstrateNetApi.Model.Types.Primitive.U32 index)
        {
            return new GenericExtrinsicCall("Indices", "free", index);
        }
        
        /// <summary>
        /// >> force_transfer
        /// </summary>
        public GenericExtrinsicCall ForceTransfer(SubstrateNetApi.Model.SpCore.AccountId32 @new, SubstrateNetApi.Model.Types.Primitive.U32 index, SubstrateNetApi.Model.Types.Primitive.Bool freeze)
        {
            return new GenericExtrinsicCall("Indices", "force_transfer", @new, index, freeze);
        }
        
        /// <summary>
        /// >> freeze
        /// </summary>
        public GenericExtrinsicCall Freeze(SubstrateNetApi.Model.Types.Primitive.U32 index)
        {
            return new GenericExtrinsicCall("Indices", "freeze", index);
        }
    }
}
