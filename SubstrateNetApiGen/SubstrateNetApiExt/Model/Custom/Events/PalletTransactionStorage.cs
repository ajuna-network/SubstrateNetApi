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


namespace SubstrateNetApi.Model.Custom.Events
{
    
    
    /// <summary>
    /// >> Path: pallet_transaction_storage.pallet.Event
    /// 
    ///			The [event](https://substrate.dev/docs/en/knowledgebase/runtime/events) emitted
    ///			by this pallet.
    ///			
    /// </summary>
    public sealed class PalletTransactionStorage
    {
        
        /// <summary>
        /// >> Event: Stored
        /// Stored data under specified index.
        /// </summary>
        public sealed class Stored : BaseTuple<U32>
        {
        }
        
        /// <summary>
        /// >> Event: Renewed
        /// Renewed data under specified index.
        /// </summary>
        public sealed class Renewed : BaseTuple<U32>
        {
        }
        
        /// <summary>
        /// >> Event: ProofChecked
        /// Storage proof was successfully checked.
        /// </summary>
        public sealed class ProofChecked : BaseTuple
        {
        }
    }
}
