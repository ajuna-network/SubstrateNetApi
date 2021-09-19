using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.Model.Types.Base
{
    public class AssetId : PrimU32
    {
        public override string TypeName() => "AssetId";
    }
}
