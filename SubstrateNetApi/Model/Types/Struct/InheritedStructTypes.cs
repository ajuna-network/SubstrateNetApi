using System;
using System.Collections.Generic;
using System.Text;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class AuthorityList : Vec<RustTuple<AuthorityId, AuthorityWeight>>
    {
        public override string TypeName() => "AuthorityList";
    }

    public class StorageKey : Vec<PrimU8>
    {
        public override string TypeName() => "StorageKey";
    }

    public class StorageData : Vec<PrimU8>
    {
        public override string TypeName() => "StorageData";
    }

    public class OpaqueTimeSlot : Vec<PrimU8>
    {
        public override string TypeName() => "OpaqueTimeSlot";
    }

    public class TaskAddress : RustTuple<BlockNumber, PrimU32>
    {
        public override string TypeName() => "TaskAddress<T::BlockNumber>";
    }
}
