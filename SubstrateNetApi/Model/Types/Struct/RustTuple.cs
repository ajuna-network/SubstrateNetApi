using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class RustTuple<T1, T2> : StructBase where T1 : IType, new()
                                                where T2 : IType, new()
    {
        public override string TypeName() => $"({new T1().TypeName()},{new T2().TypeName()})";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Value = new IType[2];

            var t1 = new T1();
            t1.Decode(byteArray, ref p);
            Value[0] = t1;

            var t2 = new T2();
            t2.Decode(byteArray, ref p);
            Value[1] = t2;

            _typeSize = p - start;

            Bytes = new byte[_typeSize];
            Array.Copy(byteArray, start, Bytes, 0, _typeSize);
        }

        public IType[] Value { get; internal set; }
    }
}