//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Extrinsics;
using SubstrateNetApi.Model.FrameSupport;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.NodeRuntime;
using SubstrateNetApi.Model.PalletCollective;
using SubstrateNetApi.Model.PrimitiveTypes;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace SubstrateNetApi.Model.PalletTechnicalCommittee
{
    
    
    public sealed class TechnicalCommitteeStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateNetApi.SubstrateClient _client;
        
        public TechnicalCommitteeStorage(SubstrateNetApi.SubstrateClient client)
        {
            this._client = client;
        }
        
        public static string ProposalsParams()
        {
            return RequestGenerator.GetStorage("TechnicalCommittee", "Proposals", SubstrateNetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> Proposals
        ///  The hashes of the active proposals.
        /// </summary>
        public async Task<SubstrateNetApi.Model.FrameSupport.BoundedVec> Proposals(CancellationToken token)
        {
            string parameters = TechnicalCommitteeStorage.ProposalsParams();
            return await _client.GetStorageAsync<SubstrateNetApi.Model.FrameSupport.BoundedVec>(parameters, token);
        }
        
        public static string ProposalOfParams(SubstrateNetApi.Model.PrimitiveTypes.H256 key)
        {
            return RequestGenerator.GetStorage("TechnicalCommittee", "ProposalOf", SubstrateNetApi.Model.Meta.Storage.Type.Map, new SubstrateNetApi.Model.Meta.Storage.Hasher[] {
                        SubstrateNetApi.Model.Meta.Storage.Hasher.Identity}, new SubstrateNetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> ProposalOf
        ///  Actual proposal for a given hash, if it's current.
        /// </summary>
        public async Task<SubstrateNetApi.Model.NodeRuntime.EnumNodeCall> ProposalOf(SubstrateNetApi.Model.PrimitiveTypes.H256 key, CancellationToken token)
        {
            string parameters = TechnicalCommitteeStorage.ProposalOfParams(key);
            return await _client.GetStorageAsync<SubstrateNetApi.Model.NodeRuntime.EnumNodeCall>(parameters, token);
        }
        
        public static string VotingParams(SubstrateNetApi.Model.PrimitiveTypes.H256 key)
        {
            return RequestGenerator.GetStorage("TechnicalCommittee", "Voting", SubstrateNetApi.Model.Meta.Storage.Type.Map, new SubstrateNetApi.Model.Meta.Storage.Hasher[] {
                        SubstrateNetApi.Model.Meta.Storage.Hasher.Identity}, new SubstrateNetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> Voting
        ///  Votes on a given proposal, if it is ongoing.
        /// </summary>
        public async Task<SubstrateNetApi.Model.PalletCollective.Votes> Voting(SubstrateNetApi.Model.PrimitiveTypes.H256 key, CancellationToken token)
        {
            string parameters = TechnicalCommitteeStorage.VotingParams(key);
            return await _client.GetStorageAsync<SubstrateNetApi.Model.PalletCollective.Votes>(parameters, token);
        }
        
        public static string ProposalCountParams()
        {
            return RequestGenerator.GetStorage("TechnicalCommittee", "ProposalCount", SubstrateNetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> ProposalCount
        ///  Proposals so far.
        /// </summary>
        public async Task<SubstrateNetApi.Model.Types.Primitive.U32> ProposalCount(CancellationToken token)
        {
            string parameters = TechnicalCommitteeStorage.ProposalCountParams();
            return await _client.GetStorageAsync<SubstrateNetApi.Model.Types.Primitive.U32>(parameters, token);
        }
        
        public static string MembersParams()
        {
            return RequestGenerator.GetStorage("TechnicalCommittee", "Members", SubstrateNetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> Members
        ///  The current members of the collective. This is stored sorted (just by value).
        /// </summary>
        public async Task<BaseVec<SubstrateNetApi.Model.SpCore.AccountId32>> Members(CancellationToken token)
        {
            string parameters = TechnicalCommitteeStorage.MembersParams();
            return await _client.GetStorageAsync<BaseVec<SubstrateNetApi.Model.SpCore.AccountId32>>(parameters, token);
        }
        
        public static string PrimeParams()
        {
            return RequestGenerator.GetStorage("TechnicalCommittee", "Prime", SubstrateNetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> Prime
        ///  The prime member that helps determine the default vote behavior in case of absentations.
        /// </summary>
        public async Task<SubstrateNetApi.Model.SpCore.AccountId32> Prime(CancellationToken token)
        {
            string parameters = TechnicalCommitteeStorage.PrimeParams();
            return await _client.GetStorageAsync<SubstrateNetApi.Model.SpCore.AccountId32>(parameters, token);
        }
    }
    
    public sealed class TechnicalCommitteeCalls
    {
        
        /// <summary>
        /// >> set_members
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method SetMembers(BaseVec<SubstrateNetApi.Model.SpCore.AccountId32> new_members, BaseOpt<SubstrateNetApi.Model.SpCore.AccountId32> prime, SubstrateNetApi.Model.Types.Primitive.U32 old_count)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(new_members.Encode());
            byteArray.AddRange(prime.Encode());
            byteArray.AddRange(old_count.Encode());
            return new Method(13, "TechnicalCommittee", 0, "set_members", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> execute
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Execute(SubstrateNetApi.Model.NodeRuntime.EnumNodeCall proposal, BaseCom<SubstrateNetApi.Model.Types.Primitive.U32> length_bound)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(proposal.Encode());
            byteArray.AddRange(length_bound.Encode());
            return new Method(13, "TechnicalCommittee", 1, "execute", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> propose
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Propose(BaseCom<SubstrateNetApi.Model.Types.Primitive.U32> threshold, SubstrateNetApi.Model.NodeRuntime.EnumNodeCall proposal, BaseCom<SubstrateNetApi.Model.Types.Primitive.U32> length_bound)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(threshold.Encode());
            byteArray.AddRange(proposal.Encode());
            byteArray.AddRange(length_bound.Encode());
            return new Method(13, "TechnicalCommittee", 2, "propose", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> vote
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Vote(SubstrateNetApi.Model.PrimitiveTypes.H256 proposal, BaseCom<SubstrateNetApi.Model.Types.Primitive.U32> index, SubstrateNetApi.Model.Types.Primitive.Bool approve)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(proposal.Encode());
            byteArray.AddRange(index.Encode());
            byteArray.AddRange(approve.Encode());
            return new Method(13, "TechnicalCommittee", 3, "vote", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> close
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Close(SubstrateNetApi.Model.PrimitiveTypes.H256 proposal_hash, BaseCom<SubstrateNetApi.Model.Types.Primitive.U32> index, BaseCom<SubstrateNetApi.Model.Types.Primitive.U64> proposal_weight_bound, BaseCom<SubstrateNetApi.Model.Types.Primitive.U32> length_bound)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(proposal_hash.Encode());
            byteArray.AddRange(index.Encode());
            byteArray.AddRange(proposal_weight_bound.Encode());
            byteArray.AddRange(length_bound.Encode());
            return new Method(13, "TechnicalCommittee", 4, "close", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> disapprove_proposal
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method DisapproveProposal(SubstrateNetApi.Model.PrimitiveTypes.H256 proposal_hash)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(proposal_hash.Encode());
            return new Method(13, "TechnicalCommittee", 5, "disapprove_proposal", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> Proposed
    /// A motion (given hash) has been proposed (by given account) with a threshold (given
    /// `MemberCount`).
    /// \[account, proposal_index, proposal_hash, threshold\]
    /// </summary>
    public sealed class EventProposed : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.Types.Primitive.U32, SubstrateNetApi.Model.PrimitiveTypes.H256, SubstrateNetApi.Model.Types.Primitive.U32>
    {
    }
    
    /// <summary>
    /// >> Voted
    /// A motion (given hash) has been voted on by given account, leaving
    /// a tally (yes votes and no votes given respectively as `MemberCount`).
    /// \[account, proposal_hash, voted, yes, no\]
    /// </summary>
    public sealed class EventVoted : BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32, SubstrateNetApi.Model.PrimitiveTypes.H256, SubstrateNetApi.Model.Types.Primitive.Bool, SubstrateNetApi.Model.Types.Primitive.U32, SubstrateNetApi.Model.Types.Primitive.U32>
    {
    }
    
    /// <summary>
    /// >> Approved
    /// A motion was approved by the required threshold.
    /// \[proposal_hash\]
    /// </summary>
    public sealed class EventApproved : BaseTuple<SubstrateNetApi.Model.PrimitiveTypes.H256>
    {
    }
    
    /// <summary>
    /// >> Disapproved
    /// A motion was not approved by the required threshold.
    /// \[proposal_hash\]
    /// </summary>
    public sealed class EventDisapproved : BaseTuple<SubstrateNetApi.Model.PrimitiveTypes.H256>
    {
    }
    
    /// <summary>
    /// >> Executed
    /// A motion was executed; result will be `Ok` if it returned without error.
    /// \[proposal_hash, result\]
    /// </summary>
    public sealed class EventExecuted : BaseTuple<SubstrateNetApi.Model.PrimitiveTypes.H256, BaseTuple<BaseTuple,  SubstrateNetApi.Model.SpRuntime.EnumDispatchError>>
    {
    }
    
    /// <summary>
    /// >> MemberExecuted
    /// A single member did some action; result will be `Ok` if it returned without error.
    /// \[proposal_hash, result\]
    /// </summary>
    public sealed class EventMemberExecuted : BaseTuple<SubstrateNetApi.Model.PrimitiveTypes.H256, BaseTuple<BaseTuple,  SubstrateNetApi.Model.SpRuntime.EnumDispatchError>>
    {
    }
    
    /// <summary>
    /// >> Closed
    /// A proposal was closed because its threshold was reached or after its duration was up.
    /// \[proposal_hash, yes, no\]
    /// </summary>
    public sealed class EventClosed : BaseTuple<SubstrateNetApi.Model.PrimitiveTypes.H256, SubstrateNetApi.Model.Types.Primitive.U32, SubstrateNetApi.Model.Types.Primitive.U32>
    {
    }
    
    public enum TechnicalCommitteeErrors
    {
        
        /// <summary>
        /// >> NotMember
        /// Account is not a member
        /// </summary>
        NotMember,
        
        /// <summary>
        /// >> DuplicateProposal
        /// Duplicate proposals not allowed
        /// </summary>
        DuplicateProposal,
        
        /// <summary>
        /// >> ProposalMissing
        /// Proposal must exist
        /// </summary>
        ProposalMissing,
        
        /// <summary>
        /// >> WrongIndex
        /// Mismatched index
        /// </summary>
        WrongIndex,
        
        /// <summary>
        /// >> DuplicateVote
        /// Duplicate vote ignored
        /// </summary>
        DuplicateVote,
        
        /// <summary>
        /// >> AlreadyInitialized
        /// Members are already initialized!
        /// </summary>
        AlreadyInitialized,
        
        /// <summary>
        /// >> TooEarly
        /// The close call was made too early, before the end of the voting.
        /// </summary>
        TooEarly,
        
        /// <summary>
        /// >> TooManyProposals
        /// There can only be a maximum of `MaxProposals` active proposals.
        /// </summary>
        TooManyProposals,
        
        /// <summary>
        /// >> WrongProposalWeight
        /// The given weight bound for the proposal was too low.
        /// </summary>
        WrongProposalWeight,
        
        /// <summary>
        /// >> WrongProposalLength
        /// The given length bound for the proposal was too low.
        /// </summary>
        WrongProposalLength,
    }
}
