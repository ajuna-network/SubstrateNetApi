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
    /// >> Path: pallet_grandpa.pallet.Error
    /// 
    ///			Custom [dispatch errors](https://substrate.dev/docs/en/knowledgebase/runtime/errors)
    ///			of this pallet.
    ///			
    /// </summary>
    public enum PalletGrandpa
    {
        
        /// <summary>
        /// >> Event: PauseFailed
        /// Attempt to signal GRANDPA pause when the authority set isn't live
        /// (either paused or already pending pause).
        /// </summary>
        PauseFailed,
        
        /// <summary>
        /// >> Event: ResumeFailed
        /// Attempt to signal GRANDPA resume when the authority set isn't paused
        /// (either live or already pending resume).
        /// </summary>
        ResumeFailed,
        
        /// <summary>
        /// >> Event: ChangePending
        /// Attempt to signal GRANDPA change with one already pending.
        /// </summary>
        ChangePending,
        
        /// <summary>
        /// >> Event: TooSoon
        /// Cannot signal forced change so soon after last.
        /// </summary>
        TooSoon,
        
        /// <summary>
        /// >> Event: InvalidKeyOwnershipProof
        /// A key ownership proof provided as part of an equivocation report is invalid.
        /// </summary>
        InvalidKeyOwnershipProof,
        
        /// <summary>
        /// >> Event: InvalidEquivocationProof
        /// An equivocation proof provided as part of an equivocation report is invalid.
        /// </summary>
        InvalidEquivocationProof,
        
        /// <summary>
        /// >> Event: DuplicateOffenceReport
        /// A given equivocation report is valid but already previously reported.
        /// </summary>
        DuplicateOffenceReport,
    }
}