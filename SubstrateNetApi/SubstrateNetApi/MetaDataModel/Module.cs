namespace SubstrateNetApi.MetaDataModel
{
    public class Module
    {
        public string Name { get; set; }
        public Storage Storage { get; set; }
        public Call[] Calls { get; set; }
        public Event[] Events { get; set; }
        public Const[] Consts { get; set; }
        public Error[] Errors { get; set; }

    }
}