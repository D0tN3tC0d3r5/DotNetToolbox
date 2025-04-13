using static System.Reflection.BindingFlags;

namespace WebApi.Utilities;

/// <summary>
/// Factory to create instances of OptionalConverter
/// Required because the converter itself is generic.
/// </summary>
public class OptionalConverterFactory
    : JsonConverterFactory {
    public override bool CanConvert(Type typeToConvert)
        // Check if the type is a generic Optional<T>
        => typeToConvert.IsGenericType
         && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
        var valueType = typeToConvert.GetGenericArguments()[0];
        return (JsonConverter?)Activator.CreateInstance(typeof(OptionalConverter<>).MakeGenericType(valueType),
                                                        Instance | Public,
                                                        binder: null,
                                                        args: null, // No constructor args needed for OptionalConverter
                                                        culture: null);
    }
}
