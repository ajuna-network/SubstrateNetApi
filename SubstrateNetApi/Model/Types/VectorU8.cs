using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types
{
    /// <summary>
    /// TODO, finish implementing this ...
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Vector<T> : IType where T: IType, new()
    {
        public string Name() => "";

        public int Size()
        {
            throw new NotImplementedException();
        }

        public List<IType> Value { get; internal set; }

        public byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public void Decode(byte[] byteArray, ref int p)
        {
            throw new NotImplementedException();
        }

        public void Create(string str)
        {
            throw new NotImplementedException();
        }

        public void CreateFromJson(string str)
        {
            throw new NotImplementedException();
        }

        public void Create(byte[] byteArray)
        {
            var list = new List<IType>();

            var p = 0;
            var length = CompactInteger.Decode(byteArray, ref p);
            for (var i = 0; i < length; i++)
            {
                var t = new T();
                t.Decode(byteArray, ref p);
                list.Add(t);
            }

            // TODO: persist somewhere
        }
    }
}
