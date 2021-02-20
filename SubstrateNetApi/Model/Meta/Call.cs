namespace SubstrateNetApi.Model.Meta
{
    public class Call
    {
        public string Name { get; set; }
        public Argument[] Arguments { get; set; }
        public string[] Documentations { get; set; }
    }
}