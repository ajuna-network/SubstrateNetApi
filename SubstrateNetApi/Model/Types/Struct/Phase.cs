using System;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class Phase : BaseType
    {
        public override string TypeName() => "Phase";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            PhaseState = new EnumType<PhaseState>();
            PhaseState.Decode(byteArray, ref p);

            if (PhaseState.Value == Enum.PhaseState.None)
            {
                ApplyExtrinsic = new ApplyExtrinsic();
                ApplyExtrinsic.Decode(byteArray, ref p);
            }

            _typeSize = p - start;
        }

        public ApplyExtrinsic ApplyExtrinsic { get; set; }
        public EnumType<PhaseState> PhaseState { get; set; }
    }
}