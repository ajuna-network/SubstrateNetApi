using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public enum DispatchClass
    {
        Normal, Operational, Mandatory
    }

    public enum Pays
    {
        Yes, No
    }

    public class DispatchInfo
    {
        public ulong Weight { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DispatchClass DispatchClass { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Pays Pays { get; set; }

        public DispatchInfo(ulong weight, DispatchClass dispatchClass, Pays pays)
        {
            Weight = weight;
            DispatchClass = dispatchClass;
            Pays = pays;
        }

        internal static DispatchInfo Decode(Memory<byte> byteArray, ref int p)
        {
            var weight = BitConverter
                .ToUInt64(byteArray.Span.Slice(p, 8).ToArray(), 0);
            p += 8; // weight

            var dispatchClass = (DispatchClass) byteArray.Span.Slice(p, 1)[0];
            p += 1; // class

            var pays = (Pays) byteArray.Span.Slice(p, 1)[0];
            p += 1; // paysFee

            return new DispatchInfo(weight, dispatchClass, pays);
        }
    }
}