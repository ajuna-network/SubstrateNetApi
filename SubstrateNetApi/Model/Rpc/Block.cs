using SubstrateNetApi.Model.Extrinsics;

namespace SubstrateNetApi.Model.Rpc
{
    public class Block
    {
        public Extrinsic[] Extrinsics { get; set; }
        public Header Header { get; set; }

    }
}