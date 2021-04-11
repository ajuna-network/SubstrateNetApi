using System;
using System.Collections.Generic;
using System.Linq;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class Vec<T> : StructType where T : IType, new()
    {
        public override string Name() => $"Vec<{new T().Name()}>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
           List<byte> result = new List<byte>();
            for(int i = 0; i < Value.Count; i++)
            {
                result.AddRange(Value[i].Encode());
            }
            return Utils.SizePrefixedByteArray(result);
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            var list = new List<T>();

            var length = CompactInteger.Decode(byteArray, ref p);
            for (var i = 0; i < length; i++)
            {
                var t = new T();
                t.Decode(byteArray, ref p);
                list.Add(t);
            }

            _size = p - start;

            var bytes = new byte[_size];
            Array.Copy(byteArray, start, bytes, 0, _size);

            Bytes = bytes;
            Value = list;
        }

        public override void CreateFromJson(string str)
        {
            Create(Utils.HexToByteArray(str));
        }

        public List<T> Value { get; internal set; }

        public void Create(List<T> list)
        {
            Value = list;
            Bytes = Encode();
        }

    }
}