//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Base;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.NodeRuntime;
using SubstrateNetApi.Model.PalletMultisig;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.PalletMultisig
{
    
    
    /// <summary>
    /// >> 290 - Variant[pallet_multisig.pallet.Call]
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class PalletMultisigCall
    {
        
        /// <summary>
        /// >> as_multi_threshold_1
        /// </summary>
        public GenericExtrinsicCall AsMultiThreshold1(BaseVec<SubstrateNetApi.Model.SpCore.AccountId32> other_signatories, SubstrateNetApi.Model.NodeRuntime.EnumNodeCall call)
        {
            return new GenericExtrinsicCall("Multisig", "as_multi_threshold_1", other_signatories, call);
        }
        
        /// <summary>
        /// >> as_multi
        /// </summary>
        public GenericExtrinsicCall AsMulti(SubstrateNetApi.Model.Types.Primitive.U16 threshold, BaseVec<SubstrateNetApi.Model.SpCore.AccountId32> other_signatories, BaseOpt<SubstrateNetApi.Model.PalletMultisig.Timepoint> maybe_timepoint, BaseVec<SubstrateNetApi.Model.Types.Primitive.U8> call, SubstrateNetApi.Model.Types.Primitive.Bool store_call, SubstrateNetApi.Model.Types.Primitive.U64 max_weight)
        {
            return new GenericExtrinsicCall("Multisig", "as_multi", threshold, other_signatories, maybe_timepoint, call, store_call, max_weight);
        }
        
        /// <summary>
        /// >> approve_as_multi
        /// </summary>
        public GenericExtrinsicCall ApproveAsMulti(SubstrateNetApi.Model.Types.Primitive.U16 threshold, BaseVec<SubstrateNetApi.Model.SpCore.AccountId32> other_signatories, BaseOpt<SubstrateNetApi.Model.PalletMultisig.Timepoint> maybe_timepoint, SubstrateNetApi.Model.Base.Arr32U8 call_hash, SubstrateNetApi.Model.Types.Primitive.U64 max_weight)
        {
            return new GenericExtrinsicCall("Multisig", "approve_as_multi", threshold, other_signatories, maybe_timepoint, call_hash, max_weight);
        }
        
        /// <summary>
        /// >> cancel_as_multi
        /// </summary>
        public GenericExtrinsicCall CancelAsMulti(SubstrateNetApi.Model.Types.Primitive.U16 threshold, BaseVec<SubstrateNetApi.Model.SpCore.AccountId32> other_signatories, SubstrateNetApi.Model.PalletMultisig.Timepoint timepoint, SubstrateNetApi.Model.Base.Arr32U8 call_hash)
        {
            return new GenericExtrinsicCall("Multisig", "cancel_as_multi", threshold, other_signatories, timepoint, call_hash);
        }
    }
}
