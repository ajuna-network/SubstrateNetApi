using NLog;
using SubstrateNetApi.MetaDataModel.Values;

namespace SubstrateNetApi.TypeConverters
{
    internal class AccountInfoConverter : ITypeConverter
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public string TypeName { get; } = "AccountInfo<T::Index, T::AccountData>";
        public object Create(string value)
        {
            Logger.Debug($"Converting {value} to AccountInfo.");
            return new AccountInfo(value);
        }
    }
}