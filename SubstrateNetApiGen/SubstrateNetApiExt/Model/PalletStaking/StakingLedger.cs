//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.PalletStaking;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.PalletStaking
{
    
    
    /// <summary>
    /// >> 345 - Composite[pallet_staking.StakingLedger]
    /// </summary>
    public sealed class StakingLedger : BaseType
    {
        
        /// <summary>
        /// >> stash
        /// </summary>
        private SubstrateNetApi.Model.SpCore.AccountId32 _stash;
        
        /// <summary>
        /// >> total
        /// </summary>
        private BaseCom<SubstrateNetApi.Model.Types.Primitive.U128> _total;
        
        /// <summary>
        /// >> active
        /// </summary>
        private BaseCom<SubstrateNetApi.Model.Types.Primitive.U128> _active;
        
        /// <summary>
        /// >> unlocking
        /// </summary>
        private BaseVec<SubstrateNetApi.Model.PalletStaking.UnlockChunk> _unlocking;
        
        /// <summary>
        /// >> claimed_rewards
        /// </summary>
        private BaseVec<SubstrateNetApi.Model.Types.Primitive.U32> _claimedRewards;
        
        public SubstrateNetApi.Model.SpCore.AccountId32 Stash
        {
            get
            {
                return this._stash;
            }
            set
            {
                this._stash = value;
            }
        }
        
        public BaseCom<SubstrateNetApi.Model.Types.Primitive.U128> Total
        {
            get
            {
                return this._total;
            }
            set
            {
                this._total = value;
            }
        }
        
        public BaseCom<SubstrateNetApi.Model.Types.Primitive.U128> Active
        {
            get
            {
                return this._active;
            }
            set
            {
                this._active = value;
            }
        }
        
        public BaseVec<SubstrateNetApi.Model.PalletStaking.UnlockChunk> Unlocking
        {
            get
            {
                return this._unlocking;
            }
            set
            {
                this._unlocking = value;
            }
        }
        
        public BaseVec<SubstrateNetApi.Model.Types.Primitive.U32> ClaimedRewards
        {
            get
            {
                return this._claimedRewards;
            }
            set
            {
                this._claimedRewards = value;
            }
        }
        
        public override string TypeName()
        {
            return "StakingLedger";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Stash.Encode());
            result.AddRange(Total.Encode());
            result.AddRange(Active.Encode());
            result.AddRange(Unlocking.Encode());
            result.AddRange(ClaimedRewards.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Stash = new SubstrateNetApi.Model.SpCore.AccountId32();
            Stash.Decode(byteArray, ref p);
            Total = new BaseCom<SubstrateNetApi.Model.Types.Primitive.U128>();
            Total.Decode(byteArray, ref p);
            Active = new BaseCom<SubstrateNetApi.Model.Types.Primitive.U128>();
            Active.Decode(byteArray, ref p);
            Unlocking = new BaseVec<SubstrateNetApi.Model.PalletStaking.UnlockChunk>();
            Unlocking.Decode(byteArray, ref p);
            ClaimedRewards = new BaseVec<SubstrateNetApi.Model.Types.Primitive.U32>();
            ClaimedRewards.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
