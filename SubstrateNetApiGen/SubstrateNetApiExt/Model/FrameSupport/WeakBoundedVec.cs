//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.SpFinalityGrandpa;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.FrameSupport
{
    
    
    /// <summary>
    /// >> 396 - Composite[frame_support.storage.weak_bounded_vec.WeakBoundedVec]
    /// </summary>
    public sealed class WeakBoundedVec : BaseType
    {
        
        /// <summary>
        /// >> value
        /// </summary>
        private BaseVec<BaseTuple<SubstrateNetApi.Model.SpFinalityGrandpa.Public,SubstrateNetApi.Model.Types.Primitive.U64>> _value;
        
        public BaseVec<BaseTuple<SubstrateNetApi.Model.SpFinalityGrandpa.Public,SubstrateNetApi.Model.Types.Primitive.U64>> Value
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
        
        public override string TypeName()
        {
            return "WeakBoundedVec";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Value.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Value = new BaseVec<BaseTuple<SubstrateNetApi.Model.SpFinalityGrandpa.Public,SubstrateNetApi.Model.Types.Primitive.U64>>();
            Value.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}