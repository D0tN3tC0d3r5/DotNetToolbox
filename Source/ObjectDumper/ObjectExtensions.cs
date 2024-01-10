namespace DotNetToolbox;

public static class ObjectExtensions {
    public static string Dump(this object? value, Action<DumpBuilderOptions>? config = null) {
        var options = new DumpBuilderOptions();
        config?.Invoke(options);
        return DumpBuilder.Build(value, options);
    }

    public static string DumpAsJson(this object? value, Action<JsonDumpBuilderOptions>? config = null) {
        var options = new JsonDumpBuilderOptions();
        config?.Invoke(options);
        return JsonDumpBuilder.Build(value, options);
    }
}
