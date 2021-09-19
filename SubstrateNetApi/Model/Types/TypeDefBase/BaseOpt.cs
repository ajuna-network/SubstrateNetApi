using Newtonsoft.Json;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubstrateNetApi.Model.Types.TypeDefBase
{
    public class BaseOpt<T> : IType where T : IType, new()
    {
        public virtual string TypeName() => $"Option<{new T().TypeName()}>";

        private int _typeSize;

        public int TypeSize() => _typeSize;

        [JsonIgnore]
        public byte[] Bytes { get; internal set; }

        public byte[] Encode()
        {
           var bytes = new List<byte>();
           if (OptionFlag)
            {
                bytes.Add(1);
                bytes.AddRange(Value.Encode());

            } else
            {
                bytes.Add(0);
            }

            return bytes.ToArray();
        }

        public void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            var optionByte = new PrimU8();
            optionByte.Decode(byteArray, ref p);

            OptionFlag = optionByte.Value > 0;

            T t = default;
            if (optionByte.Value > 0)
            {
                t = new T();
                t.Decode(byteArray, ref p);
            }

            _typeSize = p - start;

            var bytes = new byte[_typeSize];
            Array.Copy(byteArray, start, bytes, 0, _typeSize);

            Bytes = bytes;
            Value = t != null ? t : default;
        }

        public bool OptionFlag { get; set; }

        public T Value { get; internal set; }

        public void Create(T value)
        {
            OptionFlag = value != null;
            Value = value;
            Bytes = Encode();
        }

        public void Create(string str) => Create(Utils.HexToByteArray(str));

        public void CreateFromJson(string str) => Create(Utils.HexToByteArray(str));

        public void Create(byte[] byteArray)
        {
            var p = 0;
            Decode(byteArray, ref p);
        }

        public IType New() => this;

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}