using System;

namespace SubstrateNetApi.Model.Types.TypeDefPrimitive
{
    public class PrimStr : BaseVec<PrimChar>
    {
        public override string TypeName() => "str";
    }
}