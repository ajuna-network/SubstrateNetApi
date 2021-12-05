using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types.Base
{
    public class AccountId : BasePrim<string>
    {
        // TODO: <T::Lookup as StaticLookup>::Source -- RawAccountId is unprefixed Address
        public override string TypeName() => "T::AccountId";

        // TODO: might have to change this based on the address type.
        public override int TypeSize => 32;

        public override byte[] Encode()
        {
            var bytes = new List<byte>();
            switch (Constants.AddressVersion)
            {
                case 0:
                    return Bytes;
                case 1:
                    bytes.Add(0xFF);
                    bytes.AddRange(Bytes);
                    return bytes.ToArray();
                case 2:
                    bytes.Add(0x00);
                    bytes.AddRange(Bytes);
                    return bytes.ToArray();
                default:
                    throw new NotImplementedException("Unknown address version please refer to Constants.cs");
            }
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = Utils.GetAddressFrom(byteArray);
        }
    }
}