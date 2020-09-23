using System;

namespace SubstrateNetApi.Exceptions
{
    public class ClientNotConnectedException : Exception
    {
        public ClientNotConnectedException(string message)
            : base(message)
        { }
    }
}
