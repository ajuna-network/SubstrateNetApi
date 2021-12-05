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
    /// >> Path: pallet_treasury.Proposal
    /// </summary>
    public sealed class Proposal : BaseType
    {
        
        private AccountId32 _proposer;
        
        private U128 _value;
        
        private AccountId32 _beneficiary;
        
        private U128 _bond;
        
        public AccountId32 Proposer
        {
            get
            {
                return this._proposer;
            }
            set
            {
                this._proposer = value;
            }
        }
        
        public U128 Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
        
        public AccountId32 Beneficiary
        {
            get
            {
                return this._beneficiary;
            }
            set
            {
                this._beneficiary = value;
            }
        }
        
        public U128 Bond
        {
            get
            {
                return this._bond;
            }
            set
            {
                this._bond = value;
            }
        }
        
        public override string TypeName()
        {
            return "Proposal";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Proposer.Encode());
            result.AddRange(Value.Encode());
            result.AddRange(Beneficiary.Encode());
            result.AddRange(Bond.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Proposer = new AccountId32();
            Proposer.Decode(byteArray, ref p);
            Value = new U128();
            Value.Decode(byteArray, ref p);
            Beneficiary = new AccountId32();
            Beneficiary.Decode(byteArray, ref p);
            Bond = new U128();
            Bond.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
