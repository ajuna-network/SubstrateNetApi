using SubstrateNetApi.Model.Types.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class BaseString : StructBase
    {
        public override string TypeName() => $"String";

        public override byte[] Encode()
        {
            var result = Encoding.Default.GetBytes(Value);
            return Utils.SizePrefixedByteArray(result.ToList());
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            var value = String.Empty;

            var length = CompactInteger.Decode(byteArray, ref p);
            for (var i = 0; i < length; i++)
            {
                var t = new BaseChar();
                t.Decode(byteArray, ref p);
                value += t.Value;
            }

            _typeSize = p - start;

            var bytes = new byte[_typeSize];
            Array.Copy(byteArray, start, bytes, 0, _typeSize);

            Bytes = bytes;
            Value = value;
        }

        public override void CreateFromJson(string str)
        {
            Create(Utils.HexToByteArray(str));
        }

        public string Value { get; internal set; }

        public override void Create(string value)
        {
            Value = value;
            Bytes = Encode();
        }

    }
}