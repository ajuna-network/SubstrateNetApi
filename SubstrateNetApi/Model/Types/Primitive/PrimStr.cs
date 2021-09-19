using System;

namespace SubstrateNetApi.Model.Types.Primitive
{
    public class PrimStr : BaseVec<PrimChar>
    {
        public override string TypeName() => "str";
    }
}