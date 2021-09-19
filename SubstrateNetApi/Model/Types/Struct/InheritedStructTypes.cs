using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class AuthorityList : Vec<BaseTuple<AuthorityId, AuthorityWeight>>
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

    public class TaskAddress : BaseTuple<BlockNumber, U32>
    {
        public override string TypeName() => "TaskAddress<T::BlockNumber>";
    }
}
