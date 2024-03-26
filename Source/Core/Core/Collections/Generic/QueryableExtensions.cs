namespace DotNetToolbox.Collections.Generic;

public static class QueryableExtensions {
    #region Projections

    public static TOutput[] ToArray<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> project)
        => [.. IsNotNull(source).Select(project)];
    public static List<TOutput> ToList<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> project)
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

    #endregion

    #region With Index

    public static IQueryable<Indexed<TItem>> AsIndexed<TItem>(this IQueryable source) => source.Cast<TItem>().AsIndexed();

    public static IQueryable<Indexed<TOutput>> AsIndexed<TItem, TOutput>(this IQueryable source, Expression<Func<TItem, TOutput>> transform) => source.Cast<TItem>().AsIndexed(transform);

    public static IQueryable<Indexed<TItem>> AsIndexed<TItem>(this IQueryable<TItem> source) => source.AsIndexed(i => i);

    public static IQueryable<Indexed<TOutput>> AsIndexed<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> transform) => source.Select((v, i) => new Indexed<TOutput>(i, transform.Compile()(v)));

    public static List<IndexedItem<TItem>> ToIndexedList<TItem>(this IQueryable<TItem> source)
        => source.ToIndexedList(i => i);

    public static List<IndexedItem<TOutput>> ToIndexedList<TItem, TOutput>(this IQueryable<TItem> source, Func<TItem, TOutput> transform) {
        using var enumerator = source.GetEnumerator();
        var list = new List<IndexedItem<TOutput>>();
        var index = 0;
        var hasNext = enumerator.MoveNext();
        while (hasNext) {
            var value = transform(enumerator.Current);
            hasNext = enumerator.MoveNext();
            list.Add(new(index++, value, !hasNext));
        }
        return list;
    }

    #endregion

    #region ForEach

    public static void ForEach<TItem>(this IQueryable source, Action<TItem> action) => source.Cast<TItem>().ToList().ForEach(action);

    #endregion
}

