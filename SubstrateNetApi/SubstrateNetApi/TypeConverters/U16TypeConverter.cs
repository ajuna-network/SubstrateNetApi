using NLog;
using System;

namespace SubstrateNetApi.TypeConverters
{
    public class U16TypeConverter : ITypeConverter
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public string TypeName { get; } = "u16";
        public object Create(string value)
        {
            byte[] bytes = Utils.HexToByteArray(value);
            Logger.Debug($"Converting {value} [{bytes.Length}] to UInt16.");
            return BitConverter.ToUInt16(bytes, 0);
        }
    }
}
