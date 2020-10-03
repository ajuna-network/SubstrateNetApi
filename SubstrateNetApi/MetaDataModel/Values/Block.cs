using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class Block
    {
        public IList<string> Extrinsics { get; set; }
        public Header Header { get; set; }

    }
}
