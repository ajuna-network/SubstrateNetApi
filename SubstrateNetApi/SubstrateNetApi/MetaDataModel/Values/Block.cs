using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class Digest
    {
        public IList<string> Logs { get; set; }
    }

    public class Header
    {
        public Digest Digest { get; set; }
        public string ExtrinsicsRoot { get; set; }
        public string Number { get; set; }
        public string ParentHash { get; set; }
        public string StateRoot { get; set; }
    }

    public class Block
    {
        public IList<string> Extrinsics { get; set; }
        public Header Header { get; set; }
    }

    public class BlockData
    {
        public Block Block { get; set; }
        public object Justification { get; set; }

        override
        public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
