//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.SpRuntime;
using SubstrateNetApi.Model.Types.Base;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.PalletAuthorship
{
    
    
    /// <summary>
    /// >> 133 - Variant[pallet_authorship.pallet.Call]
    /// Contains one variant per dispatchable that can be called by an extrinsic.
    /// </summary>
    public sealed class PalletAuthorshipCall
    {
        
        /// <summary>
        /// >> set_uncles
        /// </summary>
        public GenericExtrinsicCall SetUncles(BaseVec<SubstrateNetApi.Model.SpRuntime.Header> new_uncles)
        {
            return new GenericExtrinsicCall("Authorship", "set_uncles", new_uncles);
        }
    }
}
