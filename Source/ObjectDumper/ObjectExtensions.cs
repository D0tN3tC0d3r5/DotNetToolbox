namespace DotNetToolbox;

public static class ObjectExtensions {
    public static string Dump(this object? value, Action<DumpBuilderOptions>? config = null) {
        var options = new DumpBuilderOptions();
        config?.Invoke(options);
        return DumpBuilder.Build(value, options);
    }
}
