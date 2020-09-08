using NLog;
using System;

namespace SubstrateNetApi.TypeConverters
{
    public class U32TypeConverter : ITypeConverter
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public string TypeName { get; } = "u32";
        public object Create(string value)
        {
            byte[] bytes = Utils.HexToByteArray(value);
            Logger.Debug($"Converting {value} [{bytes.Length}] to UInt32.");
            return BitConverter.ToUInt32(bytes, 0);
        }
    }
}
