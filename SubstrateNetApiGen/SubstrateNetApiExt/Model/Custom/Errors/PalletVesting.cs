//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Custom.Runtime;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Composite;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Sequence;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.Custom.Errors
{
    
    
    /// <summary>
    /// >> Path: pallet_vesting.pallet.Error
    /// Error for the vesting pallet.
    /// </summary>
    public enum PalletVesting
    {
        
        /// <summary>
        /// >> Event: NotVesting
        /// The account given is not vesting.
        /// </summary>
        NotVesting,
        
        /// <summary>
        /// >> Event: AtMaxVestingSchedules
        /// The account already has `MaxVestingSchedules` count of schedules and thus
        /// cannot add another one. Consider merging existing schedules in order to add another.
        /// </summary>
        AtMaxVestingSchedules,
        
        /// <summary>
        /// >> Event: AmountLow
        /// Amount being transferred is too low to create a vesting schedule.
        /// </summary>
        AmountLow,
        
        /// <summary>
        /// >> Event: ScheduleIndexOutOfBounds
        /// An index was out of bounds of the vesting schedules.
        /// </summary>
        ScheduleIndexOutOfBounds,
        
        /// <summary>
        /// >> Event: InvalidScheduleParams
        /// Failed to create a new schedule because some parameter was invalid.
        /// </summary>
        InvalidScheduleParams,
    }
}
