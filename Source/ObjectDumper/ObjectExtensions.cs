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
               _ => GetMembers(value.GetType()).GetEnumerator(),
           };

    private static readonly BindingFlags _allPublic = BindingFlags.Public | BindingFlags.Instance;
    private static PropertyInfo[] GetMembers(IReflect type)
        => type.GetProperties(_allPublic).ToArray();
}
