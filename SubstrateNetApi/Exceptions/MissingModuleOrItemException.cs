using System;

namespace SubstrateNetApi.Exceptions
{
    public class MissingModuleOrItemException : Exception
    {
        public MissingModuleOrItemException(string message)
            : base(message)
        {

        }
    }
}
