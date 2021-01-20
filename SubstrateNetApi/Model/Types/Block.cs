using SubstrateNetApi.Model.Extrinsics;

namespace SubstrateNetApi.Model.Types
{
    public class Block
    {
        public Extrinsic[] Extrinsics { get; set; }
        public Header Header { get; set; }

    }
}