using System;
using Newtonsoft.Json;
using Macquarie.Handbook.Data.Shared;
using System.Diagnostics.CodeAnalysis;

namespace Macquarie.Handbook.Data.Converters {
    public class MacquarieEmbeddedJsonConverter<T> : JsonConverter<T>
    {
        public override T ReadJson(JsonReader reader, Type objectType, [AllowNull] T existingValue, bool hasExistingValue, JsonSerializer serializer) {
            if (reader.Value != null)
                return MacquarieHandbook.DeserialiseJsonObject<T>((string)reader.Value);
            else {
                return default(T);
            }
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] T value, JsonSerializer serializer) {
            serializer.Serialize(writer, value);
        }
    }
}