using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class MogwaiStruct
    {
        public Hash Id { get; }

        public Hash Dna { get; }

        public BlockNumber Genesis { get; }

        public Balance Price { get; }

        public ulong Gen { get; }

        public MogwaiStruct(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }
         
        internal MogwaiStruct(Memory<byte> memory)
        {
            Id = new Hash(memory.Slice(0, 32));
            Dna = new Hash(memory.Slice(32, 32));
            Genesis = new BlockNumber(memory.Slice(64, 4).ToArray());
            Price = new Balance(memory.Slice(68, 16));
            Gen = BitConverter.ToUInt64(memory.Slice(84, 8).ToArray(), 0);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}