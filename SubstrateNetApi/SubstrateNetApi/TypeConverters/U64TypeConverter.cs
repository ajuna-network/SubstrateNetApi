using NLog;
using System;

namespace SubstrateNetApi.TypeConverters
{
    internal class U64TypeConverter : ITypeConverter
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public string TypeName { get; } = "u64";
        public object Create(string value)
        {
            byte[] bytes = Utils.HexToByteArray(value);
            Logger.Debug($"Converting {value} [{bytes.Length}] to UInt64.");
            return BitConverter.ToUInt64(bytes, 0);
        }
    }
}
