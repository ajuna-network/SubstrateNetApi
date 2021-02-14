using System;
using System.Xml.Xsl;
using Newtonsoft.Json;

namespace SubstrateNetApi.Model.Types
{
    public class AccountInfo
    {
        public U32 Nonce { get; }

        public RefCount Consumers { get; }

        public RefCount Providers { get; }

        public AccountData AccountData { get; }

        public AccountInfo(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal AccountInfo(Memory<byte> memory)
        {
            var u32 = new U32();
            u32.Create(memory.Slice(0, 4).ToArray());
            Nonce = u32;

            var consumers = new RefCount();
            consumers.Create(memory.Slice(4, 4).ToArray());
            Consumers = consumers;

            var providers = new RefCount();
            providers.Create(memory.Slice(8, 4).ToArray());
            Providers = providers;

            AccountData = new AccountData(memory.Slice(12));
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}