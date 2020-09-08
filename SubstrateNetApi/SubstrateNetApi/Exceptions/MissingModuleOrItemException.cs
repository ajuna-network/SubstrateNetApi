using System;
using System.Collections.Generic;
using System.Text;

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
