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


namespace SubstrateNetApi.Model.Custom.Errors
{
    
    
    /// <summary>
    /// >> Path: pallet_indices.pallet.Error
    /// 
    ///			Custom [dispatch errors](https://substrate.dev/docs/en/knowledgebase/runtime/errors)
    ///			of this pallet.
    ///			
    /// </summary>
    public enum PalletIndices
    {
        
        /// <summary>
        /// >> Event: NotAssigned
        /// The index was not already assigned.
        /// </summary>
        NotAssigned,
        
        /// <summary>
        /// >> Event: NotOwner
        /// The index is assigned to another account.
        /// </summary>
        NotOwner,
        
        /// <summary>
        /// >> Event: InUse
        /// The index was not available.
        /// </summary>
        InUse,
        
        /// <summary>
        /// >> Event: NotTransfer
        /// The source and destination accounts are identical.
        /// </summary>
        NotTransfer,
        
        /// <summary>
        /// >> Event: Permanent
        /// The index is permanent and may not be freed/changed.
        /// </summary>
        Permanent,
    }
}
