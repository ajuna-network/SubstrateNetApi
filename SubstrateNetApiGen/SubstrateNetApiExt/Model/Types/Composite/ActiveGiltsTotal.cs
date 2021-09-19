//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    /// >> Path: pallet_gilt.pallet.ActiveGiltsTotal
    /// </summary>
    public sealed class ActiveGiltsTotal : BaseType
    {
        
        private U128 _frozen;
        
        private Perquintill _proportion;
        
        private U32 _index;
        
        private Perquintill _target;
        
        public U128 Frozen
        {
            get
            {
                return this._frozen;
            }
            set
            {
                this._frozen = value;
            }
        }
        
        public Perquintill Proportion
        {
            get
            {
                return this._proportion;
            }
            set
            {
                this._proportion = value;
            }
        }
        
        public U32 Index
        {
            get
            {
                return this._index;
            }
            set
            {
                this._index = value;
            }
        }
        
        public Perquintill Target
        {
            get
            {
                return this._target;
            }
            set
            {
                this._target = value;
            }
        }
        
        public override string TypeName()
        {
            return "ActiveGiltsTotal";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Frozen.Encode());
            result.AddRange(Proportion.Encode());
            result.AddRange(Index.Encode());
            result.AddRange(Target.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Frozen = new U128();
            Frozen.Decode(byteArray, ref p);
            Proportion = new Perquintill();
            Proportion.Decode(byteArray, ref p);
            Index = new U32();
            Index.Decode(byteArray, ref p);
            Target = new Perquintill();
            Target.Decode(byteArray, ref p);
            _typeSize = p - start;
        }
    }
}
