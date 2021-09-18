using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types.TypeDefPrimitive
{
    public class PrimVec<T> : IType where T : IType, new()
    {
        public virtual string TypeName() => $"Vec<{new T().TypeName()}>";

        private int _typeSize;

        public int TypeSize() => _typeSize;

        [JsonIgnore]
        public byte[] Bytes { get; internal set; }

        public byte[] Encode()
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < Value.Length; i++)
            {
                result.AddRange(Value[i].Encode());
            }
            return Utils.SizePrefixedByteArray(result);
        }

        public void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            var length = CompactInteger.Decode(byteArray, ref p);

            var array = new T[length];
            for (var i = 0; i < length; i++)
            {
                var t = new T();
                t.Decode(byteArray, ref p);
                array[i] = t;
            }

            _typeSize = p - start;

            Bytes = new byte[_typeSize];
            Array.Copy(byteArray, start, Bytes, 0, _typeSize);
            Value = array;
        }

        public virtual T[] Value { get; internal set; }

        public void Create(T[] list)
        {
            Value = list;
            Bytes = Encode();
        }

        public void Create(string str) => Create(Utils.HexToByteArray(str));

        public void CreateFromJson(string str)=> Create(Utils.HexToByteArray(str));

        public void Create(byte[] byteArray)
        {
            var p = 0;
            Decode(byteArray, ref p);
        }

        public IType New() => this;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}