using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Types
{
    public class AccountId : BaseType<string>
    {
        public override string Name() => "T::AccountId";

        /// <summary>
        ///  TODO: might have to change this based on the address type.
        /// </summary>
        /// <returns></returns>
        public override int Size() => 32;

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
                    throw new NotImplementedException("Unknown address version please refere to Constants.cs");
            }
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = Utils.GetAddressFrom(byteArray);
        }
    }
}