namespace DotNetToolbox;

public static class ObjectExtensions {
    public static string Dump(this object? value, Action<DumpBuilderOptions>? config = null) {
        var options = new DumpBuilderOptions();
        config?.Invoke(options);
        return DumpBuilder.Build(value, options);
    }

    [MustDisposeResource]
    [SuppressMessage("ReSharper", "NotDisposedResource")]
    internal static IEnumerator? GetEnumerator(this object? value)
        => value switch {
               null => null,
               IEnumerable list => list.GetEnumerator(),
               _ => GetProperties(value.GetType()).GetEnumerator(),
           };

    private static PropertyInfo[] GetProperties(IReflect type)
        => type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToArray();
}
