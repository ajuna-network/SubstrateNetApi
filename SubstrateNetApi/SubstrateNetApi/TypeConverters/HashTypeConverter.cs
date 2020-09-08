using NLog;
using SubstrateNetApi.MetaDataModel.Values;

namespace SubstrateNetApi.TypeConverters
{
    internal class HashTypeConverter : ITypeConverter
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public string TypeName { get; } = "T::Hash";
        public object Create(string value)
        {
            Logger.Debug($"Converting {value} to Hash.");
            return new Hash(value);
        }
    }
}
