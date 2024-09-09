namespace DotNetToolbox.AI.Jobs;

internal class JobOutputConverter : JsonConverter<object> {
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsClass && typeToConvert != typeof(string);

    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch {
            JsonTokenType.Null => null,
            JsonTokenType.StartObject => ReadObject(ref reader, options),
            JsonTokenType.StartArray => ReadArray(ref reader, options),
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
        };

    private Dictionary<string, object?> ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options) {
        var dictionary = new Dictionary<string, object?>();
        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.EndObject) return dictionary;
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
            var propertyName = reader.GetString(); reader.Read();
            dictionary[propertyName!] = ExtractValue(ref reader, options);
        }
        throw new JsonException();
    }

    private List<object?> ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options) {
        var list = new List<object?>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
            list.Add(ExtractValue(ref reader, options));
        }
        return list;
    }

    private object? ExtractValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
        => reader.TokenType switch {
            JsonTokenType.String => reader.TryGetDateTimeOffset(out var dt) ? dt : reader.TryGetGuid(out var guid) ? guid : reader.GetString(),
            JsonTokenType.False => false,
            JsonTokenType.True => true,
            JsonTokenType.Null => null,
            JsonTokenType.Number => reader.TryGetInt32(out var i) ? i : reader.TryGetInt64(out var l) ? l : reader.GetDecimal(),
        JsonTokenType.StartObject => ReadObject(ref reader, options),
        JsonTokenType.StartArray => ReadArray(ref reader, options),
        _ => throw new JsonException($"Unexpected token type: {reader.TokenType}"),
    };

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value, value.GetType(), options);
}

