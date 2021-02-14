using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using MessagePack.Formatters;

namespace SubstrateNetApi.Model.Types
{
    public class Vec<T> : StructType where T: IType, new()
    {
        private int _size = 0;

        public override string Name() => $"Vec<{new T().Name()}>";

        public override int Size()
        {
            return _size;
        }

        public List<T> Value { get; internal set; }

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
    }
}
