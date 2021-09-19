using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.Model.Types.Base
{
    public class BaseChar : BasePrim<char>
    {
        public override string TypeName() => "unknown";

        public override int TypeSize() => 1;

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