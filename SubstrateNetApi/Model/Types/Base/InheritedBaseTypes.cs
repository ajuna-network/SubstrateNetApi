using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types
{
    public class BountyIndex : U32
    {
        public override string Name() => "BountyIndex";
    }

    public class CallHash : Hash
    {
        public override string Name() => "CallHash";
    }

    public class EraIndex : U32
    {
        public override string Name() => "EraIndex";
    }

    public class MemberCount : U32
    {
        public override string Name() => "MemberCount";
    }

    public class PropIndex : U32
    {
        public override string Name() => "PropIndex";
    }

    public class SessionIndex : U32
    {
        public override string Name() => "SessionIndex";
    }

    public class ProposalIndex : U32
    {
        public override string Name() => "ProposalIndex";
    }

    public class ReferendumIndex : U32
    {
        public override string Name() => "ReferendumIndex";
    }

    public class RefCount : U32
    {
        public override string Name() => "RefCount";
    }

    public class RegistrarIndex : U32
    {
        public override string Name() => "RegistrarIndex";
    }
}