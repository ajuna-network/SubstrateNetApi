namespace SubstrateNetApi.Model.Types.Base
{
    public class Hash : BaseType<string>
    {
        public override string Name()
        {
            return "T::Hash";
        }

        public override int Size()
        {
            return 32;
        }

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