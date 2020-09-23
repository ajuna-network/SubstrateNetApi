using System;

namespace SubstrateNetApi.Exceptions
{
    public class MissingParameterException : Exception
    {
        public MissingParameterException(string message)
            : base(message)
        { }
    }
}
