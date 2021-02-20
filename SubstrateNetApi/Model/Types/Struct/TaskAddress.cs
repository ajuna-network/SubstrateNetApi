using System;
using System.Collections.Generic;
using System.Text;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class TaskAddress : RustTuple<BlockNumber, U32>
    {
        public override string Name() => "TaskAddress<T::BlockNumber>";
    }
}
