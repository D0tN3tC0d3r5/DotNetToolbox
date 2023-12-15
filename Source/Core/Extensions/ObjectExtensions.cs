namespace DotNetToolbox.Extensions;

public static class ObjectExtensions {
    public static string Dump(this object? value, Action<DumperOptions>? config = null) {
        var options = new DumperOptions();
        config?.Invoke(options);
        return ObjectDumper.Dump(0, null, value, options, null);
    }
}
