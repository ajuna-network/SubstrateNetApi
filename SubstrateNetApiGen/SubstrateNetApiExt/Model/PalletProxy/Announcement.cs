//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.PrimitiveTypes;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.PalletProxy
{
    
    
    /// <summary>
    /// >> 468 - Composite[pallet_proxy.Announcement]
    /// </summary>
    public sealed class Announcement : BaseType
    {
        
        /// <summary>
        /// >> real
        /// </summary>
        private SubstrateNetApi.Model.SpCore.AccountId32 _real;
        
        /// <summary>
        /// >> call_hash
        /// </summary>
        private SubstrateNetApi.Model.PrimitiveTypes.H256 _callHash;
        
        /// <summary>
        /// >> height
        /// </summary>
        private SubstrateNetApi.Model.Types.Primitive.U32 _height;
        
        public SubstrateNetApi.Model.SpCore.AccountId32 Real
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
        
        public SubstrateNetApi.Model.PrimitiveTypes.H256 CallHash
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
        
        public SubstrateNetApi.Model.Types.Primitive.U32 Height
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
            Real = new SubstrateNetApi.Model.SpCore.AccountId32();
            Real.Decode(byteArray, ref p);
            CallHash = new SubstrateNetApi.Model.PrimitiveTypes.H256();
            CallHash.Decode(byteArray, ref p);
            Height = new SubstrateNetApi.Model.Types.Primitive.U32();
            Height.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
