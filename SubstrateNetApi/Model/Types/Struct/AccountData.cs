using System;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class AccountData : StructBase
    { 
        public override string TypeName() => "AccountData<T::Balance>";
                
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Free = new Balance();
            Free.Decode(byteArray, ref p);

            Reserved = new Balance();
            Reserved.Decode(byteArray, ref p);

            MiscFrozen = new Balance();
            MiscFrozen.Decode(byteArray, ref p);

            FeeFrozen = new Balance();
            FeeFrozen.Decode(byteArray, ref p);

            _typeSize = p - start;
        }

        public Balance Free { get; private set; }
        public Balance Reserved { get; private set; }
        public Balance MiscFrozen { get; private set; }
        public Balance FeeFrozen { get; private set; }
    }
}