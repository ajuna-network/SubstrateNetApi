using NLog;
using SubstrateNetApi.MetaDataModel.Values;

namespace SubstrateNetApi.TypeConverters
{
    public class MogwaiStructTypeConverter : ITypeConverter
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public string TypeName { get; } = "MogwaiStruct<T::Hash, BalanceOf<T>>";
        public object Create(string value)
        {
            Logger.Debug($"Converting {value} to MogwaiStruct.");
            return new MogwaiStruct(value);
        }
    }
}
