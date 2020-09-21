using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SubstrateNetApi.MetaDataModel.Values
{
    public class HashConverter : JsonConverter<Hash>
    {
        public override Hash ReadJson(JsonReader reader, Type objectType, Hash existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            return new Hash(s);
        }

        public override void WriteJson(JsonWriter writer, Hash value, JsonSerializer serializer)
        {
            writer.WriteValue(value.HexString);
        }
    }
    public class Hash
    {
        public const int HEXSIZE = 32;

        public string HexString { get; }

        [JsonIgnore]
        public byte[] Bytes { get; }

        public Hash(string str) : this(Utils.HexToByteArray(str).AsMemory())
        {
        }

        internal Hash(Memory<byte> memory)
        {
            Bytes = memory.ToArray();
            HexString = Utils.Bytes2HexString(Bytes, Utils.HexStringFormat.PREFIXED);
        }

        override
        public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}