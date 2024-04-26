// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class QueryableExtensions {
    public static HashSet<TOutput> ToHashSet<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> project)
        => [.. IsNotNull(source).Select(project)];
}
