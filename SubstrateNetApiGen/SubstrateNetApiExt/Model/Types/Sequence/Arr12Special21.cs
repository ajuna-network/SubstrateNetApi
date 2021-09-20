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


namespace SubstrateNetApi.Model.Types.Sequence
{
    
    
    /// <summary>
    /// >> Array
    /// </summary>
    public sealed class Arr12Special21 : BaseType
    {
        
        private BaseTuple<BaseCom<U16>,BaseCom<PerU16>>[] _value;
        
        public override int TypeSize
        {
            get
            {
                return 12;
            }
        }
        
        public BaseTuple<BaseCom<U16>,BaseCom<PerU16>>[] Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
        
        public override string TypeName()
        {
            return string.Format("[{0}; {1}]", new BaseTuple<BaseCom<U16>,BaseCom<PerU16>>().TypeName(), this.TypeSize);
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value){result.AddRange(v.Encode());};
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var array = new BaseTuple<BaseCom<U16>,BaseCom<PerU16>>[TypeSize];
            for (var i = 0; i < array.Length; i++) {var t = new BaseTuple<BaseCom<U16>,BaseCom<PerU16>>();t.Decode(byteArray, ref p);array[i] = t;};
            var bytesLength = p - start;
            Bytes = new byte[bytesLength];
            Array.Copy(byteArray, start, Bytes, 0, bytesLength);
            Value = array;
        }
        
        public void Create(BaseTuple<BaseCom<U16>,BaseCom<PerU16>>[] array)
        {
            Value = array;
            Bytes = Encode();
        }
    }
}
