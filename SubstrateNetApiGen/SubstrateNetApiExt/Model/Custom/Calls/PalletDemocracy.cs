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
    /// >> Path: pallet_democracy.pallet.Call
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class PalletDemocracy
    {
        
        /// <summary>
        /// >> Extrinsic: propose
        /// Propose a sensitive action to be taken.
        /// 
        /// The dispatch origin of this call must be _Signed_ and the sender must
        /// have funds to cover the deposit.
        /// 
        /// - `proposal_hash`: The hash of the proposal preimage.
        /// - `value`: The amount of deposit (must be at least `MinimumDeposit`).
        /// 
        /// Emits `Proposed`.
        /// 
        /// Weight: `O(p)`
        /// </summary>
        public GenericExtrinsicCall Propose(H256 proposal_hash, BaseCom<U128> value)
        {
            return new GenericExtrinsicCall("Democracy", "propose", proposal_hash, value);
        }
        
        /// <summary>
        /// >> Extrinsic: second
        /// Signals agreement with a particular proposal.
        /// 
        /// The dispatch origin of this call must be _Signed_ and the sender
        /// must have funds to cover the deposit, equal to the original deposit.
        /// 
        /// - `proposal`: The index of the proposal to second.
        /// - `seconds_upper_bound`: an upper bound on the current number of seconds on this
        ///   proposal. Extrinsic is weighted according to this value with no refund.
        /// 
        /// Weight: `O(S)` where S is the number of seconds a proposal already has.
        /// </summary>
        public GenericExtrinsicCall Second(BaseCom<U32> proposal, BaseCom<U32> seconds_upper_bound)
        {
            return new GenericExtrinsicCall("Democracy", "second", proposal, seconds_upper_bound);
        }
        
        /// <summary>
        /// >> Extrinsic: vote
        /// Vote in a referendum. If `vote.is_aye()`, the vote is to enact the proposal;
        /// otherwise it is a vote to keep the status quo.
        /// 
        /// The dispatch origin of this call must be _Signed_.
        /// 
        /// - `ref_index`: The index of the referendum to vote for.
        /// - `vote`: The vote configuration.
        /// 
        /// Weight: `O(R)` where R is the number of referendums the voter has voted on.
        /// </summary>
        public GenericExtrinsicCall Vote(BaseCom<U32> ref_index, EnumAccountVote vote)
        {
            return new GenericExtrinsicCall("Democracy", "vote", ref_index, vote);
        }
        
        /// <summary>
        /// >> Extrinsic: emergency_cancel
        /// Schedule an emergency cancellation of a referendum. Cannot happen twice to the same
        /// referendum.
        /// 
        /// The dispatch origin of this call must be `CancellationOrigin`.
        /// 
        /// -`ref_index`: The index of the referendum to cancel.
        /// 
        /// Weight: `O(1)`.
        /// </summary>
        public GenericExtrinsicCall EmergencyCancel(U32 ref_index)
        {
            return new GenericExtrinsicCall("Democracy", "emergency_cancel", ref_index);
        }
        
        /// <summary>
        /// >> Extrinsic: external_propose
        /// Schedule a referendum to be tabled once it is legal to schedule an external
        /// referendum.
        /// 
        /// The dispatch origin of this call must be `ExternalOrigin`.
        /// 
        /// - `proposal_hash`: The preimage hash of the proposal.
        /// 
        /// Weight: `O(V)` with V number of vetoers in the blacklist of proposal.
        ///   Decoding vec of length V. Charged as maximum
        /// </summary>
        public GenericExtrinsicCall ExternalPropose(H256 proposal_hash)
        {
            return new GenericExtrinsicCall("Democracy", "external_propose", proposal_hash);
        }
        
        /// <summary>
        /// >> Extrinsic: external_propose_majority
        /// Schedule a majority-carries referendum to be tabled next once it is legal to schedule
        /// an external referendum.
        /// 
        /// The dispatch of this call must be `ExternalMajorityOrigin`.
        /// 
        /// - `proposal_hash`: The preimage hash of the proposal.
        /// 
        /// Unlike `external_propose`, blacklisting has no effect on this and it may replace a
        /// pre-scheduled `external_propose` call.
        /// 
        /// Weight: `O(1)`
        /// </summary>
        public GenericExtrinsicCall ExternalProposeMajority(H256 proposal_hash)
        {
            return new GenericExtrinsicCall("Democracy", "external_propose_majority", proposal_hash);
        }
        
        /// <summary>
        /// >> Extrinsic: external_propose_default
        /// Schedule a negative-turnout-bias referendum to be tabled next once it is legal to
        /// schedule an external referendum.
        /// 
        /// The dispatch of this call must be `ExternalDefaultOrigin`.
        /// 
        /// - `proposal_hash`: The preimage hash of the proposal.
        /// 
        /// Unlike `external_propose`, blacklisting has no effect on this and it may replace a
        /// pre-scheduled `external_propose` call.
        /// 
        /// Weight: `O(1)`
        /// </summary>
        public GenericExtrinsicCall ExternalProposeDefault(H256 proposal_hash)
        {
            return new GenericExtrinsicCall("Democracy", "external_propose_default", proposal_hash);
        }
        
        /// <summary>
        /// >> Extrinsic: fast_track
        /// Schedule the currently externally-proposed majority-carries referendum to be tabled
        /// immediately. If there is no externally-proposed referendum currently, or if there is one
        /// but it is not a majority-carries referendum then it fails.
        /// 
        /// The dispatch of this call must be `FastTrackOrigin`.
        /// 
        /// - `proposal_hash`: The hash of the current external proposal.
        /// - `voting_period`: The period that is allowed for voting on this proposal. Increased to
        ///   `FastTrackVotingPeriod` if too low.
        /// - `delay`: The number of block after voting has ended in approval and this should be
        ///   enacted. This doesn't have a minimum amount.
        /// 
        /// Emits `Started`.
        /// 
        /// Weight: `O(1)`
        /// </summary>
        public GenericExtrinsicCall FastTrack(H256 proposal_hash, U32 voting_period, U32 delay)
        {
            return new GenericExtrinsicCall("Democracy", "fast_track", proposal_hash, voting_period, delay);
        }
        
        /// <summary>
        /// >> Extrinsic: veto_external
        /// Veto and blacklist the external proposal hash.
        /// 
        /// The dispatch origin of this call must be `VetoOrigin`.
        /// 
        /// - `proposal_hash`: The preimage hash of the proposal to veto and blacklist.
        /// 
        /// Emits `Vetoed`.
        /// 
        /// Weight: `O(V + log(V))` where V is number of `existing vetoers`
        /// </summary>
        public GenericExtrinsicCall VetoExternal(H256 proposal_hash)
        {
            return new GenericExtrinsicCall("Democracy", "veto_external", proposal_hash);
        }
        
        /// <summary>
        /// >> Extrinsic: cancel_referendum
        /// Remove a referendum.
        /// 
        /// The dispatch origin of this call must be _Root_.
        /// 
        /// - `ref_index`: The index of the referendum to cancel.
        /// 
        /// # Weight: `O(1)`.
        /// </summary>
        public GenericExtrinsicCall CancelReferendum(BaseCom<U32> ref_index)
        {
            return new GenericExtrinsicCall("Democracy", "cancel_referendum", ref_index);
        }
        
        /// <summary>
        /// >> Extrinsic: cancel_queued
        /// Cancel a proposal queued for enactment.
        /// 
        /// The dispatch origin of this call must be _Root_.
        /// 
        /// - `which`: The index of the referendum to cancel.
        /// 
        /// Weight: `O(D)` where `D` is the items in the dispatch queue. Weighted as `D = 10`.
        /// </summary>
        public GenericExtrinsicCall CancelQueued(U32 which)
        {
            return new GenericExtrinsicCall("Democracy", "cancel_queued", which);
        }
        
        /// <summary>
        /// >> Extrinsic: delegate
        /// Delegate the voting power (with some given conviction) of the sending account.
        /// 
        /// The balance delegated is locked for as long as it's delegated, and thereafter for the
        /// time appropriate for the conviction's lock period.
        /// 
        /// The dispatch origin of this call must be _Signed_, and the signing account must either:
        ///   - be delegating already; or
        ///   - have no voting activity (if there is, then it will need to be removed/consolidated
        ///     through `reap_vote` or `unvote`).
        /// 
        /// - `to`: The account whose voting the `target` account's voting power will follow.
        /// - `conviction`: The conviction that will be attached to the delegated votes. When the
        ///   account is undelegated, the funds will be locked for the corresponding period.
        /// - `balance`: The amount of the account's balance to be used in delegating. This must not
        ///   be more than the account's current balance.
        /// 
        /// Emits `Delegated`.
        /// 
        /// Weight: `O(R)` where R is the number of referendums the voter delegating to has
        ///   voted on. Weight is charged as if maximum votes.
        /// </summary>
        public GenericExtrinsicCall Delegate(AccountId32 to, EnumConviction conviction, U128 balance)
        {
            return new GenericExtrinsicCall("Democracy", "delegate", to, conviction, balance);
        }
        
        /// <summary>
        /// >> Extrinsic: undelegate
        /// Undelegate the voting power of the sending account.
        /// 
        /// Tokens may be unlocked following once an amount of time consistent with the lock period
        /// of the conviction with which the delegation was issued.
        /// 
        /// The dispatch origin of this call must be _Signed_ and the signing account must be
        /// currently delegating.
        /// 
        /// Emits `Undelegated`.
        /// 
        /// Weight: `O(R)` where R is the number of referendums the voter delegating to has
        ///   voted on. Weight is charged as if maximum votes.
        /// </summary>
        public GenericExtrinsicCall Undelegate()
        {
            return new GenericExtrinsicCall("Democracy", "undelegate");
        }
        
        /// <summary>
        /// >> Extrinsic: clear_public_proposals
        /// Clears all public proposals.
        /// 
        /// The dispatch origin of this call must be _Root_.
        /// 
        /// Weight: `O(1)`.
        /// </summary>
        public GenericExtrinsicCall ClearPublicProposals()
        {
            return new GenericExtrinsicCall("Democracy", "clear_public_proposals");
        }
        
        /// <summary>
        /// >> Extrinsic: note_preimage
        /// Register the preimage for an upcoming proposal. This doesn't require the proposal to be
        /// in the dispatch queue but does require a deposit, returned once enacted.
        /// 
        /// The dispatch origin of this call must be _Signed_.
        /// 
        /// - `encoded_proposal`: The preimage of a proposal.
        /// 
        /// Emits `PreimageNoted`.
        /// 
        /// Weight: `O(E)` with E size of `encoded_proposal` (protected by a required deposit).
        /// </summary>
        public GenericExtrinsicCall NotePreimage(BaseVec<U8> encoded_proposal)
        {
            return new GenericExtrinsicCall("Democracy", "note_preimage", encoded_proposal);
        }
        
        /// <summary>
        /// >> Extrinsic: note_preimage_operational
        /// Same as `note_preimage` but origin is `OperationalPreimageOrigin`.
        /// </summary>
        public GenericExtrinsicCall NotePreimageOperational(BaseVec<U8> encoded_proposal)
        {
            return new GenericExtrinsicCall("Democracy", "note_preimage_operational", encoded_proposal);
        }
        
        /// <summary>
        /// >> Extrinsic: note_imminent_preimage
        /// Register the preimage for an upcoming proposal. This requires the proposal to be
        /// in the dispatch queue. No deposit is needed. When this call is successful, i.e.
        /// the preimage has not been uploaded before and matches some imminent proposal,
        /// no fee is paid.
        /// 
        /// The dispatch origin of this call must be _Signed_.
        /// 
        /// - `encoded_proposal`: The preimage of a proposal.
        /// 
        /// Emits `PreimageNoted`.
        /// 
        /// Weight: `O(E)` with E size of `encoded_proposal` (protected by a required deposit).
        /// </summary>
        public GenericExtrinsicCall NoteImminentPreimage(BaseVec<U8> encoded_proposal)
        {
            return new GenericExtrinsicCall("Democracy", "note_imminent_preimage", encoded_proposal);
        }
        
        /// <summary>
        /// >> Extrinsic: note_imminent_preimage_operational
        /// Same as `note_imminent_preimage` but origin is `OperationalPreimageOrigin`.
        /// </summary>
        public GenericExtrinsicCall NoteImminentPreimageOperational(BaseVec<U8> encoded_proposal)
        {
            return new GenericExtrinsicCall("Democracy", "note_imminent_preimage_operational", encoded_proposal);
        }
        
        /// <summary>
        /// >> Extrinsic: reap_preimage
        /// Remove an expired proposal preimage and collect the deposit.
        /// 
        /// The dispatch origin of this call must be _Signed_.
        /// 
        /// - `proposal_hash`: The preimage hash of a proposal.
        /// - `proposal_length_upper_bound`: an upper bound on length of the proposal. Extrinsic is
        ///   weighted according to this value with no refund.
        /// 
        /// This will only work after `VotingPeriod` blocks from the time that the preimage was
        /// noted, if it's the same account doing it. If it's a different account, then it'll only
        /// work an additional `EnactmentPeriod` later.
        /// 
        /// Emits `PreimageReaped`.
        /// 
        /// Weight: `O(D)` where D is length of proposal.
        /// </summary>
        public GenericExtrinsicCall ReapPreimage(H256 proposal_hash, BaseCom<U32> proposal_len_upper_bound)
        {
            return new GenericExtrinsicCall("Democracy", "reap_preimage", proposal_hash, proposal_len_upper_bound);
        }
        
        /// <summary>
        /// >> Extrinsic: unlock
        /// Unlock tokens that have an expired lock.
        /// 
        /// The dispatch origin of this call must be _Signed_.
        /// 
        /// - `target`: The account to remove the lock on.
        /// 
        /// Weight: `O(R)` with R number of vote of target.
        /// </summary>
        public GenericExtrinsicCall Unlock(AccountId32 target)
        {
            return new GenericExtrinsicCall("Democracy", "unlock", target);
        }
        
        /// <summary>
        /// >> Extrinsic: remove_vote
        /// Remove a vote for a referendum.
        /// 
        /// If:
        /// - the referendum was cancelled, or
        /// - the referendum is ongoing, or
        /// - the referendum has ended such that
        ///   - the vote of the account was in opposition to the result; or
        ///   - there was no conviction to the account's vote; or
        ///   - the account made a split vote
        /// ...then the vote is removed cleanly and a following call to `unlock` may result in more
        /// funds being available.
        /// 
        /// If, however, the referendum has ended and:
        /// - it finished corresponding to the vote of the account, and
        /// - the account made a standard vote with conviction, and
        /// - the lock period of the conviction is not over
        /// ...then the lock will be aggregated into the overall account's lock, which may involve
        /// *overlocking* (where the two locks are combined into a single lock that is the maximum
        /// of both the amount locked and the time is it locked for).
        /// 
        /// The dispatch origin of this call must be _Signed_, and the signer must have a vote
        /// registered for referendum `index`.
        /// 
        /// - `index`: The index of referendum of the vote to be removed.
        /// 
        /// Weight: `O(R + log R)` where R is the number of referenda that `target` has voted on.
        ///   Weight is calculated for the maximum number of vote.
        /// </summary>
        public GenericExtrinsicCall RemoveVote(U32 index)
        {
            return new GenericExtrinsicCall("Democracy", "remove_vote", index);
        }
        
        /// <summary>
        /// >> Extrinsic: remove_other_vote
        /// Remove a vote for a referendum.
        /// 
        /// If the `target` is equal to the signer, then this function is exactly equivalent to
        /// `remove_vote`. If not equal to the signer, then the vote must have expired,
        /// either because the referendum was cancelled, because the voter lost the referendum or
        /// because the conviction period is over.
        /// 
        /// The dispatch origin of this call must be _Signed_.
        /// 
        /// - `target`: The account of the vote to be removed; this account must have voted for
        ///   referendum `index`.
        /// - `index`: The index of referendum of the vote to be removed.
        /// 
        /// Weight: `O(R + log R)` where R is the number of referenda that `target` has voted on.
        ///   Weight is calculated for the maximum number of vote.
        /// </summary>
        public GenericExtrinsicCall RemoveOtherVote(AccountId32 target, U32 index)
        {
            return new GenericExtrinsicCall("Democracy", "remove_other_vote", target, index);
        }
        
        /// <summary>
        /// >> Extrinsic: enact_proposal
        /// Enact a proposal from a referendum. For now we just make the weight be the maximum.
        /// </summary>
        public GenericExtrinsicCall EnactProposal(H256 proposal_hash, U32 index)
        {
            return new GenericExtrinsicCall("Democracy", "enact_proposal", proposal_hash, index);
        }
        
        /// <summary>
        /// >> Extrinsic: blacklist
        /// Permanently place a proposal into the blacklist. This prevents it from ever being
        /// proposed again.
        /// 
        /// If called on a queued public or external proposal, then this will result in it being
        /// removed. If the `ref_index` supplied is an active referendum with the proposal hash,
        /// then it will be cancelled.
        /// 
        /// The dispatch origin of this call must be `BlacklistOrigin`.
        /// 
        /// - `proposal_hash`: The proposal hash to blacklist permanently.
        /// - `ref_index`: An ongoing referendum whose hash is `proposal_hash`, which will be
        /// cancelled.
        /// 
        /// Weight: `O(p)` (though as this is an high-privilege dispatch, we assume it has a
        ///   reasonable value).
        /// </summary>
        public GenericExtrinsicCall Blacklist(H256 proposal_hash, BaseOpt<U32> maybe_ref_index)
        {
            return new GenericExtrinsicCall("Democracy", "blacklist", proposal_hash, maybe_ref_index);
        }
        
        /// <summary>
        /// >> Extrinsic: cancel_proposal
        /// Remove a proposal.
        /// 
        /// The dispatch origin of this call must be `CancelProposalOrigin`.
        /// 
        /// - `prop_index`: The index of the proposal to cancel.
        /// 
        /// Weight: `O(p)` where `p = PublicProps::<T>::decode_len()`
        /// </summary>
        public GenericExtrinsicCall CancelProposal(BaseCom<U32> prop_index)
        {
            return new GenericExtrinsicCall("Democracy", "cancel_proposal", prop_index);
        }
    }
}
