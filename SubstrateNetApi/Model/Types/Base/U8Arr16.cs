namespace SubstrateNetApi.Model.Types.Base
{
    public class U8Arr16 : BaseType<string>
    {
        public override string Name() => "[u8;16]";

        public override int Size() => 16;

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