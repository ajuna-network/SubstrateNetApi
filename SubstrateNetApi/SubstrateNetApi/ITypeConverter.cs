namespace SubstrateNetApi
{
    public interface ITypeConverter
    {
        string TypeName { get; }
        object Create(string value);
    }
}
