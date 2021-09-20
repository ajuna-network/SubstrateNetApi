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
    /// >> Path: pallet_tips.pallet.Error
    /// 
    ///			Custom [dispatch errors](https://substrate.dev/docs/en/knowledgebase/runtime/errors)
    ///			of this pallet.
    ///			
    /// </summary>
    public enum PalletTips
    {
        
        /// <summary>
        /// >> Event: ReasonTooBig
        /// The reason given is just too big.
        /// </summary>
        ReasonTooBig,
        
        /// <summary>
        /// >> Event: AlreadyKnown
        /// The tip was already found/started.
        /// </summary>
        AlreadyKnown,
        
        /// <summary>
        /// >> Event: UnknownTip
        /// The tip hash is unknown.
        /// </summary>
        UnknownTip,
        
        /// <summary>
        /// >> Event: NotFinder
        /// The account attempting to retract the tip is not the finder of the tip.
        /// </summary>
        NotFinder,
        
        /// <summary>
        /// >> Event: StillOpen
        /// The tip cannot be claimed/closed because there are not enough tippers yet.
        /// </summary>
        StillOpen,
        
        /// <summary>
        /// >> Event: Premature
        /// The tip cannot be claimed/closed because it's still in the countdown period.
        /// </summary>
        Premature,
    }
}
