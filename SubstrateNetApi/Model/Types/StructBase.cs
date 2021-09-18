using Newtonsoft.Json;

namespace SubstrateNetApi.Model.Types
{
    public abstract class StructBase : IType
    {
        public abstract string TypeName();

        internal int _typeSize;
        public int TypeSize() => _typeSize;

        [JsonIgnore] 
        public byte[] Bytes { get; internal set; }

        public abstract byte[] Encode();

        public abstract void Decode(byte[] byteArray, ref int p);

        public virtual void Create(string str) => Create(Utils.HexToByteArray(str));

        public virtual void CreateFromJson(string str) => Create(Utils.HexToByteArray(str));

        public void Create(byte[] byteArray)
        {
            var p = 0;
            Decode(byteArray, ref p);
        }


        public IType New() => this;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}