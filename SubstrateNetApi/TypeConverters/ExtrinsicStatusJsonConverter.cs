using Newtonsoft.Json;
using SubstrateNetApi.MetaDataModel.Rpc;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.TypeConverters
{
    public class ExtrinsicStatusJsonConverter : JsonConverter<ExtrinsicStatus>
    {
        public override ExtrinsicStatus ReadJson(JsonReader reader, Type objectType, ExtrinsicStatus existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var extrinsicStatus = new ExtrinsicStatus();

            if (reader.TokenType == JsonToken.String && Enum.TryParse((string)reader.Value, true, out ExtrinsicState extrinsicState))
            {
                extrinsicStatus.ExtrinsicState = extrinsicState;
            } 
            else if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();

                while (reader.TokenType != JsonToken.EndObject)
                {
                    switch(reader.TokenType)
                    {
                        case JsonToken.PropertyName:

                            if (reader.ValueType == typeof(string))
                            {
                                switch(reader.Value)
                                {
                                    case "broadcast":
                                        reader.Read();
                                        if (reader.TokenType == JsonToken.StartArray)
                                        {
                                            var broadcastList = new List<string>();
                                            while (reader.TokenType != JsonToken.EndArray)
                                            {
                                                if (reader.ValueType == typeof(string))
                                                {
                                                    broadcastList.Add((string)reader.Value);
                                                }
                                                reader.Read();
                                            }
                                            extrinsicStatus.Broadcast = broadcastList.ToArray();
                                        }
                                        break;
                                    case "inBlock":
                                        reader.Read();
                                        extrinsicStatus.InBlock = new Hash((string)reader.Value);
                                        break;
                                    case "finalized":
                                        reader.Read();
                                        extrinsicStatus.Finalized = new Hash((string)reader.Value);
                                        break;
                                    case "finalityTimeout":
                                        reader.Read();
                                        extrinsicStatus.FinalityTimeout = new Hash((string)reader.Value);
                                        break;
                                    case "retracted":
                                        reader.Read();
                                        extrinsicStatus.Retracted = new Hash((string)reader.Value);
                                        break;
                                    case "usurped":
                                        reader.Read();
                                        extrinsicStatus.Usurped = new Hash((string)reader.Value);
                                        break;
                                    default:
                                        throw new NotImplementedException($"Unimplemneted { reader.TokenType } of type '{reader.ValueType}' and value '{reader.Value}'.");
                                }
                            }

                            break;
                        default:
                            throw new NotImplementedException($"Unimplemneted { reader.TokenType } of type '{reader.ValueType}' and value '{reader.Value}'.");
                    }
                    reader.Read();
                }
            }
            return extrinsicStatus;
        }

        public override void WriteJson(JsonWriter writer, ExtrinsicStatus value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
