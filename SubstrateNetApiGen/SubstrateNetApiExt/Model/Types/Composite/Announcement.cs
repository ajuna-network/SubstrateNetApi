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
    /// >> Path: pallet_proxy.Announcement
    /// </summary>
    public sealed class Announcement : BaseType
    {
        
        private AccountId32 _real;
        
        private H256 _callHash;
        
        private U32 _height;
        
        public AccountId32 Real
        {
            get
            {
                return this._real;
            }
            set
            {
                this._real = value;
            }
        }
        
        public H256 CallHash
        {
            get
            {
                return this._callHash;
            }
            set
            {
                this._callHash = value;
            }
        }
        
        public U32 Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }
        
        public override string TypeName()
        {
            return "Announcement";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Real.Encode());
            result.AddRange(CallHash.Encode());
            result.AddRange(Height.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Real = new AccountId32();
            Real.Decode(byteArray, ref p);
            CallHash = new H256();
            CallHash.Decode(byteArray, ref p);
            Height = new U32();
            Height.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
