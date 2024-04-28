// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class QueryableExtensions {
    public static void Load<TSource>(this IQueryable<TSource> source) {
        using var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext()) { }
    }
}
