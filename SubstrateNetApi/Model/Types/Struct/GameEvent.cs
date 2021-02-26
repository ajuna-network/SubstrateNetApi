using System;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class GameEvent : StructType
    {
        public override string Name() => "GameEvent<T::Hash, T::BlockNumber, GameEventType>";

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

            Begin = new BlockNumber();
            Begin.Decode(byteArray, ref p);

            Duration = new U16();
            Duration.Decode(byteArray, ref p);

            EventType = new EnumType<GameEventType>();
            EventType.Decode(byteArray, ref p);

            Hashes = new Vec<Hash>();
            Hashes.Decode(byteArray, ref p);

            Value = new U64();
            Value.Decode(byteArray, ref p);

            _size = p - start;
        }

        public Hash Id { get; private set; }
        public BlockNumber Begin { get; private set; }
        public U16 Duration { get; private set; }
        public EnumType<GameEventType> EventType { get; private set; }
        public Vec<Hash> Hashes { get; private set; }
        public U64 Value { get; private set; }
    }
}