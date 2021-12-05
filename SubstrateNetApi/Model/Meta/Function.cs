namespace SubstrateNetApi.Model.Meta
{
    public class Function
    {
        public Storage.Hasher Hasher { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Value { get; set; }
        public bool? IsLinked { get; set; }
        public Storage.Hasher Key2Hasher { get; set; }
    }
}