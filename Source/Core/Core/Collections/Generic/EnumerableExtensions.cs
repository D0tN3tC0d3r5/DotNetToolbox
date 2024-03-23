namespace DotNetToolbox.Collections.Generic;

public static class EnumerableExtensions {

    #region Projections

    public static IEnumerable<TItem> As<TItem>(this IEnumerable source) => source.Cast<TItem>();
    public static IEnumerable<TOutput> As<TItem, TOutput>(this IEnumerable source, Func<TItem, TOutput> transform) => source.Cast<TItem>().Select(transform);

    public static TOutput[] ToArray<TItem, TOutput>(this IEnumerable<TItem> source, Func<TItem, TOutput> transform)
        => [.. IsNotNull(source).Select(transform)];

    public static List<TOutput> ToList<TItem, TOutput>(this IEnumerable<TItem> source, Func<TItem, TOutput> transform)
        => [.. IsNotNull(source).Select(transform)];

    public static Dictionary<TKey, TOutputValue> ToDictionary<TKey, TInputValue, TOutputValue>(this IEnumerable<KeyValuePair<TKey, TInputValue>> source, Func<TInputValue, TOutputValue> transformValue)
        where TKey : notnull
        => IsNotNull(source).ToDictionary(i => i.Key, i => transformValue(i.Value));

    public static Dictionary<TKey, TValue> ToDictionary<TInput, TOutput, TKey, TValue>(this IEnumerable<TInput> source, Func<TInput, TOutput> transformElement, Func<TOutput, TKey> selectKey, Func<TOutput, TValue> selectValue)
        where TKey : notnull
        => IsNotNull(source).Select(transformElement).ToDictionary(selectKey, selectValue);

    public static HashSet<TOutput> ToHashSet<TItem, TOutput>(this IEnumerable<TItem> source, Func<TItem, TOutput> transform)
        => [.. IsNotNull(source).Select(transform)];

    #endregion

    #region With Index

    public static IEnumerable<Indexed<TItem>> AsIndexed<TItem>(this IEnumerable source) => source.Cast<TItem>().AsIndexed();

    public static IEnumerable<Indexed<TOutput>> AsIndexed<TItem, TOutput>(this IEnumerable source, Func<TItem, TOutput> transform) => source.Cast<TItem>().AsIndexed(transform);

    public static IEnumerable<Indexed<TItem>> AsIndexed<TItem>(this IEnumerable<TItem> source) => source.AsIndexed(i => i);

    public static IEnumerable<Indexed<TOutput>> AsIndexed<TItem, TOutput>(this IEnumerable<TItem> source, Func<TItem, TOutput> transform) => source.Select((v, i) => new Indexed<TOutput>(i, transform(v)));

    public static List<IndexedItem<TItem>> ToIndexedList<TItem>(this IEnumerable<TItem> source)
        => source.ToIndexedList(i => i);

    public static List<IndexedItem<TOutput>> ToIndexedList<TItem, TOutput>(this IEnumerable<TItem> source, Func<TItem, TOutput> transform) {
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
    public static void ForEach<TItem>(this IEnumerable source, Action<TItem> action) => source.Cast<TItem>().ToList().ForEach(action);
    public static void ForEach<TItem>(this IEnumerable source, Action<IndexedItem<TItem>> action) => source.Cast<TItem>().ToIndexedList().ForEach(action);
    #endregion
}
