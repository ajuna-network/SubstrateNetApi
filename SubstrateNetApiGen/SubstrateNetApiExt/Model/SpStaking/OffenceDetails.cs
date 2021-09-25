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
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.SpStaking
{
    
    
    /// <summary>
    /// >> 422 - Composite[sp_staking.offence.OffenceDetails]
    /// </summary>
    public sealed class OffenceDetails : BaseType
    {
        
        /// <summary>
        /// >> offender
        /// </summary>
        private BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32,SubstrateNetApi.Model.PalletStaking.Exposure> _offender;
        
        /// <summary>
        /// >> reporters
        /// </summary>
        private BaseVec<SubstrateNetApi.Model.SpCore.AccountId32> _reporters;
        
        public BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32,SubstrateNetApi.Model.PalletStaking.Exposure> Offender
        {
            get
            {
                return this._offender;
            }
            set
            {
                this._offender = value;
            }
        }
        
        public BaseVec<SubstrateNetApi.Model.SpCore.AccountId32> Reporters
        {
            get
            {
                return this._reporters;
            }
            set
            {
                this._reporters = value;
            }
        }
        
        public override string TypeName()
        {
            return "OffenceDetails";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Offender.Encode());
            result.AddRange(Reporters.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Offender = new BaseTuple<SubstrateNetApi.Model.SpCore.AccountId32,SubstrateNetApi.Model.PalletStaking.Exposure>();
            Offender.Decode(byteArray, ref p);
            Reporters = new BaseVec<SubstrateNetApi.Model.SpCore.AccountId32>();
            Reporters.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
