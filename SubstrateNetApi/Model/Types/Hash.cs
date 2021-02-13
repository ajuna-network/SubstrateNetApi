using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.Model.Types
{
    public class Hash : BaseType
    {
        public override string Name() => "T::Hash";

        public override int Size() => 32;

        public string Value { get; internal set; }

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = Utils.Bytes2HexString(Bytes, Utils.HexStringFormat.PREFIXED);
        }

    }
}
