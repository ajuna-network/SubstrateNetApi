using System;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class MogwaiBios : StructType
    {
        public override string Name() => "MogwaiBios<T::Hash, T::BlockNumber, BalanceOf<T>>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Id = new Hash();
            Id.Decode(byteArray, ref p);

            State = new U32();
            State.Decode(byteArray, ref p);

            MetaXy = new Vec<U8Arr16>();
            MetaXy.Decode(byteArray, ref p);

            Intrinsic = new Balance();
            Intrinsic.Decode(byteArray, ref p);

            Level = new U8();
            Level.Decode(byteArray, ref p);

            Phases = new Vec<BlockNumber>();
            Phases.Decode(byteArray, ref p);

            Adaptations = new Vec<Hash>();
            Adaptations.Decode(byteArray, ref p);

            _size = p - start;
        }

        public Hash Id { get; private set; }
        public U32 State { get; private set; }
        public Vec<U8Arr16> MetaXy { get; private set; }
        public Balance Intrinsic { get; private set; }
        public U8 Level { get; private set; }
        public Vec<BlockNumber> Phases { get; private set; }
        public Vec<Hash> Adaptations { get; private set; }
    }
}