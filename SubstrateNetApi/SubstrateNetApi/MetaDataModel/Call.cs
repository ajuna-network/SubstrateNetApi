namespace SubstrateNetApi
{
    public class Call
    {
        public string Name { get; internal set; }
        public Argument[] Arguments { get; internal set; }
        public string[] Documentations { get; internal set; }
    }
}