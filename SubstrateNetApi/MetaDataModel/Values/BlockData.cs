using Newtonsoft.Json;
using System;
using System.Text;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class BlockData
    {

        public BlockData(Block block, object justification)
        {
            Block = block;
            Justification = justification;
        }

        public Block Block { get; set; }
        public object Justification { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
