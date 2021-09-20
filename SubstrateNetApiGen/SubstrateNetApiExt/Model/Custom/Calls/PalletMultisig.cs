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
    /// >> Path: pallet_multisig.pallet.Call
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class PalletMultisig
    {
        
        /// <summary>
        /// >> Extrinsic: as_multi_threshold_1
        /// Immediately dispatch a multi-signature call using a single approval from the caller.
        /// 
        /// The dispatch origin for this call must be _Signed_.
        /// 
        /// - `other_signatories`: The accounts (other than the sender) who are part of the
        /// multi-signature, but do not participate in the approval process.
        /// - `call`: The call to be executed.
        /// 
        /// Result is equivalent to the dispatched result.
        /// 
        /// # <weight>
        /// O(Z + C) where Z is the length of the call and C its execution weight.
        /// -------------------------------
        /// - DB Weight: None
        /// - Plus Call Weight
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall AsMultiThreshold1(BaseVec<AccountId32> other_signatories, EnumNodeCall call)
        {
            return new GenericExtrinsicCall("Multisig", "as_multi_threshold_1", other_signatories, call);
        }
        
        /// <summary>
        /// >> Extrinsic: as_multi
        /// Register approval for a dispatch to be made from a deterministic composite account if
        /// approved by a total of `threshold - 1` of `other_signatories`.
        /// 
        /// If there are enough, then dispatch the call.
        /// 
        /// Payment: `DepositBase` will be reserved if this is the first approval, plus
        /// `threshold` times `DepositFactor`. It is returned once this dispatch happens or
        /// is cancelled.
        /// 
        /// The dispatch origin for this call must be _Signed_.
        /// 
        /// - `threshold`: The total number of approvals for this dispatch before it is executed.
        /// - `other_signatories`: The accounts (other than the sender) who can approve this
        /// dispatch. May not be empty.
        /// - `maybe_timepoint`: If this is the first approval, then this must be `None`. If it is
        /// not the first approval, then it must be `Some`, with the timepoint (block number and
        /// transaction index) of the first approval transaction.
        /// - `call`: The call to be executed.
        /// 
        /// NOTE: Unless this is the final approval, you will generally want to use
        /// `approve_as_multi` instead, since it only requires a hash of the call.
        /// 
        /// Result is equivalent to the dispatched result if `threshold` is exactly `1`. Otherwise
        /// on success, result is `Ok` and the result from the interior call, if it was executed,
        /// may be found in the deposited `MultisigExecuted` event.
        /// 
        /// # <weight>
        /// - `O(S + Z + Call)`.
        /// - Up to one balance-reserve or unreserve operation.
        /// - One passthrough operation, one insert, both `O(S)` where `S` is the number of
        ///   signatories. `S` is capped by `MaxSignatories`, with weight being proportional.
        /// - One call encode & hash, both of complexity `O(Z)` where `Z` is tx-len.
        /// - One encode & hash, both of complexity `O(S)`.
        /// - Up to one binary search and insert (`O(logS + S)`).
        /// - I/O: 1 read `O(S)`, up to 1 mutate `O(S)`. Up to one remove.
        /// - One event.
        /// - The weight of the `call`.
        /// - Storage: inserts one item, value size bounded by `MaxSignatories`, with a deposit
        ///   taken for its lifetime of `DepositBase + threshold * DepositFactor`.
        /// -------------------------------
        /// - DB Weight:
        ///     - Reads: Multisig Storage, [Caller Account], Calls (if `store_call`)
        ///     - Writes: Multisig Storage, [Caller Account], Calls (if `store_call`)
        /// - Plus Call Weight
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall AsMulti(U16 threshold, BaseVec<AccountId32> other_signatories, BaseOpt<Timepoint> maybe_timepoint, BaseVec<U8> call, Bool store_call, U64 max_weight)
        {
            return new GenericExtrinsicCall("Multisig", "as_multi", threshold, other_signatories, maybe_timepoint, call, store_call, max_weight);
        }
        
        /// <summary>
        /// >> Extrinsic: approve_as_multi
        /// Register approval for a dispatch to be made from a deterministic composite account if
        /// approved by a total of `threshold - 1` of `other_signatories`.
        /// 
        /// Payment: `DepositBase` will be reserved if this is the first approval, plus
        /// `threshold` times `DepositFactor`. It is returned once this dispatch happens or
        /// is cancelled.
        /// 
        /// The dispatch origin for this call must be _Signed_.
        /// 
        /// - `threshold`: The total number of approvals for this dispatch before it is executed.
        /// - `other_signatories`: The accounts (other than the sender) who can approve this
        /// dispatch. May not be empty.
        /// - `maybe_timepoint`: If this is the first approval, then this must be `None`. If it is
        /// not the first approval, then it must be `Some`, with the timepoint (block number and
        /// transaction index) of the first approval transaction.
        /// - `call_hash`: The hash of the call to be executed.
        /// 
        /// NOTE: If this is the final approval, you will want to use `as_multi` instead.
        /// 
        /// # <weight>
        /// - `O(S)`.
        /// - Up to one balance-reserve or unreserve operation.
        /// - One passthrough operation, one insert, both `O(S)` where `S` is the number of
        ///   signatories. `S` is capped by `MaxSignatories`, with weight being proportional.
        /// - One encode & hash, both of complexity `O(S)`.
        /// - Up to one binary search and insert (`O(logS + S)`).
        /// - I/O: 1 read `O(S)`, up to 1 mutate `O(S)`. Up to one remove.
        /// - One event.
        /// - Storage: inserts one item, value size bounded by `MaxSignatories`, with a deposit
        ///   taken for its lifetime of `DepositBase + threshold * DepositFactor`.
        /// ----------------------------------
        /// - DB Weight:
        ///     - Read: Multisig Storage, [Caller Account]
        ///     - Write: Multisig Storage, [Caller Account]
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall ApproveAsMulti(U16 threshold, BaseVec<AccountId32> other_signatories, BaseOpt<Timepoint> maybe_timepoint, Arr32U8 call_hash, U64 max_weight)
        {
            return new GenericExtrinsicCall("Multisig", "approve_as_multi", threshold, other_signatories, maybe_timepoint, call_hash, max_weight);
        }
        
        /// <summary>
        /// >> Extrinsic: cancel_as_multi
        /// Cancel a pre-existing, on-going multisig transaction. Any deposit reserved previously
        /// for this operation will be unreserved on success.
        /// 
        /// The dispatch origin for this call must be _Signed_.
        /// 
        /// - `threshold`: The total number of approvals for this dispatch before it is executed.
        /// - `other_signatories`: The accounts (other than the sender) who can approve this
        /// dispatch. May not be empty.
        /// - `timepoint`: The timepoint (block number and transaction index) of the first approval
        /// transaction for this dispatch.
        /// - `call_hash`: The hash of the call to be executed.
        /// 
        /// # <weight>
        /// - `O(S)`.
        /// - Up to one balance-reserve or unreserve operation.
        /// - One passthrough operation, one insert, both `O(S)` where `S` is the number of
        ///   signatories. `S` is capped by `MaxSignatories`, with weight being proportional.
        /// - One encode & hash, both of complexity `O(S)`.
        /// - One event.
        /// - I/O: 1 read `O(S)`, one remove.
        /// - Storage: removes one item.
        /// ----------------------------------
        /// - DB Weight:
        ///     - Read: Multisig Storage, [Caller Account], Refund Account, Calls
        ///     - Write: Multisig Storage, [Caller Account], Refund Account, Calls
        /// # </weight>
        /// </summary>
        public GenericExtrinsicCall CancelAsMulti(U16 threshold, BaseVec<AccountId32> other_signatories, Timepoint timepoint, Arr32U8 call_hash)
        {
            return new GenericExtrinsicCall("Multisig", "cancel_as_multi", threshold, other_signatories, timepoint, call_hash);
        }
    }
}
