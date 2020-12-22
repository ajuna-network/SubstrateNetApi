using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class AccountInfo
    {
        public uint Nonce { get; }

        public uint RefCount {get;}

        public AccountData AccountData { get; }

        public AccountInfo(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal AccountInfo(Memory<byte> memory)
        {
            Nonce = BitConverter.ToUInt32(memory.Slice(0, 4).ToArray(), 0);
            RefCount = BitConverter.ToUInt32(memory.Slice(4, 4).ToArray(), 0);
            AccountData = new AccountData(memory.Slice(8));
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}