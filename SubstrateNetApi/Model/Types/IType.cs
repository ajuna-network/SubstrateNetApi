using System;
using Newtonsoft.Json;

namespace SubstrateNetApi.Model.Types
{
    /// <summary>
    /// Reference to the polkadot js types implementation
    /// https://github.com/polkadot-js/api/tree/master/packages/types/src
    /// </summary>
    public interface IType
    {
        string Name();

        int Size();

        byte[] Encode();

        void Decode(byte[] byteArray, ref int p);

        void Create(string str);

        void CreateFromJson(string str);

        void Create(byte[] byteArray);
    }

    public abstract class StructType : IType
    {
        public abstract string Name();

        [JsonIgnore]
        public byte[] Bytes { get; internal set; }

        public abstract int Size();

        public abstract byte[] Encode();

        public abstract void Decode(byte[] byteArray, ref int p);


        public virtual void Create(string str) => Create(Utils.HexToByteArray(str));

        public virtual void CreateFromJson(string str) => Create(Utils.HexToByteArray(str));

        public void Create(byte[] byteArray)
        {
            var p = 0;
            Decode(byteArray, ref p);
        }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}