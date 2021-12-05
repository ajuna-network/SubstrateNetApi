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
    /// >> Path: pallet_bounties.Call
    /// Dispatchable calls.
    /// 
    /// Each variant of this enum maps to a dispatchable function from the associated module.
    /// </summary>
    public sealed class PalletBounties
    {
        
        /// <summary>
        /// >> Extrinsic: propose_bounty
        /// Propose a new bounty.
        /// 
        /// The dispatch origin for this call must be _Signed_.
        /// 
        /// Payment: `TipReportDepositBase` will be reserved from the origin account, as well as
        /// `DataDepositPerByte` for each byte in `reason`. It will be unreserved upon approval,
        /// or slashed when rejected.
        /// 
        /// - `curator`: The curator account whom will manage this bounty.
        /// - `fee`: The curator fee.
        /// - `value`: The total payment amount of this bounty, curator fee included.
        /// - `description`: The description of this bounty.
        /// </summary>
        public GenericExtrinsicCall ProposeBounty(BaseCom<U128> value, BaseVec<U8> description)
        {
            return new GenericExtrinsicCall("Bounties", "propose_bounty", value, description);
        }
        
        /// <summary>
        /// >> Extrinsic: approve_bounty
        /// Approve a bounty proposal. At a later time, the bounty will be funded and become active
        /// and the original deposit will be returned.
        /// 
        /// May only be called from `T::ApproveOrigin`.
        /// 
        /// # <weight>
        /// - O(1).
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall ApproveBounty(BaseCom<U32> bounty_id)
        {
            return new GenericExtrinsicCall("Bounties", "approve_bounty", bounty_id);
        }
        
        /// <summary>
        /// >> Extrinsic: propose_curator
        /// Assign a curator to a funded bounty.
        /// 
        /// May only be called from `T::ApproveOrigin`.
        /// 
        /// # <weight>
        /// - O(1).
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall ProposeCurator(BaseCom<U32> bounty_id, EnumMultiAddress curator, BaseCom<U128> fee)
        {
            return new GenericExtrinsicCall("Bounties", "propose_curator", bounty_id, curator, fee);
        }
        
        /// <summary>
        /// >> Extrinsic: unassign_curator
        /// Unassign curator from a bounty.
        /// 
        /// This function can only be called by the `RejectOrigin` a signed origin.
        /// 
        /// If this function is called by the `RejectOrigin`, we assume that the curator is malicious
        /// or inactive. As a result, we will slash the curator when possible.
        /// 
        /// If the origin is the curator, we take this as a sign they are unable to do their job and
        /// they willingly give up. We could slash them, but for now we allow them to recover their
        /// deposit and exit without issue. (We may want to change this if it is abused.)
        /// 
        /// Finally, the origin can be anyone if and only if the curator is "inactive". This allows
        /// anyone in the community to call out that a curator is not doing their due diligence, and
        /// we should pick a new curator. In this case the curator should also be slashed.
        /// 
        /// # <weight>
        /// - O(1).
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall UnassignCurator(BaseCom<U32> bounty_id)
        {
            return new GenericExtrinsicCall("Bounties", "unassign_curator", bounty_id);
        }
        
        /// <summary>
        /// >> Extrinsic: accept_curator
        /// Accept the curator role for a bounty.
        /// A deposit will be reserved from curator and refund upon successful payout.
        /// 
        /// May only be called from the curator.
        /// 
        /// # <weight>
        /// - O(1).
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall AcceptCurator(BaseCom<U32> bounty_id)
        {
            return new GenericExtrinsicCall("Bounties", "accept_curator", bounty_id);
        }
        
        /// <summary>
        /// >> Extrinsic: award_bounty
        /// Award bounty to a beneficiary account. The beneficiary will be able to claim the funds after a delay.
        /// 
        /// The dispatch origin for this call must be the curator of this bounty.
        /// 
        /// - `bounty_id`: Bounty ID to award.
        /// - `beneficiary`: The beneficiary account whom will receive the payout.
        /// 
        /// # <weight>
        /// - O(1).
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall AwardBounty(BaseCom<U32> bounty_id, EnumMultiAddress beneficiary)
        {
            return new GenericExtrinsicCall("Bounties", "award_bounty", bounty_id, beneficiary);
        }
        
        /// <summary>
        /// >> Extrinsic: claim_bounty
        /// Claim the payout from an awarded bounty after payout delay.
        /// 
        /// The dispatch origin for this call must be the beneficiary of this bounty.
        /// 
        /// - `bounty_id`: Bounty ID to claim.
        /// 
        /// # <weight>
        /// - O(1).
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall ClaimBounty(BaseCom<U32> bounty_id)
        {
            return new GenericExtrinsicCall("Bounties", "claim_bounty", bounty_id);
        }
        
        /// <summary>
        /// >> Extrinsic: close_bounty
        /// Cancel a proposed or active bounty. All the funds will be sent to treasury and
        /// the curator deposit will be unreserved if possible.
        /// 
        /// Only `T::RejectOrigin` is able to cancel a bounty.
        /// 
        /// - `bounty_id`: Bounty ID to cancel.
        /// 
        /// # <weight>
        /// - O(1).
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall CloseBounty(BaseCom<U32> bounty_id)
        {
            return new GenericExtrinsicCall("Bounties", "close_bounty", bounty_id);
        }
        
        /// <summary>
        /// >> Extrinsic: extend_bounty_expiry
        /// Extend the expiry time of an active bounty.
        /// 
        /// The dispatch origin for this call must be the curator of this bounty.
        /// 
        /// - `bounty_id`: Bounty ID to extend.
        /// - `remark`: additional information.
        /// 
        /// # <weight>
        /// - O(1).
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall ExtendBountyExpiry(BaseCom<U32> bounty_id, BaseVec<U8> _remark)
        {
            return new GenericExtrinsicCall("Bounties", "extend_bounty_expiry", bounty_id, _remark);
        }
    }
}
