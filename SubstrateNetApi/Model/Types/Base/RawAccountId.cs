using System;

namespace SubstrateNetApi.Model.Types.Base
{
    public class RawAccountId : BaseType<string>
    {
        public override string Name() => "Raw";

        public override int Size() => 32;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = Utils.GetAddressFrom(byteArray);
        }
    }
}