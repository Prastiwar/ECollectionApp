using ECollectionApp.TagService.Data;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ECollectionApp.TagService.Serialization
{
    public class TagJsonConverter : JsonConverter<Tag>
    {
        public override Tag Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new Tag() {
                    Name = reader.GetString()
                };
            }
            Tag tag = new Tag();
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
                if (string.Compare(propertyName, nameof(Tag.Name), true) == 0)
                {
                    tag.Name = reader.GetString();
                }
                if (string.Compare(propertyName, nameof(Tag.Id), true) == 0)
                {
                    tag.Id = reader.GetInt32();
                }
            }
            return tag;
        }

        public override void Write(Utf8JsonWriter writer, Tag value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            string idPropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(Tag.Id)) ?? nameof(Tag.Id);
            string namePropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(Tag.Name)) ?? nameof(Tag.Name);
            writer.WriteNumber(idPropertyName, value.Id);
            writer.WriteString(namePropertyName, value.Name);
            writer.WriteEndObject();
        }
    }
}
