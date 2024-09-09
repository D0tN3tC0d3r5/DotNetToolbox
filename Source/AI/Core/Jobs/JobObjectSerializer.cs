namespace DotNetToolbox.AI.Jobs;

internal class JobObjectSerializer
    : JsonConverter<object> {
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsClass && typeToConvert != typeof(string);

    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch {
            JsonTokenType.Null => string.Empty,
            JsonTokenType.StartObject => ReadObject(ref reader, options),
            JsonTokenType.StartArray => ReadArray(ref reader, options),
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}"),
        };

    private Dictionary<string, object> ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options) {
        var dictionary = new Dictionary<string, object>();
        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.EndObject) return dictionary;
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
            var propertyName = reader.GetString();
            reader.Read();
            dictionary[propertyName!] = ExtractValue(ref reader, options);
        }
        throw new JsonException();
    }

    private List<object> ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options) {
        var list = new List<object>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) list.Add(ExtractValue(ref reader, options));
        return list;
    }

    private object ExtractValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
        => reader.TokenType switch {
            JsonTokenType.String => reader.TryGetDateTimeOffset(out var dt) ? dt : reader.TryGetGuid(out var guid) ? guid : reader.GetString() ?? string.Empty,
            JsonTokenType.False => false,
            JsonTokenType.True => true,
            JsonTokenType.Null => string.Empty,
            JsonTokenType.Number => reader.TryGetInt32(out var i) ? i : reader.TryGetInt64(out var l) ? l : reader.GetDecimal(),
            JsonTokenType.StartObject => ReadObject(ref reader, options),
            JsonTokenType.StartArray => ReadArray(ref reader, options),
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}"),
        };

    public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
        => WriteValue(writer, value ?? string.Empty, options, 0);

    private void WriteValue(Utf8JsonWriter writer, object value, JsonSerializerOptions options, int indentLevel) {
        switch (value) {
            case null:
                writer.WriteStringValue("[[No Value]]");
                break;
            case IEnumerable<KeyValuePair<string, object>> dict:
                WriteObject(writer, dict, options, indentLevel);
                break;
            case IEnumerable<object> list:
                WriteArray(writer, list, options, indentLevel);
                break;
            default:
                writer.WriteStringValue(value.ToString());
                break;
        }
    }

    private void WriteObject(Utf8JsonWriter writer, IEnumerable<KeyValuePair<string, object>> dict, JsonSerializerOptions options, int indentLevel) {
        foreach (var item in dict) {
            WriteIndent(writer, indentLevel);
            writer.WritePropertyName(item.Key);
            writer.WriteStringValue(": ");
            WriteValue(writer, item.Value, options, indentLevel + 1);
        }
    }

    private void WriteArray(Utf8JsonWriter writer, IEnumerable<object> list, JsonSerializerOptions options, int indentLevel) {
        writer.WriteStringValue("[");
        var count = 1;
        foreach (var item in list) {
            WriteIndent(writer, indentLevel + 1);
            writer.WriteStringValue($"{count}. ");
            WriteValue(writer, item, options, indentLevel + 2);
            count++;
        }
        WriteIndent(writer, indentLevel);
        writer.WriteStringValue("]");
    }

    private static void WriteIndent(Utf8JsonWriter writer, int indentLevel)
        => writer.WriteStringValue(new string(' ', indentLevel * 4));
}
