using System;
using Newtonsoft.Json;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types
{
    public abstract class BasePrim<T> : BaseType
    {
        public override void Decode(byte[] byteArray, ref int p)
        {
            var memory = byteArray.AsMemory();
            var result = memory.Span.Slice(p, TypeSize).ToArray();
            p += TypeSize;
            Create(result);
        }

        public T Value { get; set; }

    }
}