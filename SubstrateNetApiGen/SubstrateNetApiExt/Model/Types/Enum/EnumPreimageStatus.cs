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


namespace SubstrateNetApi.Model.Types.Enum
{
    
    
    public enum PreimageStatus
    {
        
        Missing,
        
        Available,
    }
    
    /// <summary>
    /// >> Enum
    /// </summary>
    public sealed class EnumPreimageStatus : BaseEnumExt<PreimageStatus, U32, BaseTuple<BaseVec<U8>, AccountId32, U128, U32, BaseOpt<U32>>>
    {
    }
}
