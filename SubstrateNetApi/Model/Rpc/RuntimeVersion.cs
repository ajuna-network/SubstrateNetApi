using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.Model.Rpc
{
    public class RuntimeVersion
    {
        public object[][] Apis { get; set; }
        public int AuthoringVersion { get; set; }
        public string ImplName { get; set; }
        public uint ImplVersion { get; set; }
        public string SpecName { get; set; }
        public uint SpecVersion { get; set; }
        public uint TransactionVersion { get; set; }
    }
}
