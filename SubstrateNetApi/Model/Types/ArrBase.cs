using Newtonsoft.Json;
using SubstrateNetApi.Model.Types.TypeDefPrimitive;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types.TypeDefArray
{
    public abstract class ArrBase : IType
    {
        public abstract string TypeName();
        public abstract int TypeSize();
        [JsonIgnore]
        public byte[] Bytes { get; internal set; }
        public abstract byte[] Encode();
        public abstract void Decode(byte[] byteArray, ref int p);
        public void Create(string str) => Create(Utils.HexToByteArray(str));
        public void Create(byte[] byteArray)
        {
            var p = 0;
            Decode(byteArray, ref p);
        }
        public void CreateFromJson(string str) => Create(Utils.HexToByteArray(str));
        public IType New() => this;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}