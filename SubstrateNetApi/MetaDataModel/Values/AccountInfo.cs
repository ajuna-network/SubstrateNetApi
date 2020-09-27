using Newtonsoft.Json;
using System;

namespace SubstrateNetApi.MetaDataModel.Values
{
    internal class AccountInfo
    {
        public uint Nonce { get; }

        public byte RefCount {get;}

        public AccountData AccountData { get; }

        public AccountInfo(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal AccountInfo(Memory<byte> memory)
        {
            Nonce = BitConverter.ToUInt32(memory.Slice(0, 4).ToArray(), 0);
            RefCount = memory.Slice(4, 1).ToArray()[0];
            AccountData = new AccountData(memory.Slice(5));
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}