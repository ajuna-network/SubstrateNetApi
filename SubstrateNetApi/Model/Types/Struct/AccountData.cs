using System;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class AccountData : StructType
    {
        private int _size;

        public Balance Free { get; private set; }
        public Balance Reserved { get; private set; }
        public Balance MiscFrozen { get; private set; }
        public Balance FeeFrozen { get; private set; }

        public override string Name()
        {
            return "AccountData<T::Balance>";
        }

        public override int Size()
        {
            return _size;
        }

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

            _size = p - start;
        }
    }
}