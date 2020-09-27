using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.MetaDataModel.Values
{
    internal class MogwaiStruct
    {
        public Hash Id { get; }

        public Hash Dna { get; }

        public Balance Price { get; }

        public ulong Gen { get; }

        public MogwaiStruct(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }
         
        internal MogwaiStruct(Memory<byte> memory)
        {
            Id = new Hash(memory.Slice(0, 32));
            Dna = new Hash(memory.Slice(32, 32));
            Price = new Balance(memory.Slice(64, 16));
            Gen = BitConverter.ToUInt64(memory.Slice(80, 8).ToArray(), 0);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}