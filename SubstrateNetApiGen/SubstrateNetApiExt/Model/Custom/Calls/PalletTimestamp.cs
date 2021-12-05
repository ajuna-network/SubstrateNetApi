//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.Custom.Runtime;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Composite;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Sequence;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.Custom.Calls
{
    
    
    /// <summary>
    /// >> Path: pallet_timestamp.pallet.Call
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class PalletTimestamp
    {
        
        /// <summary>
        /// >> Extrinsic: set
        /// Set the current time.
        /// 
        /// This call should be invoked exactly once per block. It will panic at the finalization
        /// phase, if this call hasn't been invoked by that time.
        /// 
        /// The timestamp should be greater than the previous one by the amount specified by
        /// `MinimumPeriod`.
        /// 
        /// The dispatch origin for this call must be `Inherent`.
        /// 
        /// # <weight>
        /// - `O(1)` (Note that implementations of `OnTimestampSet` must also be `O(1)`)
        /// - 1 storage read and 1 storage mutation (codec `O(1)`). (because of `DidUpdate::take` in
        ///   `on_finalize`)
        /// - 1 event handler `on_timestamp_set`. Must be `O(1)`.
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall Set(BaseCom<U64> now)
        {
            return new GenericExtrinsicCall("Timestamp", "set", now);
        }
    }
}
