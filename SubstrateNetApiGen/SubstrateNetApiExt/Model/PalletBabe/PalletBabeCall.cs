//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.SpConsensusBabe;
using SubstrateNetApi.Model.SpConsensusSlots;
using SubstrateNetApi.Model.SpSession;
using SubstrateNetApi.Model.Types.Base;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.PalletBabe
{
    
    
    /// <summary>
    /// >> 121 - Variant[pallet_babe.pallet.Call]
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class PalletBabeCall
    {
        
        /// <summary>
        /// >> report_equivocation
        /// </summary>
        public GenericExtrinsicCall ReportEquivocation(SubstrateNetApi.Model.SpConsensusSlots.EquivocationProof equivocation_proof, SubstrateNetApi.Model.SpSession.MembershipProof key_owner_proof)
        {
            return new GenericExtrinsicCall("Babe", "report_equivocation", equivocation_proof, key_owner_proof);
        }
        
        /// <summary>
        /// >> report_equivocation_unsigned
        /// </summary>
        public GenericExtrinsicCall ReportEquivocationUnsigned(SubstrateNetApi.Model.SpConsensusSlots.EquivocationProof equivocation_proof, SubstrateNetApi.Model.SpSession.MembershipProof key_owner_proof)
        {
            return new GenericExtrinsicCall("Babe", "report_equivocation_unsigned", equivocation_proof, key_owner_proof);
        }
        
        /// <summary>
        /// >> plan_config_change
        /// </summary>
        public GenericExtrinsicCall PlanConfigChange(SubstrateNetApi.Model.SpConsensusBabe.EnumNextConfigDescriptor config)
        {
            return new GenericExtrinsicCall("Babe", "plan_config_change", config);
        }
    }
}
