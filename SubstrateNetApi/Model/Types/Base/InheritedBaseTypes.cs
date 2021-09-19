using SubstrateNetApi.Model.Types.Primitive;

namespace SubstrateNetApi.Model.Types.Base
{
    public class AuthorityId : AccountId
    {
        public override string TypeName() => "AuthorityId";
    }

    public class AuthorityWeight : PrimU64
    {
        public override string TypeName() => "AuthorityWeight";
    }

    public class Topic : Hash
    {
        public override string TypeName() => "Topic";
    }

    public class EraIndex : PrimU32
    {
        public override string TypeName() => "EraIndex";
    }

    public class AccountIndex : PrimU32
    {
        public override string TypeName() => "AccountIndex";
    }

    public class ApplyExtrinsic : PrimU32
    {
        public override string TypeName() => "ApplyExtrinsic";
    }

    public class BountyIndex : PrimU32
    {
        public override string TypeName() => "BountyIndex";
    }

    public class CallHash : Hash
    {
        public override string TypeName() => "CallHash";
    }

    public class MemberCount : PrimU32
    {
        public override string TypeName() => "MemberCount";
    }

    public class PropIndex : PrimU32
    {
        public override string TypeName() => "PropIndex";
    }

    public class SessionIndex : PrimU32
    {
        public override string TypeName() => "SessionIndex";
    }

    public class ProposalIndex : PrimU32
    {
        public override string TypeName() => "ProposalIndex";
    }

    public class ReferendumIndex : PrimU32
    {
        public override string TypeName() => "ReferendumIndex";
    }

    public class RefCount : PrimU32
    {
        public override string TypeName() => "RefCount";
    }

    public class RegistrarIndex : PrimU32
    {
        public override string TypeName() => "RegistrarIndex";
    }
}