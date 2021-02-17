namespace SubstrateNetApi.Model.Types.Base
{
    public class Hash : BaseType<string>
    {
        public override string Name() => "T::Hash";

        public override int Size() => 32;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = Utils.Bytes2HexString(Bytes);
        }
    }
}