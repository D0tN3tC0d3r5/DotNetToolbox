namespace WebApi.Utilities;

/// <summary>
/// JsonConverter for the Optional<T> type.
/// Handles deserialization based on property presence and serializes appropriately.
/// </summary>
/// <typeparam name="T">The underlying type of the Optional.</typeparam>
internal sealed class OptionalConverter<T> : JsonConverter<Optional<T>> {
    /// <summary>
    /// Reads the JSON and deserializes it into an Optional<T>.
    /// This method is only called if the property *exists* in the JSON payload.
    /// If the property is missing, the default Optional<T> (None) will be used.
    /// </summary>
    public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        // If Read is called, the property was present in the JSON.
        // Deserialize the inner value (which might be null).
        var value = JsonSerializer.Deserialize<T?>(ref reader, options);

        // Return an Optional representing a value that was explicitly set.
        return Optional<T>.Some(value);
    }

    /// <summary>
    /// Writes the Optional value to JSON.
    /// If IsSet is true, writes the inner Value.
    /// If IsSet is false, writes null (as omitting requires higher-level handling).
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Optional<T> optionalValue, JsonSerializerOptions options) {
        if (optionalValue.IsSet)             // Serialize the actual value if it's present
{
            JsonSerializer.Serialize(writer, optionalValue.Value, options);
        }
        else {
            // Write JSON null if the Optional represents 'None'.
            // Note: To completely omit the property during serialization,
            // you would typically configure [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            // on the property in your DTO and ensure Optional<T>.None is the default value.
            writer.WriteNullValue();
        }
    }
}
