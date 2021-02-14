using System;
using System.Xml.Xsl;
using Newtonsoft.Json;

namespace SubstrateNetApi.Model.Types
{
    public class AccountInfo
    {
        public uint Nonce { get; }

        public RefCount Consumers { get; }

        public RefCount Providers { get; }

        public AccountData AccountData { get; }

        public AccountInfo(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal AccountInfo(Memory<byte> memory)
        {
            Nonce = BitConverter.ToUInt32(memory.Slice(0, 4).ToArray(), 0);

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