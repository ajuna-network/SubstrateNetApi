using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Struct;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SubstrateNetApi.Model.Types.Custom
{
    public class CustomU32 : PrimU32
    {
        public override string TypeName() => "CustomU32";
    }

}
