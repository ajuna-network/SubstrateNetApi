namespace SubstrateNetApi.Model.Types
{
    public class Void : BaseType
    {
        public override string TypeName() => "Void";

        public override int TypeSize => 0;

        public override void Decode(byte[] byteArray, ref int p)
        {
            Bytes = new byte[] { };
        }

        public override byte[] Encode()
        {
            return new byte[] { };
        }
    }
}