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


namespace SubstrateNetApi.Model.PalletProxy
{
    
    
    /// <summary>
    /// >> 470 - Variant[pallet_proxy.pallet.Error]
    /// 
    ///			Custom [dispatch errors](https://substrate.dev/docs/en/knowledgebase/runtime/errors)
    ///			of this pallet.
    ///			
    /// </summary>
    public enum PalletProxyError
    {
        
        /// <summary>
        /// >> TooMany
        /// </summary>
        TooMany,
        
        /// <summary>
        /// >> NotFound
        /// </summary>
        NotFound,
        
        /// <summary>
        /// >> NotProxy
        /// </summary>
        NotProxy,
        
        /// <summary>
        /// >> Unproxyable
        /// </summary>
        Unproxyable,
        
        /// <summary>
        /// >> Duplicate
        /// </summary>
        Duplicate,
        
        /// <summary>
        /// >> NoPermission
        /// </summary>
        NoPermission,
        
        /// <summary>
        /// >> Unannounced
        /// </summary>
        Unannounced,
        
        /// <summary>
        /// >> NoSelfProxy
        /// </summary>
        NoSelfProxy,
    }
}
