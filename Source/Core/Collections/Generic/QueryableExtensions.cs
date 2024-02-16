namespace DotNetToolbox.Collections.Generic;

public static class QueryableExtensions {
    public static TOutput[] ToArray<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> project)
        => [.. IsNotNull(source).Select(project)];

    public static Dictionary<TKey, TValue> ToDictionary<TInput, TKey, TValue>(this IQueryable<TInput> source, Expression<Func<TInput, TInput>> project, Func<TInput, TKey> selectKey, Func<TInput, TValue> selectValue)
        where TKey : notnull
        => ToDictionary<TInput, TInput, TKey, TValue>(source, project, selectKey, selectValue);
    public static Dictionary<TKey, TValue> ToDictionary<TInput, TOutput, TKey, TValue>(this IQueryable<TInput> source, Expression<Func<TInput, TOutput>> project, Func<TOutput, TKey> selectKey, Func<TOutput, TValue> selectValue)
        where TKey : notnull
        => IsNotNull(source).Select(project).ToDictionary(selectKey, selectValue);

    public static HashSet<TItem> ToHashSet<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, TItem>> project)
        => ToHashSet<TItem, TItem>(source, project);

    public static HashSet<TOutput> ToHashSet<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> project)
        => [.. IsNotNull(source).Select(project)];
}
