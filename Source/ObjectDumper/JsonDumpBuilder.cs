namespace DotNetToolbox;

internal static class JsonDumpBuilder {
    private static readonly Dictionary<JsonDumpBuilderOptions, JsonSerializerOptions> _cache = [];
    public static string Build(object? value, JsonDumpBuilderOptions options) {
        if (value is null) return "null";
        if (!_cache.TryGetValue(options, out var jsonOptions)) {
            _cache[options] = jsonOptions = new() {
                WriteIndented = options.Indented,
                MaxDepth = options.MaxDepth,
            };
        }

        return JsonSerializer.Serialize(value, jsonOptions);
    }
}
