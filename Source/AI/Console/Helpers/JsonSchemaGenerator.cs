namespace AI.Sample.Helpers;

public static class JsonSchemaGenerator {
    public static string GenerateSchemaFor<T>()
        where T : class
        => GenerateSchema(typeof(T));

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

        return JsonSerializer.Serialize(schema, new JsonSerializerOptions { WriteIndented = true });
    }

    private static Dictionary<string, object> GenerateArrayItems(Type type) {
        var elementType = type.GetElementType() ?? type.GetGenericArguments().FirstOrDefault();
        return elementType == null
            ? new Dictionary<string, object> { ["type"] = "object" }
            : GenerateSchemaForType(elementType);
    }

    private static string GetJsonType(Type type) => type switch
    {
        Type t when t == typeof(string) => "string",
        Type t when t == typeof(DateTime) || t == typeof(DateTimeOffset) || t == typeof(TimeSpan) ||
                    t == typeof(Guid) || t == typeof(Uri) || t == typeof(Version) => "string",
        Type t when t.IsEnum => "string",
        Type t when t == typeof(int) || t == typeof(long) || t == typeof(float) || t == typeof(double) ||
                    t == typeof(decimal) || t == typeof(short) || t == typeof(ushort) || t == typeof(uint) ||
                    t == typeof(ulong) || t == typeof(byte) || t == typeof(sbyte) => "number",
        Type t when t == typeof(bool) => "boolean",
        Type t when t.IsArray || (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(List<>) ||
                    t.GetGenericTypeDefinition() == typeof(IEnumerable<>))) => "array",
        Type t when t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>) =>
            GetJsonType(Nullable.GetUnderlyingType(t)!),
        Type t when t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>) => "object",
        _ => "object"
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
            if (requiredProperties.Any()) {
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
            prop.GetCustomAttribute<NullableAttribute>()?.NullableFlags?[0] == 2;
}
