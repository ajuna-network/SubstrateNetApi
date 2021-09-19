//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Calls;
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
    /// >> Path: pallet_gilt.pallet.Call
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class PalletGilt
    {
        
        /// <summary>
        /// >> Extrinsic: place_bid
        /// Place a bid for a gilt to be issued.
        /// 
        /// Origin must be Signed, and account must have at least `amount` in free balance.
        /// 
        /// - `amount`: The amount of the bid; these funds will be reserved. If the bid is
        /// successfully elevated into an issued gilt, then these funds will continue to be
        /// reserved until the gilt expires. Must be at least `MinFreeze`.
        /// - `duration`: The number of periods for which the funds will be locked if the gilt is
        /// issued. It will expire only after this period has elapsed after the point of issuance.
        /// Must be greater than 1 and no more than `QueueCount`.
        /// 
        /// Complexities:
        /// - `Queues[duration].len()` (just take max).
        /// </summary>
        public GenericExtrinsicCall PlaceBid(BaseCom<U128> amount, U32 duration)
        {
            return new GenericExtrinsicCall("Gilt", "place_bid", amount, duration);
        }
        
        /// <summary>
        /// >> Extrinsic: retract_bid
        /// Retract a previously placed bid.
        /// 
        /// Origin must be Signed, and the account should have previously issued a still-active bid
        /// of `amount` for `duration`.
        /// 
        /// - `amount`: The amount of the previous bid.
        /// - `duration`: The duration of the previous bid.
        /// </summary>
        public GenericExtrinsicCall RetractBid(BaseCom<U128> amount, U32 duration)
        {
            return new GenericExtrinsicCall("Gilt", "retract_bid", amount, duration);
        }
        
        /// <summary>
        /// >> Extrinsic: set_target
        /// Set target proportion of gilt-funds.
        /// 
        /// Origin must be `AdminOrigin`.
        /// 
        /// - `target`: The target proportion of effective issued funds that should be under gilts
        /// at any one time.
        /// </summary>
        public GenericExtrinsicCall SetTarget(BaseCom<Perquintill> target)
        {
            return new GenericExtrinsicCall("Gilt", "set_target", target);
        }
        
        /// <summary>
        /// >> Extrinsic: thaw
        /// Remove an active but expired gilt. Reserved funds under gilt are freed and balance is
        /// adjusted to ensure that the funds grow or shrink to maintain the equivalent proportion
        /// of effective total issued funds.
        /// 
        /// Origin must be Signed and the account must be the owner of the gilt of the given index.
        /// 
        /// - `index`: The index of the gilt to be thawed.
        /// </summary>
        public GenericExtrinsicCall Thaw(BaseCom<U32> index)
        {
            return new GenericExtrinsicCall("Gilt", "thaw", index);
        }
    }
}
