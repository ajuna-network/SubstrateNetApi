using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types.TypeDefPrimitive
{
    public class PrimTuple : StructBase
    {
        public override string TypeName()
        {
            return "(" + ")";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value)
            {
                result.AddRange(v.Encode());
            }
            return result.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            _typeSize = p - start;

            Bytes = new byte[_typeSize];
            Array.Copy(byteArray, start, Bytes, 0, _typeSize);
        }

        public IType[] Value { get; internal set; }
    }

    public class PrimTuple<T1, T2> : StructBase 
                                                where T1 : IType, new()
                                                where T2 : IType, new()
    {
        public override string TypeName()
        {
            return "(" +
                new T1().TypeName() + "," +
                new T2().TypeName() + ")";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value) {
                result.AddRange(v.Encode());
            }
            return result.ToArray();
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

    public class PrimTuple<T1, T2, T3> : StructBase 
                                            where T1 : IType, new()
                                            where T2 : IType, new()
                                            where T3 : IType, new()
    {
        public override string TypeName()
        {
            return "(" +
                new T1().TypeName() + "," +
                new T2().TypeName() + "," +
                new T3().TypeName() + ")";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value)
            {
                result.AddRange(v.Encode());
            }
            return result.ToArray();
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

            var t3 = new T3();
            t3.Decode(byteArray, ref p);
            Value[2] = t3;

            _typeSize = p - start;

            Bytes = new byte[_typeSize];
            Array.Copy(byteArray, start, Bytes, 0, _typeSize);
        }

        public IType[] Value { get; internal set; }
    }

    public class PrimTuple<T1, T2, T3, T4> : StructBase
                                            where T1 : IType, new()
                                            where T2 : IType, new()
                                            where T3 : IType, new()
                                            where T4 : IType, new()
    {
        public override string TypeName()
        {
            return "(" +
                new T1().TypeName() + "," +
                new T2().TypeName() + "," +
                new T3().TypeName() + "," +
                new T4().TypeName() + ")";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value)
            {
                result.AddRange(v.Encode());
            }
            return result.ToArray();
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

            var t3 = new T3();
            t3.Decode(byteArray, ref p);
            Value[2] = t3;

            var t4 = new T4();
            t4.Decode(byteArray, ref p);
            Value[3] = t4;

            _typeSize = p - start;

            Bytes = new byte[_typeSize];
            Array.Copy(byteArray, start, Bytes, 0, _typeSize);
        }

        public IType[] Value { get; internal set; }
    }

    public class PrimTuple<T1, T2, T3, T4, T5> : StructBase
                                        where T1 : IType, new()
                                        where T2 : IType, new()
                                        where T3 : IType, new()
                                        where T4 : IType, new()
                                        where T5 : IType, new()
    {
        public override string TypeName()
        {
            return "(" +
                new T1().TypeName() + "," +
                new T2().TypeName() + "," +
                new T3().TypeName() + "," +
                new T4().TypeName() + "," +
                new T5().TypeName() + ")";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value)
            {
                result.AddRange(v.Encode());
            }
            return result.ToArray();
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

            var t3 = new T3();
            t3.Decode(byteArray, ref p);
            Value[2] = t3;

            var t4 = new T4();
            t4.Decode(byteArray, ref p);
            Value[3] = t4;

            var t5 = new T5();
            t5.Decode(byteArray, ref p);
            Value[4] = t5;

            _typeSize = p - start;

            Bytes = new byte[_typeSize];
            Array.Copy(byteArray, start, Bytes, 0, _typeSize);
        }

        public IType[] Value { get; internal set; }
    }

    public class PrimTuple<T1, T2, T3, T4, T5, T6> : StructBase
                                    where T1 : IType, new()
                                    where T2 : IType, new()
                                    where T3 : IType, new()
                                    where T4 : IType, new()
                                    where T5 : IType, new()
                                    where T6 : IType, new()
    {
        public override string TypeName()
        {
            return "(" +
                new T1().TypeName() + "," +
                new T2().TypeName() + "," +
                new T3().TypeName() + "," +
                new T4().TypeName() + "," +
                new T5().TypeName() + "," +
                new T6().TypeName() + ")";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value)
            {
                result.AddRange(v.Encode());
            }
            return result.ToArray();
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

            var t3 = new T3();
            t3.Decode(byteArray, ref p);
            Value[2] = t3;

            var t4 = new T4();
            t4.Decode(byteArray, ref p);
            Value[3] = t4;

            var t5 = new T5();
            t5.Decode(byteArray, ref p);
            Value[4] = t5;

            var t6 = new T6();
            t6.Decode(byteArray, ref p);
            Value[5] = t6;

            _typeSize = p - start;

            Bytes = new byte[_typeSize];
            Array.Copy(byteArray, start, Bytes, 0, _typeSize);
        }

        public IType[] Value { get; internal set; }
    }

    public class PrimTuple<T1, T2, T3, T4, T5, T6, T7> : StructBase
                                where T1 : IType, new()
                                where T2 : IType, new()
                                where T3 : IType, new()
                                where T4 : IType, new()
                                where T5 : IType, new()
                                where T6 : IType, new()
                                where T7 : IType, new()
    {
        public override string TypeName()
        {
            return "(" +
                new T1().TypeName() + "," +
                new T2().TypeName() + "," +
                new T3().TypeName() + "," +
                new T4().TypeName() + "," +
                new T5().TypeName() + "," +
                new T6().TypeName() + "," +
                new T7().TypeName() + ")";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value)
            {
                result.AddRange(v.Encode());
            }
            return result.ToArray();
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

            var t3 = new T3();
            t3.Decode(byteArray, ref p);
            Value[2] = t3;

            var t4 = new T4();
            t4.Decode(byteArray, ref p);
            Value[3] = t4;

            var t5 = new T5();
            t5.Decode(byteArray, ref p);
            Value[4] = t5;

            var t6 = new T6();
            t6.Decode(byteArray, ref p);
            Value[5] = t6;

            var t7 = new T7();
            t7.Decode(byteArray, ref p);
            Value[6] = t7;

            _typeSize = p - start;

            Bytes = new byte[_typeSize];
            Array.Copy(byteArray, start, Bytes, 0, _typeSize);
        }

        public IType[] Value { get; internal set; }
    }
}