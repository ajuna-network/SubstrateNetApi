using System;

namespace SubstrateNetApi.Exceptions
{
    public class ConverterAlreadyRegisteredException : Exception
    {
        public ConverterAlreadyRegisteredException(string message)
            : base(message)
        { }
    }
}
