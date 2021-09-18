using System;
using System.Collections.Generic;
using System.Text;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class AuthorityList : Vec<RustTuple<AuthorityId, AuthorityWeight>>
    {
        public override string TypeName() => "AuthorityList";
    }

    public class StorageKey : Vec<U8>
    {
        public override string TypeName() => "StorageKey";
    }

    public class StorageData : Vec<U8>
    {
        public override string TypeName() => "StorageData";
    }

    public class OpaqueTimeSlot : Vec<U8>
    {
        public override string TypeName() => "OpaqueTimeSlot";
    }

    public class TaskAddress : RustTuple<BlockNumber, U32>
    {
        public override string TypeName() => "TaskAddress<T::BlockNumber>";
    }
}
