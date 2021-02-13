using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types
{
    /// <summary>
    /// TODO, finish implementing this ...
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Vector<T> : BaseType where T: BaseType, new()
    {
        public override string Name() => "";

        public override int Size()
        {
            throw new NotImplementedException();
        }

        public List<BaseType> Value { get; internal set; }

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Create(byte[] byteArray)
        {
            var list = new List<BaseType>();

            var p = 0;
            var length = CompactInteger.Decode(byteArray, ref p);
            for (var i = 0; i < length; i++)
            {
                var t = new T();
                t.Decode(byteArray, ref p);
                list.Add(t);
            }

            Bytes = byteArray;
            Value = list;
        }
    }
}
