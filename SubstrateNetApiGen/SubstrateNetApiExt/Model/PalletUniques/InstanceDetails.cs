//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.PalletUniques
{
    
    
    /// <summary>
    /// >> 497 - Composite[pallet_uniques.types.InstanceDetails]
    /// </summary>
    public sealed class InstanceDetails : BaseType
    {
        
        /// <summary>
        /// >> owner
        /// </summary>
        private SubstrateNetApi.Model.SpCore.AccountId32 _owner;
        
        /// <summary>
        /// >> approved
        /// </summary>
        private BaseOpt<SubstrateNetApi.Model.SpCore.AccountId32> _approved;
        
        /// <summary>
        /// >> is_frozen
        /// </summary>
        private SubstrateNetApi.Model.Types.Primitive.Bool _isFrozen;
        
        /// <summary>
        /// >> deposit
        /// </summary>
        private SubstrateNetApi.Model.Types.Primitive.U128 _deposit;
        
        public SubstrateNetApi.Model.SpCore.AccountId32 Owner
        {
            get
            {
                return this._owner;
            }
            set
            {
                this._owner = value;
            }
        }
        
        public BaseOpt<SubstrateNetApi.Model.SpCore.AccountId32> Approved
        {
            get
            {
                return this._approved;
            }
            set
            {
                this._approved = value;
            }
        }
        
        public SubstrateNetApi.Model.Types.Primitive.Bool IsFrozen
        {
            get
            {
                return this._isFrozen;
            }
            set
            {
                this._isFrozen = value;
            }
        }
        
        public SubstrateNetApi.Model.Types.Primitive.U128 Deposit
        {
            get
            {
                return this._deposit;
            }
            set
            {
                this._deposit = value;
            }
        }
        
        public override string TypeName()
        {
            return "InstanceDetails";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Owner.Encode());
            result.AddRange(Approved.Encode());
            result.AddRange(IsFrozen.Encode());
            result.AddRange(Deposit.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Owner = new SubstrateNetApi.Model.SpCore.AccountId32();
            Owner.Decode(byteArray, ref p);
            Approved = new BaseOpt<SubstrateNetApi.Model.SpCore.AccountId32>();
            Approved.Decode(byteArray, ref p);
            IsFrozen = new SubstrateNetApi.Model.Types.Primitive.Bool();
            IsFrozen.Decode(byteArray, ref p);
            Deposit = new SubstrateNetApi.Model.Types.Primitive.U128();
            Deposit.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
