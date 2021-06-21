using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SubstrateNetApi.Model.Types
{
    public class NullType : IType
    {
        public string Name() => "NullType";
        public int Size() => 0;

        public void Create(string str)
        {
            throw new NotImplementedException();
        }

        public void Create(byte[] byteArray)
        {
            throw new NotImplementedException();
        }

        public void CreateFromJson(string str)
        {
            throw new NotImplementedException();
        }

        public void Decode(byte[] byteArray, ref int p)
        {
            throw new NotImplementedException();
        }

        public byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public IType New()
        {
            throw new NotImplementedException();
        }
    }

    public class ExtEnumType<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> : IType 
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
        public virtual string Name() => typeof(T0).Name;

        private int _size;
        public int Size() => _size;

        [JsonIgnore] 
        public byte[] Bytes { get; internal set; }

        public byte[] Encode()
        {
            return Bytes;
        }

        public void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Value = (T0)System.Enum.Parse(typeof(T0), byteArray[0].ToString(), true);
            p += 1;

            Value2 = DecodeOneOf(byteArray[0], byteArray, ref p);

            _size = p - start;
        }

        private IType DecodeOneOf(byte value, byte[] byteArray, ref int p)
        {
            IType result;
            switch (value)
            {
                case 0x00:
                    result = new T1();
                    if (result.GetType().Name == "NullType")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x01:
                    result = new T2();
                    if (result.GetType().Name == "NullType")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x02:
                    result = new T3();
                    if (result.GetType().Name == "NullType")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x03:
                    result = new T4();
                    if (result.GetType().Name == "NullType")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x04:
                    result = new T5();
                    if (result.GetType().Name == "NullType")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x05:
                    result = new T6();
                    if (result.GetType().Name == "NullType")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x06:
                    result = new T7();
                    if (result.GetType().Name == "NullType")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x07:
                    result = new T8();
                    if (result.GetType().Name == "NullType")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                case 0x08:
                    result = new T9();
                    if (result.GetType().Name == "NullType")
                        return null;
                    result.Decode(byteArray, ref p);
                    return result;
                default:
                    return null;
            }
        }

        public virtual void Create(string str) => Create(Utils.HexToByteArray(str));

        public virtual void CreateFromJson(string str) => Create(Utils.HexToByteArray(str));

        public void Create(T0 t, IType value2)
        {
            Bytes = BitConverter.GetBytes(Convert.ToInt32(t));
            Value = t;
            Value2 = value2;
        }

        public void Create(byte[] byteArray)
        {
            int p = 0;
            Decode(byteArray, ref p);
        }

        public IType New() => this;

        public override string ToString() => JsonConvert.SerializeObject(Value);

        [JsonConverter(typeof(StringEnumConverter))]
        public T0 Value { get; internal set; }

        public IType Value2 { get; internal set; }

    }
}