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


namespace SubstrateNetApi.Model.Types.Composite
{
    
    
    /// <summary>
    /// >> Path: sp_core.offchain.OpaqueNetworkState
    /// </summary>
    public sealed class OpaqueNetworkState : BaseType
    {
        
        private OpaquePeerId _peerId;
        
        private BaseVec<OpaqueMultiaddr> _externalAddresses;
        
        public OpaquePeerId PeerId
        {
            get
            {
                return this._peerId;
            }
            set
            {
                this._peerId = value;
            }
        }
        
        public BaseVec<OpaqueMultiaddr> ExternalAddresses
        {
            get
            {
                return this._externalAddresses;
            }
            set
            {
                this._externalAddresses = value;
            }
        }
        
        public override string TypeName()
        {
            return "OpaqueNetworkState";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(PeerId.Encode());
            result.AddRange(ExternalAddresses.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            PeerId = new OpaquePeerId();
            PeerId.Decode(byteArray, ref p);
            ExternalAddresses = new BaseVec<OpaqueMultiaddr>();
            ExternalAddresses.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
