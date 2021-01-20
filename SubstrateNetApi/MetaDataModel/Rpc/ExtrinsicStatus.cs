using Newtonsoft.Json;
using SubstrateNetApi.MetaDataModel.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.MetaDataModel.Rpc
{
    public enum ExtrinsicState
    {
        NONE, FUTURE, READY, DROPPED, INVALID
    }

    public class ExtrinsicStatus
    {
        public ExtrinsicState ExtrinsicState { get; set; }
        public string[] Broadcast { get; set; }
        public Hash InBlock { get; set; }
        public Hash Retracted { get; set; }
        public Hash FinalityTimeout { get; set; }
        public Hash Finalized { get; set; }
        public Hash Usurped { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
