// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class QueryableExtensions {
    public static HashSet<TItem> ToHashSet<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, TItem>> project)
        => [.. IsNotNull(source).Select(project)];
    public static HashSet<TResult> ToHashSet<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, TResult>> project)
        => [.. IsNotNull(source).Select(project)];
    public static HashSet<TItem> ToHashSet<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, TItem>> project, IEqualityComparer<TItem> comparer)
        => IsNotNull(source).Select(project).ToHashSet(comparer);
    public static HashSet<TResult> ToHashSet<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, TResult>> project, IEqualityComparer<TResult> comparer)
        => IsNotNull(source).Select(project).ToHashSet(comparer);
}
