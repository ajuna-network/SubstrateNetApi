//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.SpConsensusBabe;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.SpConsensusBabe
{
    
    
    public enum NextConfigDescriptor
    {
        
        V1,
    }
    
    /// <summary>
    /// >> 128 - Variant[sp_consensus_babe.digests.NextConfigDescriptor]
    /// </summary>
    public sealed class EnumNextConfigDescriptor : BaseEnumExt<NextConfigDescriptor, BaseTuple<BaseTuple<SubstrateNetApi.Model.Types.Primitive.U64,SubstrateNetApi.Model.Types.Primitive.U64>, SubstrateNetApi.Model.SpConsensusBabe.EnumAllowedSlots>>
    {
    }
}