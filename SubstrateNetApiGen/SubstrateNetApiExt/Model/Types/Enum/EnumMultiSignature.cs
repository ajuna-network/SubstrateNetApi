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
    
    
    public enum MultiSignature
    {
        
        Ed25519,
        
        Sr25519,
        
        Ecdsa,
    }
    
    /// <summary>
    /// >> Enum
    /// </summary>
    public sealed class EnumMultiSignature : BaseEnumExt<MultiSignature, Signature, Signature, Signature>
    {
    }
}