using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SubstrateNetApi.Model.Types.Base
{
    public abstract class BaseEnumType : BaseType
    {

    }

    public class BaseEnumExt<T0, T1> : BaseEnumType
                                        where T0 : System.Enum
                                        where T1 : IType, new()
    {
        public override string TypeName() => typeof(T0).Name;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var enumByte = byteArray[p];

            Value = (T0)System.Enum.Parse(typeof(T0), enumByte.ToString(), true);
            p += 1;

            Value2 = DecodeOneOf(enumByte, byteArray, ref p);

            TypeSize = p - start;
        }

        private IType DecodeOneOf(byte value, byte[] byteArray, ref int p)
        {
            IType result;
            switch (value)
            {
                case 0x00:
                    result = new T1();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                default:
                    return null;
            }
        }

        public void Create(T0 t, IType value2)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Convert.ToByte(t));
            bytes.AddRange(value2.Encode());
            Bytes = bytes.ToArray();
            Value = t;
            Value2 = value2;
        }

        public override string ToString() => JsonConvert.SerializeObject(Value);

        [JsonConverter(typeof(StringEnumConverter))]
        public T0 Value { get; set; }

        public IType Value2 { get; set; }

    }

    public class BaseEnumExt<T0, T1, T2> : BaseEnumType
                                            where T0 : System.Enum
                                            where T1 : IType, new()
                                            where T2 : IType, new()
    {
        public override string TypeName() => typeof(T0).Name;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var enumByte = byteArray[p];

            Value = (T0)System.Enum.Parse(typeof(T0), enumByte.ToString(), true);
            p += 1;

            Value2 = DecodeOneOf(enumByte, byteArray, ref p);

            TypeSize = p - start;
        }

        private IType DecodeOneOf(byte value, byte[] byteArray, ref int p)
        {
            IType result;
            switch (value)
            {
                case 0x00:
                    result = new T1();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x01:
                    result = new T2();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                default:
                    return null;
            }
        }

        public void Create(T0 t, IType value2)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Convert.ToByte(t));
            bytes.AddRange(value2.Encode());
            Bytes = bytes.ToArray();
            Value = t;
            Value2 = value2;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public T0 Value { get; set; }

        public IType Value2 { get; set; }

    }

    public class BaseEnumExt<T0, T1, T2, T3> : BaseEnumType
                                                where T0 : System.Enum
                                                where T1 : IType, new()
                                                where T2 : IType, new()
                                                where T3 : IType, new()
    {
        public override string TypeName() => typeof(T0).Name;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var enumByte = byteArray[p];

            Value = (T0)System.Enum.Parse(typeof(T0), enumByte.ToString(), true);
            p += 1;

            Value2 = DecodeOneOf(enumByte, byteArray, ref p);

            TypeSize = p - start;
        }

        private IType DecodeOneOf(byte value, byte[] byteArray, ref int p)
        {
            IType result;
            switch (value)
            {
                case 0x00:
                    result = new T1();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x01:
                    result = new T2();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x02:
                    result = new T3();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                default:
                    return null;
            }
        }

        public void Create(T0 t, IType value2)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Convert.ToByte(t));
            bytes.AddRange(value2.Encode());
            Bytes = bytes.ToArray();
            Value = t;
            Value2 = value2;
        }

        public override string ToString() => JsonConvert.SerializeObject(Value);

        [JsonConverter(typeof(StringEnumConverter))]
        public T0 Value { get; set; }

        public IType Value2 { get; set; }

    }

    public class BaseEnumExt<T0, T1, T2, T3, T4> : BaseEnumType
                                                    where T0 : System.Enum
                                                    where T1 : IType, new()
                                                    where T2 : IType, new()
                                                    where T3 : IType, new()
                                                    where T4 : IType, new()
    {
        public override string TypeName() => typeof(T0).Name;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var enumByte = byteArray[p];

            Value = (T0)System.Enum.Parse(typeof(T0), enumByte.ToString(), true);
            p += 1;

            Value2 = DecodeOneOf(enumByte, byteArray, ref p);

            TypeSize = p - start;
        }

        private IType DecodeOneOf(byte value, byte[] byteArray, ref int p)
        {
            IType result;
            switch (value)
            {
                case 0x00:
                    result = new T1();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x01:
                    result = new T2();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x02:
                    result = new T3();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x03:
                    result = new T4();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                default:
                    return null;
            }
        }

        public void Create(T0 t, IType value2)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Convert.ToByte(t));
            bytes.AddRange(value2.Encode());
            Bytes = bytes.ToArray();
            Value = t;
            Value2 = value2;
        }

        public override string ToString() => JsonConvert.SerializeObject(Value);

        [JsonConverter(typeof(StringEnumConverter))]
        public T0 Value { get; set; }

        public IType Value2 { get; set; }

    }

    public class BaseEnumExt<T0, T1, T2, T3, T4, T5> : BaseEnumType
                                                        where T0 : System.Enum
                                                        where T1 : IType, new()
                                                        where T2 : IType, new()
                                                        where T3 : IType, new()
                                                        where T4 : IType, new()
                                                        where T5 : IType, new()
    {
        public override string TypeName() => typeof(T0).Name;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var enumByte = byteArray[p];

            Value = (T0)System.Enum.Parse(typeof(T0), enumByte.ToString(), true);
            p += 1;

            Value2 = DecodeOneOf(enumByte, byteArray, ref p);

            TypeSize = p - start;
        }

        private IType DecodeOneOf(byte value, byte[] byteArray, ref int p)
        {
            IType result;
            switch (value)
            {
                case 0x00:
                    result = new T1();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x01:
                    result = new T2();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x02:
                    result = new T3();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x03:
                    result = new T4();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x04:
                    result = new T5();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                default:
                    return null;
            }
        }

        public void Create(T0 t, IType value2)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Convert.ToByte(t));
            bytes.AddRange(value2.Encode());
            Bytes = bytes.ToArray();
            Value = t;
            Value2 = value2;
        }

        public override string ToString() => JsonConvert.SerializeObject(Value);

        [JsonConverter(typeof(StringEnumConverter))]
        public T0 Value { get; set; }

        public IType Value2 { get; set; }

    }

    public class BaseEnumExt<T0, T1, T2, T3, T4, T5, T6> : BaseEnumType
                                                            where T0 : System.Enum
                                                            where T1 : IType, new()
                                                            where T2 : IType, new()
                                                            where T3 : IType, new()
                                                            where T4 : IType, new()
                                                            where T5 : IType, new()
                                                            where T6 : IType, new()
    {
        public override string TypeName() => typeof(T0).Name;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var enumByte = byteArray[p];

            Value = (T0)System.Enum.Parse(typeof(T0), enumByte.ToString(), true);
            p += 1;

            Value2 = DecodeOneOf(enumByte, byteArray, ref p);

            TypeSize = p - start;
        }

        private IType DecodeOneOf(byte value, byte[] byteArray, ref int p)
        {
            IType result;
            switch (value)
            {
                case 0x00:
                    result = new T1();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x01:
                    result = new T2();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x02:
                    result = new T3();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x03:
                    result = new T4();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x04:
                    result = new T5();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x05:
                    result = new T6();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                default:
                    return null;
            }
        }

        public void Create(T0 t, IType value2)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Convert.ToByte(t));
            bytes.AddRange(value2.Encode());
            Bytes = bytes.ToArray();
            Value = t;
            Value2 = value2;
        }

        public override string ToString() => JsonConvert.SerializeObject(Value);

        [JsonConverter(typeof(StringEnumConverter))]
        public T0 Value { get; set; }

        public IType Value2 { get; set; }

    }

    public class BaseEnumExt<T0, T1, T2, T3, T4, T5, T6, T7> : BaseEnumType
                                                            where T0 : System.Enum
                                                            where T1 : IType, new()
                                                            where T2 : IType, new()
                                                            where T3 : IType, new()
                                                            where T4 : IType, new()
                                                            where T5 : IType, new()
                                                            where T6 : IType, new()
                                                            where T7 : IType, new()
    {
        public override string TypeName() => typeof(T0).Name;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var enumByte = byteArray[p];

            Value = (T0)System.Enum.Parse(typeof(T0), enumByte.ToString(), true);
            p += 1;

            Value2 = DecodeOneOf(enumByte, byteArray, ref p);

            TypeSize = p - start;
        }

        private IType DecodeOneOf(byte value, byte[] byteArray, ref int p)
        {
            IType result;
            switch (value)
            {
                case 0x00:
                    result = new T1();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x01:
                    result = new T2();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x02:
                    result = new T3();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x03:
                    result = new T4();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x04:
                    result = new T5();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x05:
                    result = new T6();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x06:
                    result = new T7();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                default:
                    return null;
            }
        }

        public void Create(T0 t, IType value2)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Convert.ToByte(t));
            bytes.AddRange(value2.Encode());
            Bytes = bytes.ToArray();
            Value = t;
            Value2 = value2;
        }

        public override string ToString() => JsonConvert.SerializeObject(Value);

        [JsonConverter(typeof(StringEnumConverter))]
        public T0 Value { get; set; }

        public IType Value2 { get; set; }

    }

    public class BaseEnumExt<T0, T1, T2, T3, T4, T5, T6, T7, T8> : BaseEnumType
                                                                where T0 : System.Enum
                                                                where T1 : IType, new()
                                                                where T2 : IType, new()
                                                                where T3 : IType, new()
                                                                where T4 : IType, new()
                                                                where T5 : IType, new()
                                                                where T6 : IType, new()
                                                                where T7 : IType, new()
                                                                where T8 : IType, new()
    {
        public override string TypeName() => typeof(T0).Name;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var enumByte = byteArray[p];

            Value = (T0)System.Enum.Parse(typeof(T0), enumByte.ToString(), true);
            p += 1;

            Value2 = DecodeOneOf(enumByte, byteArray, ref p);

            TypeSize = p - start;
        }

        private IType DecodeOneOf(byte value, byte[] byteArray, ref int p)
        {
            IType result;
            switch (value)
            {
                case 0x00:
                    result = new T1();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x01:
                    result = new T2();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x02:
                    result = new T3();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x03:
                    result = new T4();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x04:
                    result = new T5();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x05:
                    result = new T6();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x06:
                    result = new T7();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x07:
                    result = new T8();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                default:
                    return null;
            }
        }

        public void Create(T0 t, IType value2)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Convert.ToByte(t));
            bytes.AddRange(value2.Encode());
            Bytes = bytes.ToArray();
            Value = t;
            Value2 = value2;
        }

        public override string ToString() => JsonConvert.SerializeObject(Value);

        [JsonConverter(typeof(StringEnumConverter))]
        public T0 Value { get; set; }

        public IType Value2 { get; set; }

    }

    public class BaseEnumExt<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : BaseEnumType
        where T0 : System.Enum
        where T1 : IType, new()
        where T2 : IType, new()
        where T3 : IType, new()
        where T4 : IType, new()
        where T5 : IType, new()
        where T6 : IType, new()
        where T7 : IType, new()
        where T8 : IType, new()
        where T9 : IType, new()
    {
        public override string TypeName() => typeof(T0).Name;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var enumByte = byteArray[p];

            Value = (T0)System.Enum.Parse(typeof(T0), enumByte.ToString(), true);
            p += 1;

            Value2 = DecodeOneOf(enumByte, byteArray, ref p);

            TypeSize = p - start;
        }

        private IType DecodeOneOf(byte value, byte[] byteArray, ref int p)
        {
            IType result;
            switch (value)
            {
                case 0x00:
                    result = new T1();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x01:
                    result = new T2();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x02:
                    result = new T3();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x03:
                    result = new T4();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x04:
                    result = new T5();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x05:
                    result = new T6();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x06:
                    result = new T7();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x07:
                    result = new T8();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x08:
                    result = new T9();
                    if (result.GetType().Name == "Void")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                default:
                    return null;
            }
        }

        public void Create(T0 t, IType value2)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Convert.ToByte(t));
            bytes.AddRange(value2.Encode());
            Bytes = bytes.ToArray();
            Value = t;
            Value2 = value2;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public T0 Value { get; set; }

        public IType Value2 { get; set; }

    }
}