using System;
using System.Collections.Generic;
using System.Text;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class OpaqueTimeSlot : Vec<U8>
    {
        public override string Name() => "OpaqueTimeSlot";
    }
}
