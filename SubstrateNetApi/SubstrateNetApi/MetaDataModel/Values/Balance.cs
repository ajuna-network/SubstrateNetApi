using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class Balance
    {
        public BigInteger Value { get; }

        public Balance(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal Balance(Memory<byte> memory)
        {
            Value = new BigInteger(memory.ToArray());
        }
    }
}
