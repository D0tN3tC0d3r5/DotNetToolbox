// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class QueryableExtensions {
    public static void ForEach<T>(this IQueryable source, Action<T> action) {
        IsNotNull(action, nameof(action));
        foreach (var element in source.Cast<T>())
            action(element);
    }
}
