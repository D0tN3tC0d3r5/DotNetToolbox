namespace DotNetToolbox;

internal static class JsonDumpBuilder {
    public static string Build(object? value, JsonDumpBuilderOptions options)
        => value is null
           ? "null"
           : JsonSerializer.Serialize(value, new JsonSerializerOptions {
               WriteIndented = options.Indented,
               MaxDepth = options.MaxDepth,
           });
}
