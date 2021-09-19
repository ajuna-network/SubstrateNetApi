using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class AuthorityList : BaseVec<BaseTuple<AuthorityId, AuthorityWeight>>
    {
        public override string TypeName() => "AuthorityList";
    }

    public class StorageKey : BaseVec<U8>
    {
        public override string TypeName() => "StorageKey";
    }

    public class StorageData : BaseVec<U8>
    {
        public override string TypeName() => "StorageData";
    }

    public class OpaqueTimeSlot : BaseVec<U8>
    {
        public override string TypeName() => "OpaqueTimeSlot";
    }

    public class TaskAddress : BaseTuple<BlockNumber, U32>
    {
        public override string TypeName() => "TaskAddress<T::BlockNumber>";
    }
}
