//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.PalletSociety
{
    
    
    /// <summary>
    /// >> 69 - Variant[pallet_society.pallet.Event]
    /// 
    ///			The [event](https://substrate.dev/docs/en/knowledgebase/runtime/events) emitted
    ///			by this pallet.
    ///			
    /// </summary>
    public sealed class PalletSocietyEvent
    {
        
        /// <summary>
        /// >> Founded
        /// </summary>
        public sealed class Founded : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32>
        {
        }
        
        /// <summary>
        /// >> Bid
        /// </summary>
        public sealed class Bid : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.Types.Primitive.U128>
        {
        }
        
        /// <summary>
        /// >> Vouch
        /// </summary>
        public sealed class Vouch : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.Types.Primitive.U128, SubstrateNetApi.Model.SpCore.AccountId32>
        {
        }
        
        /// <summary>
        /// >> AutoUnbid
        /// </summary>
        public sealed class AutoUnbid : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32>
        {
        }
        
        /// <summary>
        /// >> Unbid
        /// </summary>
        public sealed class Unbid : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32>
        {
        }
        
        /// <summary>
        /// >> Unvouch
        /// </summary>
        public sealed class Unvouch : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32>
        {
        }
        
        /// <summary>
        /// >> Inducted
        /// </summary>
        public sealed class Inducted : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, BaseVec<SubstrateNetApi.Model.SpCore.AccountId32>>
        {
        }
        
        /// <summary>
        /// >> SuspendedMemberJudgement
        /// </summary>
        public sealed class SuspendedMemberJudgement : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.Types.Primitive.Bool>
        {
        }
        
        /// <summary>
        /// >> CandidateSuspended
        /// </summary>
        public sealed class CandidateSuspended : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32>
        {
        }
        
        /// <summary>
        /// >> MemberSuspended
        /// </summary>
        public sealed class MemberSuspended : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32>
        {
        }
        
        /// <summary>
        /// >> Challenged
        /// </summary>
        public sealed class Challenged : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32>
        {
        }
        
        /// <summary>
        /// >> Vote
        /// </summary>
        public sealed class Vote : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.Types.Primitive.Bool>
        {
        }
        
        /// <summary>
        /// >> DefenderVote
        /// </summary>
        public sealed class DefenderVote : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.Types.Primitive.Bool>
        {
        }
        
        /// <summary>
        /// >> NewMaxMembers
        /// </summary>
        public sealed class NewMaxMembers : BaseTuple<SubstrateNetApi.Model.Types.Primitive.U32>
        {
        }
        
        /// <summary>
        /// >> Unfounded
        /// </summary>
        public sealed class Unfounded : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32>
        {
        }
        
        /// <summary>
        /// >> Deposit
        /// </summary>
        public sealed class Deposit : BaseTuple<SubstrateNetApi.Model.Types.Primitive.U128>
        {
        }
    }
}
