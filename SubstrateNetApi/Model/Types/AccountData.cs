using System;
using Newtonsoft.Json;

namespace SubstrateNetApi.Model.Types
{
    public class AccountData
    {
        public Balance Free { get; }

        public Balance Reserved { get; }

        public Balance MiscFrozen { get; }

        public Balance FeeFrozen { get; }

        public AccountData(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal AccountData(Memory<byte> memory)
        {
            var free = new Balance();
            free.Create(memory.Slice(0, 16).ToArray());
            Free = free;

            var reserved = new Balance();
            reserved.Create(memory.Slice(16, 16).ToArray());
            Reserved = reserved;

            var miscFrozen = new Balance();
            miscFrozen.Create(memory.Slice(32, 16).ToArray());
            MiscFrozen = miscFrozen;

            var feeFrozen = new Balance();
            feeFrozen.Create(memory.Slice(48, 16).ToArray());
            FeeFrozen = feeFrozen;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}