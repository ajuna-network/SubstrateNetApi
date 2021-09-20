//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Types.Base;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.SpFinalityGrandpa
{
    
    
    /// <summary>
    /// >> Path: sp_finality_grandpa.EquivocationProof
    /// </summary>
    public sealed class EquivocationProof : BaseType
    {
        
        private SubstrateNetApi.Model.Types.Primitive.U64 _setId;
        
        private SubstrateNetApi.Model.SpFinalityGrandpa.EnumEquivocation _equivocation;
        
        public SubstrateNetApi.Model.Types.Primitive.U64 SetId
        {
            get
            {
                return this._setId;
            }
            set
            {
                this._setId = value;
            }
        }
        
        public SubstrateNetApi.Model.SpFinalityGrandpa.EnumEquivocation Equivocation
        {
            get
            {
                return this._equivocation;
            }
            set
            {
                this._equivocation = value;
            }
        }
        
        public override string TypeName()
        {
            return "EquivocationProof";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(SetId.Encode());
            result.AddRange(Equivocation.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            SetId = new SubstrateNetApi.Model.Types.Primitive.U64();
            SetId.Decode(byteArray, ref p);
            Equivocation = new SubstrateNetApi.Model.SpFinalityGrandpa.EnumEquivocation();
            Equivocation.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
