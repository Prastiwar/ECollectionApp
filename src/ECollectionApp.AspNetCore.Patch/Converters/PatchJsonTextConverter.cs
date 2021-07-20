using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ECollectionApp.AspNetCore.Patch.Converters
{
    public class PatchJsonTextConverter : JsonConverter<PatchDocument>
    {
        public override PatchDocument Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<KeyValuePair<string, object>> changes = new List<KeyValuePair<string, object>>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }
                string propertyName = reader.GetString();
                reader.Read();
                object value = JsonSerializer.Deserialize(ref reader, typeof(object), options);
                changes.Add(new KeyValuePair<string, object>(propertyName, value));
            }
            return new PatchDocument(changes);
        }

        public override void Write(Utf8JsonWriter writer, PatchDocument value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (KeyValuePair<string, object> changes in value)
            {
                string propertyName = changes.Key;
                writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName);
                JsonSerializer.Serialize(writer, value, options);
            }
            writer.WriteEndObject();
        }
    }
}
