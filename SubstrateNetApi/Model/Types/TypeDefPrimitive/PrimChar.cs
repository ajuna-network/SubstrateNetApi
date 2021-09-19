using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.Model.Types.TypeDefPrimitive
{
    public class PrimChar : BasePrim<char>
    {
        public override string TypeName() => "char";

        public override int TypeSize() => 1;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Create(byte[] byteArray)
        {
            Bytes = byteArray;
            Value = Encoding.UTF8.GetString(byteArray)[0];
        }
    }
}