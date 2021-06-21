using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Struct;
using System;
using System.Numerics;

namespace SubstrateNetApi.Model.Types.Custom
{
    #region BASE_TYPES

 
    #endregion

    #region ENUM_TYPES

    public enum BoardState
    {
        None,
        Running,
        Finished

    }
    public class BoardStateEnum : ExtEnumType<BoardState, NullType, NullType, AccountId, NullType, NullType, NullType, NullType, NullType, NullType> {
        public override string Name() => "BoardState<T::AccountId>";
    }

    #endregion

    #region STRUCT_TYPES

    public class BoardStruct : StructType
    {
        public override string Name() => "BoardStruct<T::Hash, T::AccountId, T::BlockNumber, BoardState<T::AccountId>>";

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

    #endregion

}
