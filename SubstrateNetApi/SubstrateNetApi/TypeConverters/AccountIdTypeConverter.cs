using NLog;
using SubstrateNetApi.MetaDataModel.Values;

namespace SubstrateNetApi.TypeConverters
{
    public class AccountIdTypeConverter : ITypeConverter
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public string TypeName { get; } = "T::AccountId";
        public object Create(string value)
        {
            Logger.Debug($"Converting {value} to AccountId.");
            return new AccountId(value);
        }
    }
}