//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.SpCore
{
    
    
    /// <summary>
    /// >> 238 - Composite[sp_core.offchain.OpaqueMultiaddr]
    /// </summary>
    public sealed class OpaqueMultiaddr : BaseType
    {
        
        /// <summary>
        /// >> value
        /// </summary>
        private BaseVec<SubstrateNetApi.Model.Types.Primitive.U8> _value;
        
        public BaseVec<SubstrateNetApi.Model.Types.Primitive.U8> Value
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
            return "OpaqueMultiaddr";
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
            Value = new BaseVec<SubstrateNetApi.Model.Types.Primitive.U8>();
            Value.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
