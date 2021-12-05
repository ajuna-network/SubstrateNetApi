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
    /// >> Path: pallet_assets.types.AssetDetails
    /// </summary>
    public sealed class AssetDetails : BaseType
    {
        
        private AccountId32 _owner;
        
        private AccountId32 _issuer;
        
        private AccountId32 _admin;
        
        private AccountId32 _freezer;
        
        private U64 _supply;
        
        private U128 _deposit;
        
        private U64 _minBalance;
        
        private Bool _isSufficient;
        
        private U32 _accounts;
        
        private U32 _sufficients;
        
        private U32 _approvals;
        
        private Bool _isFrozen;
        
        public AccountId32 Owner
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
        
        public AccountId32 Issuer
        {
            get
            {
                return this._issuer;
            }
            set
            {
                this._issuer = value;
            }
        }
        
        public AccountId32 Admin
        {
            get
            {
                return this._admin;
            }
            set
            {
                this._admin = value;
            }
        }
        
        public AccountId32 Freezer
        {
            get
            {
                return this._freezer;
            }
            set
            {
                this._freezer = value;
            }
        }
        
        public U64 Supply
        {
            get
            {
                return this._supply;
            }
            set
            {
                this._supply = value;
            }
        }
        
        public U128 Deposit
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
        
        public U64 MinBalance
        {
            get
            {
                return this._minBalance;
            }
            set
            {
                this._minBalance = value;
            }
        }
        
        public Bool IsSufficient
        {
            get
            {
                return this._isSufficient;
            }
            set
            {
                this._isSufficient = value;
            }
        }
        
        public U32 Accounts
        {
            get
            {
                return this._accounts;
            }
            set
            {
                this._accounts = value;
            }
        }
        
        public U32 Sufficients
        {
            get
            {
                return this._sufficients;
            }
            set
            {
                this._sufficients = value;
            }
        }
        
        public U32 Approvals
        {
            get
            {
                return this._approvals;
            }
            set
            {
                this._approvals = value;
            }
        }
        
        public Bool IsFrozen
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
        
        public override string TypeName()
        {
            return "AssetDetails";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Owner.Encode());
            result.AddRange(Issuer.Encode());
            result.AddRange(Admin.Encode());
            result.AddRange(Freezer.Encode());
            result.AddRange(Supply.Encode());
            result.AddRange(Deposit.Encode());
            result.AddRange(MinBalance.Encode());
            result.AddRange(IsSufficient.Encode());
            result.AddRange(Accounts.Encode());
            result.AddRange(Sufficients.Encode());
            result.AddRange(Approvals.Encode());
            result.AddRange(IsFrozen.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Owner = new AccountId32();
            Owner.Decode(byteArray, ref p);
            Issuer = new AccountId32();
            Issuer.Decode(byteArray, ref p);
            Admin = new AccountId32();
            Admin.Decode(byteArray, ref p);
            Freezer = new AccountId32();
            Freezer.Decode(byteArray, ref p);
            Supply = new U64();
            Supply.Decode(byteArray, ref p);
            Deposit = new U128();
            Deposit.Decode(byteArray, ref p);
            MinBalance = new U64();
            MinBalance.Decode(byteArray, ref p);
            IsSufficient = new Bool();
            IsSufficient.Decode(byteArray, ref p);
            Accounts = new U32();
            Accounts.Decode(byteArray, ref p);
            Sufficients = new U32();
            Sufficients.Decode(byteArray, ref p);
            Approvals = new U32();
            Approvals.Decode(byteArray, ref p);
            IsFrozen = new Bool();
            IsFrozen.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
