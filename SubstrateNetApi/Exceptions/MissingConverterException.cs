using System;

namespace SubstrateNetApi.Exceptions
{
    public class MissingConverterException : Exception
    {
        public MissingConverterException(string message) :
            base(message)
        { }
    }
}
