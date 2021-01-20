using SubstrateNetApi.MetaDataModel.Extrinsics;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel.Types
{
    public class Block
    {
        public Extrinsic[] Extrinsics { get; set; }
        public Header Header { get; set; }

    }
}