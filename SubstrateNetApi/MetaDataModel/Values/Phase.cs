namespace SubstrateNetApi.MetaDataModel.Values
{
    public enum PhaseState
    {
        None, Finalization, Initialization
    }

    public class Phase
    {
        public uint ApplyExtrinsic { get; set; }

        public PhaseState PhaseState { get; set; }

        public Phase(uint applyExtrinisc)
        {
            ApplyExtrinsic = applyExtrinisc;
            PhaseState = PhaseState.None;
        }
    }
}