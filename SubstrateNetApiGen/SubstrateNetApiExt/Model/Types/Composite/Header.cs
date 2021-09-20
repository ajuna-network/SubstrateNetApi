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
    /// >> Path: sp_runtime.generic.header.Header
    /// </summary>
    public sealed class Header : BaseType
    {
        
        private H256 _parentHash;
        
        private BaseCom<U32> _number;
        
        private H256 _stateRoot;
        
        private H256 _extrinsicsRoot;
        
        private Digest _digest;
        
        public H256 ParentHash
        {
            get
            {
                return this._parentHash;
            }
            set
            {
                this._parentHash = value;
            }
        }
        
        public BaseCom<U32> Number
        {
            get
            {
                return this._number;
            }
            set
            {
                this._number = value;
            }
        }
        
        public H256 StateRoot
        {
            get
            {
                return this._stateRoot;
            }
            set
            {
                this._stateRoot = value;
            }
        }
        
        public H256 ExtrinsicsRoot
        {
            get
            {
                return this._extrinsicsRoot;
            }
            set
            {
                this._extrinsicsRoot = value;
            }
        }
        
        public Digest Digest
        {
            get
            {
                return this._digest;
            }
            set
            {
                this._digest = value;
            }
        }
        
        public override string TypeName()
        {
            return "Header";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(ParentHash.Encode());
            result.AddRange(Number.Encode());
            result.AddRange(StateRoot.Encode());
            result.AddRange(ExtrinsicsRoot.Encode());
            result.AddRange(Digest.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            ParentHash = new H256();
            ParentHash.Decode(byteArray, ref p);
            Number = new BaseCom<U32>();
            Number.Decode(byteArray, ref p);
            StateRoot = new H256();
            StateRoot.Decode(byteArray, ref p);
            ExtrinsicsRoot = new H256();
            ExtrinsicsRoot.Decode(byteArray, ref p);
            Digest = new Digest();
            Digest.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
