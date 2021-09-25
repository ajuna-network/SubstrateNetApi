//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.SpArithmetic;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.PalletGilt
{
    
    
    /// <summary>
    /// >> 493 - Composite[pallet_gilt.pallet.ActiveGilt]
    /// </summary>
    public sealed class ActiveGilt : BaseType
    {
        
        /// <summary>
        /// >> proportion
        /// </summary>
        private SubstrateNetApi.Model.SpArithmetic.Perquintill _proportion;
        
        /// <summary>
        /// >> amount
        /// </summary>
        private SubstrateNetApi.Model.Types.Primitive.U128 _amount;
        
        /// <summary>
        /// >> who
        /// </summary>
        private SubstrateNetApi.Model.SpCore.AccountId32 _who;
        
        /// <summary>
        /// >> expiry
        /// </summary>
        private SubstrateNetApi.Model.Types.Primitive.U32 _expiry;
        
        public SubstrateNetApi.Model.SpArithmetic.Perquintill Proportion
        {
            get
            {
                return this._proportion;
            }
            set
            {
                this._proportion = value;
            }
        }
        
        public SubstrateNetApi.Model.Types.Primitive.U128 Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }
        
        public SubstrateNetApi.Model.SpCore.AccountId32 Who
        {
            get
            {
                return this._who;
            }
            set
            {
                this._who = value;
            }
        }
        
        public SubstrateNetApi.Model.Types.Primitive.U32 Expiry
        {
            get
            {
                return this._expiry;
            }
            set
            {
                this._expiry = value;
            }
        }
        
        public override string TypeName()
        {
            return "ActiveGilt";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Proportion.Encode());
            result.AddRange(Amount.Encode());
            result.AddRange(Who.Encode());
            result.AddRange(Expiry.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Proportion = new SubstrateNetApi.Model.SpArithmetic.Perquintill();
            Proportion.Decode(byteArray, ref p);
            Amount = new SubstrateNetApi.Model.Types.Primitive.U128();
            Amount.Decode(byteArray, ref p);
            Who = new SubstrateNetApi.Model.SpCore.AccountId32();
            Who.Decode(byteArray, ref p);
            Expiry = new SubstrateNetApi.Model.Types.Primitive.U32();
            Expiry.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
