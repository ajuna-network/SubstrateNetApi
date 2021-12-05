//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Custom.Runtime;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Composite;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Sequence;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.Types.Composite
{
    
    
    /// <summary>
    /// >> Path: pallet_gilt.pallet.ActiveGilt
    /// </summary>
    public sealed class ActiveGilt : BaseType
    {
        
        private Perquintill _proportion;
        
        private U128 _amount;
        
        private AccountId32 _who;
        
        private U32 _expiry;
        
        public Perquintill Proportion
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
        
        public U128 Amount
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
        
        public AccountId32 Who
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
        
        public U32 Expiry
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
            Proportion = new Perquintill();
            Proportion.Decode(byteArray, ref p);
            Amount = new U128();
            Amount.Decode(byteArray, ref p);
            Who = new AccountId32();
            Who.Decode(byteArray, ref p);
            Expiry = new U32();
            Expiry.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}