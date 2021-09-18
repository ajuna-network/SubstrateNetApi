using System;
using Newtonsoft.Json;
using SubstrateNetApi.Model.Types;

namespace SubstrateNetApi.TypeConverters
{
    public class GenericTypeConverter<T> : JsonConverter<T>, ITypeConverter where T : IType, new()
    {
        /// <summary>Gets the name of the type.</summary>
        /// <value>The name of the type.</value>
        public string TypeName { get; } = new T().TypeName();

        /// <summary>Creates a new object.</summary>
        /// <param name="value">The value.</param>
        /// <returns>An object.</returns>
        public object Create(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            var baseType = new T();
            baseType.Create(value);
            return baseType;
        }

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read. If there is no existing value then <c>null</c> will be used.</param>
        /// <param name="hasExistingValue">The existing value has a value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var baseType = new T();
            baseType.CreateFromJson((string) reader.Value);
            return baseType;
        }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}