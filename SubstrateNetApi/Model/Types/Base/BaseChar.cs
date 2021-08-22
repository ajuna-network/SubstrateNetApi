using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.Model.Types.Base
{
    public class BaseChar : BaseType<char>
    {
        public override string Name() => "unknown";

        public override int Size() => 1;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = Encoding.Default.GetString(byteArray)[0];
        }
    }
}