using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Struct;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SubstrateNetApi.Model.Types.Custom
{
    #region BASE_TYPES

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

    public class RawBalance : BaseType<BigInteger>
    {
        public override string Name() => "Raw";

        public override int Size() => 16;

        public override byte[] Encode()
        {
            return Bytes;
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = new BigInteger(byteArray);
        }

        public void Create(BigInteger value)
        {
            var byteArray = new byte[16];
            Array.Copy(value.ToByteArray(), 0, byteArray, 0, value.ToByteArray().Length);
            Bytes = byteArray;
            Value = value;
        }
    }

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

    #endregion

    #region ENUM_TYPES

    public enum GameEventType
    {
        Default,
        Hatch
    }

    public enum RarityType
    {
        Minor,
        Normal,
        Rare,
        Epic,
        Legendary
    }

    public enum ClaimState
    {
        None,
        Registred,
        Verified,
        Secured,
        Processed,
        Holded,
        Failed,
        Cancelled
    }

    #endregion

    #region STRUCT_TYPES

    public class MogwaiStruct : StructType
    {
        public override string Name() => "MogwaiStruct<T::Hash, T::BlockNumber, BalanceOf<T>, RarityType>";

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

            Dna = new Hash();
            Dna.Decode(byteArray, ref p);

            Genesis = new BlockNumber();
            Genesis.Decode(byteArray, ref p);

            Price = new Balance();
            Price.Decode(byteArray, ref p);

            Gen = new U32();
            Gen.Decode(byteArray, ref p);

            Rarity = new EnumType<RarityType>();
            Rarity.Decode(byteArray, ref p);

            _size = p - start;
        }

        public Hash Id { get; private set; }
        public Hash Dna { get; private set; }
        public BlockNumber Genesis { get; private set; }
        public Balance Price { get; private set; }
        public U32 Gen { get; private set; }
        public EnumType<RarityType> Rarity { get; private set; }
        public void Create(Hash id, Hash dna, BlockNumber genesis, Balance price, U32 gen, EnumType<RarityType> rarity)
        {
            var start = 0;

            Id = id;
            start += id.Bytes.Length;

            Dna = dna;
            start += dna.Bytes.Length;

            Genesis = genesis;
            start += genesis.Bytes.Length;

            Price = price;
            start += price.Bytes.Length;

            Gen = gen;
            start += gen.Bytes.Length;

            Gen = gen;
            start += gen.Bytes.Length;

            Rarity = rarity;
            start += rarity.Bytes.Length;

            _size = start;
        }
    }

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

        public void Create(Hash id, U32 state, Vec<U8Arr16> metaXy, Balance intrinsic, U8 level, Vec<BlockNumber> phases, Vec<Hash> adaptations)
        {
            var start = 0;

            Id = id;
            start += id.Bytes.Length;

            State = state;
            start += state.Bytes.Length;

            MetaXy = metaXy;
            start += metaXy.Bytes.Length;

            Intrinsic = intrinsic;
            start += intrinsic.Bytes.Length;

            Level = level;
            start += level.Bytes.Length;

            Phases = phases;
            start += phases.Bytes.Length;

            Adaptations = adaptations;
            start += adaptations.Bytes.Length;

            _size = start;
        }
    }

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

    public class MogwaicoinAddress : StructType
    {
        public override string Name() => "MogwaicoinAddress<T::AccountId, ClaimState, BalanceOf<T>>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Address = new Vec<U8>();
            Address.Decode(byteArray, ref p);

            Account = new AccountId();
            Account.Decode(byteArray, ref p);

            Signature = new Vec<U8>();
            Signature.Decode(byteArray, ref p);

            State = new EnumType<ClaimState>();
            State.Decode(byteArray, ref p);

            Balance = new Balance();
            Balance.Decode(byteArray, ref p);

            _size = p - start;
        }

        public Vec<U8> Address { get; private set; }
        public AccountId Account { get; private set; }
        public Vec<U8> Signature { get; private set; }
        public EnumType<ClaimState> State { get; private set; }
        public Balance Balance { get; private set; }

    }

    #endregion

}
