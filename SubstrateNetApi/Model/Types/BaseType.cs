using System;
using Newtonsoft.Json;

namespace SubstrateNetApi.Model.Types
{
    public interface IType
    {
        string Name();

        int Size();

        byte[] Encode();

        void Decode(byte[] byteArray, ref int p);

        void Create(string str);

        void Create(byte[] byteArray);
    }

    public abstract class BaseType : IType
    {
        public abstract string Name();

        [JsonIgnore]
        public byte[] Bytes { get; internal set; }

        public abstract int Size();

        public abstract byte[] Encode();

        public void Decode(byte[] byteArray, ref int p)
        {
            var memory = byteArray.AsMemory();
            var result = memory.Span.Slice(p, Size()).ToArray();
            p += Size();
            Create(result);
        }

        public virtual void Create(string str) => Create(Utils.HexToByteArray(str));

        public abstract void Create(byte[] byteArray);

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
