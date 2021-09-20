using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Base
{
    public class BaseVoid : BaseType
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