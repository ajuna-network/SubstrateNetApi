using System;

namespace SubstrateNetApi.Model.Types
{
    public class DispatchError
    {
        public static DispatchError Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException("DispatchError");
        }
    }

    public class DispatchResult
    {
        public static DispatchResult Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException("DispatchResult");
        }
    }

    public class Kind
    {
        public static Kind Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException("Kind");
        }
    }

    public class Moment
    {
        public byte[] Encode()
        {
            throw new NotImplementedException("Moment");
        }
    }
    
}