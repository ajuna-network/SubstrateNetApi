using SubstrateNetApi.MetaDataModel.Extrinsics;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class Block
    {
        public Extrinsic[] Extrinsics { get; set; }
        public Header Header { get; set; }

    }
}