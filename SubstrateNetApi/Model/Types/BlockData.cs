using Newtonsoft.Json;

namespace SubstrateNetApi.Model.Types
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
