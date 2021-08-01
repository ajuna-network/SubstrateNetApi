namespace SubstrateNetApi.Model.Meta
{
    public class Function
    {
        public Storage.Hasher Hasher { get; internal set; }
        public string Key1 { get; internal set; }
        public string Key2 { get; internal set; }
        public string Value { get; internal set; }
        public bool? IsLinked { get; internal set; }
        public Storage.Hasher Key2Hasher { get; internal set; }
    }
}