using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class AuthorityList : Vec<RustTuple<AuthorityId, AuthorityWeight>>
    {
        public override string Name() => "AuthorityList";
    }
}