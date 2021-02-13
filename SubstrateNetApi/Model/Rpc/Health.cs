using Newtonsoft.Json;

namespace SubstrateNetApi.Model.Rpc
{
    public class Health
    {
        public bool IsSyncing { get; set; }
        public int Peers { get; set; }
        public bool ShouldHavePeers { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
