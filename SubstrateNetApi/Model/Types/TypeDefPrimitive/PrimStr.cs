using System;

namespace SubstrateNetApi.Model.Types.TypeDefPrimitive
{
    public class PrimStr : PrimVec<PrimChar>
    {
        public override string TypeName() => "str";
    }
}