namespace DotNetToolbox.AI.Jobs;

internal static partial class JobInputHelper {
    private static readonly JsonSerializerOptions _jsonOptions = new() {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        Converters = { new JobObjectSerializer() },
    };

    public static string FormatInput(object? input, string? template = null) {
        if (string.IsNullOrWhiteSpace(template)) return input is null ? string.Empty : JsonSerializer.Serialize(input);
        var keys = ExtractKeys(template);
        if (keys.Length == 0) return template;

        if (IsNotNull(input) is not IEnumerable<KeyValuePair<string, object>> && !input.GetType().IsClass)
            throw new ArgumentException("Input must be either Dictionary<string, object> or an Class.", nameof(input));
        var result = template;
        foreach (var key in keys) {
            var replacement = FindValue(input, key);
            result = result.Replace($"<<{key}>>", replacement);
        }

        return result;
    }

    private static string[] ExtractKeys(string template)
        => MatchInputKey.Matches(template).Select(m => m.Groups[1].Value).Distinct().ToArray();

    private static string FindValue(object input, string key) => input switch {
        IEnumerable<KeyValuePair<string, object>> map when map.Any(x => x.Key == key) => FormatValue(map.First(x => x.Key == key).Value),
        IEnumerable<KeyValuePair<string, object>> => throw new KeyNotFoundException($"Key '{key}' not found in the input."),
        _ when input.GetType().GetProperty(key) is { } prop => FormatValue(prop.GetValue(input)),
        _ => throw new MissingMemberException($"Property '{key}' not found in the input object."),
    };

    private static string FormatValue(object? value)
        => value switch {
            null => "[[no value found]]",
            IEnumerable<object> enumerable => string.Join("\n", enumerable.Select(static item => $" - {item}")),
            _ => JsonSerializer.Serialize(value, _jsonOptions),
        };

    [GeneratedRegex(@"<<(\w+)>>", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex MatchInputKey { get; }
}
