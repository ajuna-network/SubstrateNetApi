using System;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class MogwaiStruct : StructType
    {
        public override string Name() => "MogwaiStruct<T::Hash, T::BlockNumber, BalanceOf<T>>";

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

            Gen = new U64();
            Gen.Decode(byteArray, ref p);

            _size = p - start;
        }

        public Hash Id { get; private set; }
        public Hash Dna { get; private set; }
        public BlockNumber Genesis { get; private set; }
        public Balance Price { get; private set; }
        public U64 Gen { get; private set; }

        public void Create(Hash id, Hash dna, BlockNumber genesis, Balance price, U64 gen)
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

            _size = start;
        }
    }
}