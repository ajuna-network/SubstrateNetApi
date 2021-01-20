namespace SubstrateNetApi.Model.Meta
{
    public class Item
    {
        public string Name { get; set; }
        public Storage.Modifier Modifier { get; set; }
        public Storage.Type Type { get; set; }
        public Function Function { get; set; }
        public string FallBack { get; set; }
        public string[] Documentations { get; set; }
    }
}