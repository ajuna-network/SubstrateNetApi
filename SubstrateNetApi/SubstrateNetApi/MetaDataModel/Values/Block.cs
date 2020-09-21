using Newtonsoft.Json;
using SubstrateNetApi.TypeConverters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class DigestStr
    {
        public IList<string> Logs { get; set; }
    }

    public class HeaderStr
    {
        public DigestStr Digest { get; set; }
        public string ExtrinsicsRoot { get; set; }

        [JsonConverter(typeof(HexTypeConverter))]
        public ushort Number { get; set; }

        [JsonConverter(typeof(HashTypeConverter))]
        public Hash ParentHash { get; set; }
        public string StateRoot { get; set; }
    }

    public class Block
    {
        public IList<string> Extrinsics { get; set; }
        public HeaderStr Header { get; set; }
    }

    public class BlockData
    {

        public BlockData(Block block, object justification)
        {
            Block = block;
            Justification = justification;
        }

        public Block Block { get; set; }
        public object Justification { get; set; }

        override
        public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
