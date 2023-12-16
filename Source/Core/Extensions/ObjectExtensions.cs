namespace DotNetToolbox.Extensions;

public static class ObjectExtensions {
    public static string Dump(this object? value, Action<DumpOptions>? config = null) {
        var options = new DumpOptions();
        config?.Invoke(options);
        return DumpBuilder.Build(value, options);
    }
}
