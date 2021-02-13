using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.Model.Types
{
    public class MogwaiStruct
    {
        public Hash Id { get; }

        public Hash Dna { get; }

        public BlockNumber Genesis { get; }

        public Balance Price { get; }

        public U64 Gen { get; }

        public MogwaiStruct(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal MogwaiStruct(Memory<byte> memory)
        {
            var id = new Hash();
            id.Create(memory.Slice(0, 32).ToArray());
            Id = id;

            var dna = new Hash();
            dna.Create(memory.Slice(32, 32).ToArray());
            Dna = dna;

            var genesis = new BlockNumber();
            genesis.Create(memory.Slice(64, 4).ToArray());
            Genesis = genesis;

            var price = new Balance();
            price.Create(memory.Slice(68, 16).ToArray());
            Price = price;

            var gen = new U64();
            gen.Create(memory.Slice(84, 8).ToArray());
            Gen = gen;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}