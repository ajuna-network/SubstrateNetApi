using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SubstrateNetApi.Model.Types
{
    public class EnumType<T> : IType where T : System.Enum
    {
        public string Name() => typeof(T).Name;

        public int Size() => 1;

        [JsonIgnore] 
        public byte[] Bytes { get; internal set; }

        public byte[] Encode()
        {
            return Bytes;
        }

        public void Decode(byte[] byteArray, ref int p)
        {
            var memory = byteArray.AsMemory();
            var result = memory.Span.Slice(p, Size()).ToArray();
            p += Size();
            Create(result);
        }

        public virtual void Create(string str) => Create(Utils.HexToByteArray(str));

        public virtual void CreateFromJson(string str) => Create(Utils.HexToByteArray(str));

        public void Create(T t)
        {
            Bytes = BitConverter.GetBytes(Convert.ToInt32(t));
            Value = t;
        }

        public void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = (T) System.Enum.Parse(typeof(T), byteArray[0].ToString(), true);

            //if (byteArray.Length < Size())
            //{
            //    var newByteArray = new byte[Size()];
            //    byteArray.CopyTo(newByteArray, 0);
            //    byteArray = newByteArray;
            //}

            //Bytes = byteArray;
            //Value = (T)System.Enum.Parse(typeof(T), BitConverter.ToUInt32(byteArray, 0).ToString(), true);
        }

        public IType New() => this;

        public override string ToString() => JsonConvert.SerializeObject(Value);

        [JsonConverter(typeof(StringEnumConverter))]
        public T Value { get; internal set; }

    }
}