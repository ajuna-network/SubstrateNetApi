using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.Model.Types.TypeDefPrimitive
{
    public class PrimCom<T> : IType where T : IType, new()
    {
        public virtual string TypeName() => $"PrimCom<{new T().TypeName()}>";

        private int _size;

        public int TypeSize() => _size;

        [JsonIgnore]
        public byte[] Bytes { get; internal set; }

        public byte[] Encode()
        {
            return Value.Encode();
        }

        public void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Value = CompactInteger.Decode(byteArray, ref p);

            _size = p - start;

        }

        public virtual CompactInteger Value { get; internal set; }

        public void Create(CompactInteger compactInteger)
        {
            Value = compactInteger;
            Bytes = Encode();
        }

        public void Create(string str) => Create(Utils.HexToByteArray(str));

        public void CreateFromJson(string str) => Create(Utils.HexToByteArray(str));

        public void Create(byte[] byteArray)
        {
            var p = 0;
            Decode(byteArray, ref p);
        }

        public IType New() => this;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}