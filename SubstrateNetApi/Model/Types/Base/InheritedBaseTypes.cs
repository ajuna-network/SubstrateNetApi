using SubstrateNetApi.Model.Types.Primitive;

namespace SubstrateNetApi.Model.Types.Base
{
    public class AuthorityId : AccountId
    {
        public override string TypeName() => "AuthorityId";
    }

    public class AuthorityWeight : U64
    {
        public override string TypeName() => "AuthorityWeight";
    }

    public class Topic : Hash
    {
        public override string TypeName() => "Topic";
    }

    public class EraIndex : U32
    {
        public override string TypeName() => "EraIndex";
    }

    public class AccountIndex : U32
    {
        public override string TypeName() => "AccountIndex";
    }

    public class ApplyExtrinsic : U32
    {
        public override string TypeName() => "ApplyExtrinsic";
    }

    public class BountyIndex : U32
    {
        public override string TypeName() => "BountyIndex";
    }

    public class CallHash : Hash
    {
        public override string TypeName() => "CallHash";
    }

    public class MemberCount : U32
    {
        public override string TypeName() => "MemberCount";
    }

    public class PropIndex : U32
    {
        public override string TypeName() => "PropIndex";
    }

    public class SessionIndex : U32
    {
        public override string TypeName() => "SessionIndex";
    }

    public class ProposalIndex : U32
    {
        public override string TypeName() => "ProposalIndex";
    }

    public class ReferendumIndex : U32
    {
        public override string TypeName() => "ReferendumIndex";
    }

    public class RefCount : U32
    {
        public override string TypeName() => "RefCount";
    }

    public class RegistrarIndex : U32
    {
        public override string TypeName() => "RegistrarIndex";
    }
}