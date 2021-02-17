using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class Vec<T> : StructType where T : IType, new()
    {
        public override string Name() => $"Vec<{new T().Name()}>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
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

            Bytes = byteArray;
            Value = list;

            _size = p - start;
        }

        public List<T> Value { get; internal set; }
    }
}