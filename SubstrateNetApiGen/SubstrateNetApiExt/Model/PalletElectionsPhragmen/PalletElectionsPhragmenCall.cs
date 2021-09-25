//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.PalletElectionsPhragmen;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.SpRuntime;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.PalletElectionsPhragmen
{
    
    
    /// <summary>
    /// >> 215 - Variant[pallet_elections_phragmen.pallet.Call]
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class PalletElectionsPhragmenCall
    {
        
        /// <summary>
        /// >> vote
        /// </summary>
        public GenericExtrinsicCall Vote(BaseVec<SubstrateNetApi.Model.SpCore.AccountId32> votes, BaseCom<SubstrateNetApi.Model.Types.Primitive.U128> value)
        {
            return new GenericExtrinsicCall("ElectionsPhragmen", "vote", votes, value);
        }
        
        /// <summary>
        /// >> remove_voter
        /// </summary>
        public GenericExtrinsicCall RemoveVoter()
        {
            return new GenericExtrinsicCall("ElectionsPhragmen", "remove_voter");
        }
        
        /// <summary>
        /// >> submit_candidacy
        /// </summary>
        public GenericExtrinsicCall SubmitCandidacy(BaseCom<SubstrateNetApi.Model.Types.Primitive.U32> candidate_count)
        {
            return new GenericExtrinsicCall("ElectionsPhragmen", "submit_candidacy", candidate_count);
        }
        
        /// <summary>
        /// >> renounce_candidacy
        /// </summary>
        public GenericExtrinsicCall RenounceCandidacy(SubstrateNetApi.Model.PalletElectionsPhragmen.EnumRenouncing renouncing)
        {
            return new GenericExtrinsicCall("ElectionsPhragmen", "renounce_candidacy", renouncing);
        }
        
        /// <summary>
        /// >> remove_member
        /// </summary>
        public GenericExtrinsicCall RemoveMember(SubstrateNetApi.Model.SpRuntime.EnumMultiAddress who, SubstrateNetApi.Model.Types.Primitive.Bool has_replacement)
        {
            return new GenericExtrinsicCall("ElectionsPhragmen", "remove_member", who, has_replacement);
        }
        
        /// <summary>
        /// >> clean_defunct_voters
        /// </summary>
        public GenericExtrinsicCall CleanDefunctVoters(SubstrateNetApi.Model.Types.Primitive.U32 num_voters, SubstrateNetApi.Model.Types.Primitive.U32 num_defunct)
        {
            return new GenericExtrinsicCall("ElectionsPhragmen", "clean_defunct_voters", num_voters, num_defunct);
        }
    }
}
