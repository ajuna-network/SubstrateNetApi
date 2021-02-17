using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types
{
    public class BountyIndex : U32
    {
        public override string Name()
        {
            return "BountyIndex";
        }
    }

    public class CallHash : Hash
    {
        public override string Name()
        {
            return "CallHash";
        }
    }

    public class EraIndex : U32
    {
        public override string Name()
        {
            return "EraIndex";
        }
    }

    public class MemberCount : U32
    {
        public override string Name()
        {
            return "MemberCount";
        }
    }

    public class PropIndex : U32
    {
        public override string Name()
        {
            return "PropIndex";
        }
    }

    public class SessionIndex : U32
    {
        public override string Name()
        {
            return "SessionIndex";
        }
    }

    public class ProposalIndex : U32
    {
        public override string Name()
        {
            return "ProposalIndex";
        }
    }

    public class ReferendumIndex : U32
    {
        public override string Name()
        {
            return "ReferendumIndex";
        }
    }

    public class RefCount : U32
    {
        public override string Name()
        {
            return "RefCount";
        }
    }

    public class RegistrarIndex : U32
    {
        public override string Name()
        {
            return "RegistrarIndex";
        }
    }
}