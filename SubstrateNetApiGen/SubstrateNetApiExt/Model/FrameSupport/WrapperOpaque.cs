//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.PalletImOnline;
using SubstrateNetApi.Model.Types.Base;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.FrameSupport
{
    
    
    /// <summary>
    /// >> 416 - Composite[frame_support.traits.misc.WrapperOpaque]
    /// </summary>
    public sealed class WrapperOpaque : BaseType
    {
        
        /// <summary>
        /// >> value
        /// </summary>
        private SubstrateNetApi.Model.PalletImOnline.BoundedOpaqueNetworkState _value;
        
        public SubstrateNetApi.Model.PalletImOnline.BoundedOpaqueNetworkState Value
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
            return "WrapperOpaque";
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
            Value = new SubstrateNetApi.Model.PalletImOnline.BoundedOpaqueNetworkState();
            Value.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
