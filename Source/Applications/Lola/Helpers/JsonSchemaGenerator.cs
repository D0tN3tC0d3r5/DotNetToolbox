namespace Lola.Helpers;

public static class JsonSchemaGenerator {
    public static string GenerateSchemaFor<T>()
        where T : class
        => GenerateSchema(typeof(T));

    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public static string GenerateSchema(Type type) {
        var schema = new Dictionary<string, object> {
            ["$schema"] = "https://json-schema.org/draft/2020-12/schema",
            ["type"] = GetJsonType(type)
        };

        if (GetJsonType(type) == "object") {
            schema["properties"] = GenerateProperties(type);
            var requiredProperties = GetRequiredProperties(type);
            if (requiredProperties.Count != 0) {
                schema["required"] = requiredProperties;
            }
        }
        else if (GetJsonType(type) == "array") {
            schema["items"] = GenerateArrayItems(type);
        }

        return JsonSerializer.Serialize(schema, _jsonOptions);
    }

    private static Dictionary<string, object> GenerateArrayItems(Type type) {
        var elementType = type.GetElementType() ?? type.GetGenericArguments().FirstOrDefault();
        return elementType == null
            ? new() { ["type"] = "object" }
            : GenerateSchemaForType(elementType);
    }

    private static string GetJsonType(Type type) => type switch {
        not null when type == typeof(string) => "string",
        not null when type == typeof(DateTime)
                   || type == typeof(DateTimeOffset)
                   || type == typeof(TimeSpan)
                   || type == typeof(Guid)
                   || type == typeof(Uri)
                   || type == typeof(Version) => "string",
        not null when type.IsEnum => "string",
        not null when type == typeof(int)
                   || type == typeof(long)
                   || type == typeof(float)
                   || type == typeof(double)
                   || type == typeof(decimal)
                   || type == typeof(short)
                   || type == typeof(ushort)
                   || type == typeof(uint)
                   || type == typeof(ulong)
                   || type == typeof(byte)
                   || type == typeof(sbyte) => "number",
        not null when type == typeof(bool) => "boolean",
        not null when type.IsArray
                   || (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)
                                           || type.GetGenericTypeDefinition() == typeof(IEnumerable<>))) => "array",
        not null when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) => GetJsonType(Nullable.GetUnderlyingType(type)!),
        not null when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>) => "object",
        _ => "object",
    };
    private static Dictionary<string, object> GenerateProperties(Type type) {
        var properties = new Dictionary<string, object>();

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
            properties[prop.Name] = GenerateSchemaForType(prop.PropertyType);
            AddAttributeInfo((Dictionary<string, object>)properties[prop.Name], prop);
        }

        return properties;
    }

    private static Dictionary<string, object> GenerateSchemaForType(Type type) {
        var schema = new Dictionary<string, object> {
            ["type"] = GetJsonType(type)
        };

        if (type.IsEnum) {
            schema["enum"] = Enum.GetNames(type);
        }
        else if (GetJsonType(type) == "object") {
            schema["properties"] = GenerateProperties(type);
            var requiredProperties = GetRequiredProperties(type);
            if (requiredProperties.Count != 0) {
                schema["required"] = requiredProperties;
            }
        }
        else if (GetJsonType(type) == "array") {
            schema["items"] = GenerateArrayItems(type);
        }

        return schema;
    }

    private static void AddAttributeInfo(Dictionary<string, object> schema, PropertyInfo prop) {
        var descriptionAttribute = prop.GetCustomAttribute<DescriptionAttribute>();
        if (descriptionAttribute != null) {
            schema["description"] = descriptionAttribute.Description;
        }

        if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTimeOffset)) {
            schema["format"] = "date-time";
        }
        else if (prop.PropertyType == typeof(Guid)) {
            schema["format"] = "uuid";
        }

        var rangeAttribute = prop.GetCustomAttribute<RangeAttribute>();
        if (rangeAttribute != null) {
            schema["minimum"] = rangeAttribute.Minimum;
            schema["maximum"] = rangeAttribute.Maximum;
        }

        var stringLengthAttribute = prop.GetCustomAttribute<StringLengthAttribute>();
        if (stringLengthAttribute != null) {
            if (stringLengthAttribute.MinimumLength > 0)
                schema["minLength"] = stringLengthAttribute.MinimumLength;
            schema["maxLength"] = stringLengthAttribute.MaximumLength;
        }

        var regexAttribute = prop.GetCustomAttribute<RegularExpressionAttribute>();
        if (regexAttribute != null) {
            schema["pattern"] = regexAttribute.Pattern;
        }
    }

    private static List<string> GetRequiredProperties(Type type)
        => type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(p => (Nullable.GetUnderlyingType(p.PropertyType) == null
                        && p.PropertyType.IsValueType
                     && !IsNullableReferenceType(p))
                     || p.GetCustomAttribute<RequiredAttribute>() != null)
            .Select(p => p.Name)
            .ToList();

    private static bool IsNullableReferenceType(PropertyInfo prop)
        => !prop.PropertyType.IsValueType &&
            prop.GetCustomAttribute<NullableAttribute>()?.NullableFlags[0] == 2;
}
