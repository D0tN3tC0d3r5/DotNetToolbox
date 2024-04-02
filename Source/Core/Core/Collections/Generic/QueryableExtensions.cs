namespace DotNetToolbox.Collections.Generic;

public static class QueryableExtensions {
    #region Messages

    private static readonly CompositeFormat _typeIsNotIAsyncEnumerable = CompositeFormat.Parse("The source 'IQueryable' doesn't implement 'IAsyncEnumerable<{0}>'. Only sources that implement 'IAsyncEnumerable' can be used for Entity Framework asynchronous operations.");
    private const string _providerIsNotIAsyncQueryProvider = "The provider for the source 'IQueryable' doesn't implement 'IAsyncQueryProvider'. Only providers that implement 'IAsyncQueryProvider' can be used for Entity Framework asynchronous operations.";

    #endregion

    #region AsAsyncEnumerable

    public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this IQueryable<TSource> source)
        => source as IAsyncEnumerable<TSource>
        ?? throw new InvalidOperationException(string.Format(null, _typeIsNotIAsyncEnumerable, typeof(TSource)));

    #endregion

    #region Load

    public static void Load<TSource>(this IQueryable<TSource> source) {
        using var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext()) {
        }
    }

    public static async Task LoadAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default) {
        var enumerator = source.AsAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
        await using var _ = enumerator.ConfigureAwait(false);

        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
        }
    }

    #endregion

    #region Projections

    public static List<TOutput> ToList<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> project)
        => [.. IsNotNull(source).Select(project)];
    public static List<TItem> ToList<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, TItem>> project)
        => [.. IsNotNull(source).ToList<TItem, TItem>(project)];
    public static async Task<List<TOutput>> ToListAsync<TItem, TOutput>(this IQueryable<TItem> source, Func<TItem, Task<TOutput>> project, CancellationToken cancellationToken = default) {
        var list = new List<TOutput>();
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
            list.Add(await project(element));
        return list;
    }
    public static async Task<List<TOutput>> ToListAsync<TItem, TOutput>(this IQueryable<TItem> source, Func<TItem, TOutput> project, CancellationToken cancellationToken = default) {
        var list = new List<TOutput>();
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
            list.Add(project(element));
        return list;
    }
    public static async Task<List<TItem>> ToListAsync<TItem>(this IQueryable<TItem> source, Func<TItem, Task<TItem>> project, CancellationToken cancellationToken = default) {
        var list = new List<TItem>();
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
            list.Add(await project(element));
        return list;
    }
    public static async Task<List<TItem>> ToListAsync<TItem>(this IQueryable<TItem> source, Func<TItem, TItem> project, CancellationToken cancellationToken = default) {
        var list = new List<TItem>();
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
            list.Add(project(element));
        return list;
    }
    public static async Task<List<TItem>> ToListAsync<TItem>(this IQueryable<TItem> source, CancellationToken cancellationToken = default) {
        var list = new List<TItem>();
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
            list.Add(element);
        return list;
    }
    public static TOutput[] ToArray<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> project)
        => [.. IsNotNull(source).Select(project)];
    public static TItem[] ToArray<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, TItem>> project)
        => [.. IsNotNull(source).Select(project)];
    public static async Task<TOutput[]> ToArrayAsync<TItem, TOutput>(this IQueryable<TItem> source, Func<TItem, Task<TOutput>> project, CancellationToken cancellationToken = default)
        => [.. await source.ToListAsync(project, cancellationToken).ConfigureAwait(false)];
    public static async Task<TOutput[]> ToArrayAsync<TItem, TOutput>(this IQueryable<TItem> source, Func<TItem, TOutput> project, CancellationToken cancellationToken = default)
        => [.. await source.ToListAsync(project, cancellationToken).ConfigureAwait(false)];
    public static async Task<TItem[]> ToArrayAsync<TItem>(this IQueryable<TItem> source, Func<TItem, Task<TItem>> project, CancellationToken cancellationToken = default)
        => [.. await source.ToListAsync<TItem>(project, cancellationToken).ConfigureAwait(false)];
    public static async Task<TItem[]> ToArrayAsync<TItem>(this IQueryable<TItem> source, Func<TItem, TItem> project, CancellationToken cancellationToken = default)
        => [.. await source.ToListAsync<TItem>(project, cancellationToken).ConfigureAwait(false)];
    public static async Task<TItem[]> ToArrayAsync<TItem>(this IQueryable<TItem> source, CancellationToken cancellationToken = default)
        => [.. await source.ToListAsync(cancellationToken).ConfigureAwait(false)];
    public static HashSet<TItem> ToHashSet<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, TItem>> project)
        => ToHashSet<TItem, TItem>(source, project);
    public static HashSet<TOutput> ToHashSet<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> project)
        => [.. IsNotNull(source).ToList(project)];
    public static async Task<HashSet<TOutput>> ToHashSetAsync<TItem, TOutput>(this IQueryable<TItem> source,
                                                                              Func<TItem, Task<TOutput>> project,
                                                                              CancellationToken cancellationToken = default)
        => [.. await IsNotNull(source).ToListAsync(project, cancellationToken).ConfigureAwait(false)];
    public static async Task<HashSet<TOutput>> ToHashSetAsync<TItem, TOutput>(this IQueryable<TItem> source,
                                                                              Func<TItem, TOutput> project,
                                                                              CancellationToken cancellationToken = default)
        => [.. await IsNotNull(source).ToListAsync(project, cancellationToken).ConfigureAwait(false)];
    public static async Task<HashSet<TItem>> ToHashSetAsync<TItem>(this IQueryable<TItem> source,
                                                                              Func<TItem, Task<TItem>> project,
                                                                              CancellationToken cancellationToken = default)
        => [.. await IsNotNull(source).ToListAsync<TItem>(project, cancellationToken).ConfigureAwait(false)];
    public static async Task<HashSet<TItem>> ToHashSetAsync<TItem>(this IQueryable<TItem> source,
                                                                              Func<TItem, TItem> project,
                                                                              CancellationToken cancellationToken = default)
        => [.. await IsNotNull(source).ToListAsync<TItem>(project, cancellationToken).ConfigureAwait(false)];
    public static async Task<HashSet<TItem>> ToHashSetAsync<TItem>(this IQueryable<TItem> source,
                                                                   CancellationToken cancellationToken = default)
        => [.. await IsNotNull(source).ToListAsync(cancellationToken).ConfigureAwait(false)];
    public static Dictionary<TKey, TValue> ToDictionary<TInput, TKey, TValue>(this IQueryable<TInput> source, Expression<Func<TInput, TInput>> project, Func<TInput, TKey> selectKey, Func<TInput, TValue> selectValue)
        where TKey : notnull
        => ToDictionary<TInput, TInput, TKey, TValue>(source, project, selectKey, selectValue);
    public static Dictionary<TKey, TValue> ToDictionary<TInput, TOutput, TKey, TValue>(this IQueryable<TInput> source, Expression<Func<TInput, TOutput>> project, Func<TOutput, TKey> selectKey, Func<TOutput, TValue> selectValue)
        where TKey : notnull
        => IsNotNull(source).Select(project).ToDictionary(selectKey, selectValue);
    public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IQueryable<TSource> source,
                                                                                   Func<TSource, TKey> keySelector,
                                                                                   CancellationToken cancellationToken = default)
        where TKey : notnull
        => ToDictionaryAsync(source, keySelector, e => e, comparer: null, cancellationToken);
    public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IQueryable<TSource> source,
                                                                                   Func<TSource, TKey> keySelector,
                                                                                   IEqualityComparer<TKey> comparer,
                                                                                   CancellationToken cancellationToken = default)
        where TKey : notnull
        => ToDictionaryAsync(source, keySelector, e => e, comparer, cancellationToken);
    public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IQueryable<TSource> source,
                                                                                              Func<TSource, TKey> keySelector,
                                                                                              Func<TSource, TElement> elementSelector,
                                                                                              CancellationToken cancellationToken = default)
        where TKey : notnull
        => ToDictionaryAsync(source, keySelector, elementSelector, comparer: null, cancellationToken);
    public static async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IQueryable<TSource> source,
                                                                                                    Func<TSource, TKey> keySelector,
                                                                                                    Func<TSource, TElement> elementSelector,
                                                                                                    IEqualityComparer<TKey>? comparer,
                                                                                                    CancellationToken cancellationToken = default)
        where TKey : notnull {
        IsNotNull(keySelector, nameof(keySelector));
        IsNotNull(elementSelector, nameof(elementSelector));

        var d = new Dictionary<TKey, TElement>(comparer);
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken)) {
            d.Add(keySelector(element), elementSelector(element));
        }

        return d;
    }
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

    public static async Task<List<IndexedItem<TOutput>>> ToIndexedListAsync<TItem, TOutput>(this IQueryable<TItem> source,
                                                                                Func<TItem, Task<TOutput>> transform,
                                                                                CancellationToken cancellationToken = default) {
        await using var enumerator = source.AsAsyncEnumerable().WithCancellation(cancellationToken).GetAsyncEnumerator();
        var list = new List<IndexedItem<TOutput>>();
        var index = 0;
        var hasNext = await enumerator.MoveNextAsync();
        while (hasNext) {
            var value = await transform(enumerator.Current);
            hasNext = await enumerator.MoveNextAsync();
            list.Add(new(index++, value, !hasNext));
        }
        return list;
    }
    public static async Task<List<IndexedItem<TOutput>>> ToIndexedListAsync<TItem, TOutput>(this IQueryable<TItem> source,
                                                                                Func<TItem, TOutput> transform,
                                                                                CancellationToken cancellationToken = default) {
        await using var enumerator = source.AsAsyncEnumerable().WithCancellation(cancellationToken).GetAsyncEnumerator();
        var list = new List<IndexedItem<TOutput>>();
        var index = 0;
        var hasNext = await enumerator.MoveNextAsync();
        while (hasNext) {
            var value = transform(enumerator.Current);
            hasNext = await enumerator.MoveNextAsync();
            list.Add(new(index++, value, !hasNext));
        }
        return list;
    }
    public static async Task<List<IndexedItem<TItem>>> ToIndexedListAsync<TItem>(this IQueryable<TItem> source,
                                                                                Func<TItem, Task<TItem>> transform,
                                                                                CancellationToken cancellationToken = default) {
        await using var enumerator = source.AsAsyncEnumerable().WithCancellation(cancellationToken).GetAsyncEnumerator();
        var list = new List<IndexedItem<TItem>>();
        var index = 0;
        var hasNext = await enumerator.MoveNextAsync();
        while (hasNext) {
            var value = await transform(enumerator.Current);
            hasNext = await enumerator.MoveNextAsync();
            list.Add(new(index++, value, !hasNext));
        }
        return list;
    }
    public static async Task<List<IndexedItem<TItem>>> ToIndexedListAsync<TItem>(this IQueryable<TItem> source,
                                                                                Func<TItem, TItem> transform,
                                                                                CancellationToken cancellationToken = default) {
        await using var enumerator = source.AsAsyncEnumerable().WithCancellation(cancellationToken).GetAsyncEnumerator();
        var list = new List<IndexedItem<TItem>>();
        var index = 0;
        var hasNext = await enumerator.MoveNextAsync();
        while (hasNext) {
            var value = transform(enumerator.Current);
            hasNext = await enumerator.MoveNextAsync();
            list.Add(new(index++, value, !hasNext));
        }
        return list;
    }
    public static Task<List<IndexedItem<TItem>>> ToIndexedListAsync<TItem>(this IQueryable<TItem> source)
        => source.ToIndexedListAsync<TItem>(i => i);

    #endregion

    #region With Index

    public static IQueryable<Indexed<TItem>> AsIndexed<TItem>(this IQueryable source) => source.Cast<TItem>().AsIndexed();

    public static IQueryable<Indexed<TOutput>> AsIndexed<TItem, TOutput>(this IQueryable source, Expression<Func<TItem, TOutput>> transform) => source.Cast<TItem>().AsIndexed(transform);

    public static IQueryable<Indexed<TItem>> AsIndexed<TItem>(this IQueryable<TItem> source) => source.AsIndexed(i => i);

    public static IQueryable<Indexed<TOutput>> AsIndexed<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> transform) => source.Select((v, i) => new Indexed<TOutput>(i, transform.Compile()(v)));

    #endregion

    #region Any/All

    public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<bool>>(QueryableMethods.AnyWithoutPredicate, source, cancellationToken);

    public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate, nameof(predicate));
        return ExecuteAsync<TSource, Task<bool>>(QueryableMethods.AnyWithPredicate, source, predicate, cancellationToken);
    }

    public static Task<bool> AllAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate, nameof(predicate));
        return ExecuteAsync<TSource, Task<bool>>(QueryableMethods.All, source, predicate, cancellationToken);
    }

    #endregion

    #region Count/LongCount

    public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<int>>(QueryableMethods.CountWithoutPredicate, source, cancellationToken);

    public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate, nameof(predicate));
        return ExecuteAsync<TSource, Task<int>>(QueryableMethods.CountWithPredicate, source, predicate, cancellationToken);
    }

    public static Task<long> LongCountAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<long>>(QueryableMethods.LongCountWithoutPredicate, source, cancellationToken);

    public static Task<long> LongCountAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate, nameof(predicate));
        return ExecuteAsync<TSource, Task<long>>(QueryableMethods.LongCountWithPredicate, source, predicate, cancellationToken);
    }

    #endregion

    #region ElementAt/ElementAtOrDefault

    public static Task<TSource> ElementAtAsync<TSource>(this IQueryable<TSource> source, int index, CancellationToken cancellationToken = default) {
        IsNotNull(index, nameof(index));
        return ExecuteAsync<TSource, Task<TSource>>(
            QueryableMethods.ElementAt, source, Expression.Constant(index), cancellationToken);
    }

    public static Task<TSource> ElementAtOrDefaultAsync<TSource>(this IQueryable<TSource> source, int index, CancellationToken cancellationToken = default) {
        IsNotNull(index, nameof(index));
        return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.ElementAtOrDefault, source, Expression.Constant(index), cancellationToken);
    }

    #endregion

    #region First/FirstOrDefault

    public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.FirstWithoutPredicate, source, cancellationToken);

    public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate, nameof(predicate));
        return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.FirstWithPredicate, source, predicate, cancellationToken);
    }

    public static Task<TSource?> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<TSource?>>(QueryableMethods.FirstOrDefaultWithoutPredicate, source, cancellationToken);

    public static Task<TSource?> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate, nameof(predicate));
        return ExecuteAsync<TSource, Task<TSource?>>(QueryableMethods.FirstOrDefaultWithPredicate, source, predicate, cancellationToken);
    }

    #endregion

    #region Last/LastOrDefault

    public static Task<TSource> LastAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.LastWithoutPredicate, source, cancellationToken);

    public static Task<TSource> LastAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate, nameof(predicate));
        return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.LastWithPredicate, source, predicate, cancellationToken);
    }

    public static Task<TSource?> LastOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<TSource?>>(QueryableMethods.LastOrDefaultWithoutPredicate, source, cancellationToken);

    public static Task<TSource?> LastOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate, nameof(predicate));
        return ExecuteAsync<TSource, Task<TSource?>>(QueryableMethods.LastOrDefaultWithPredicate, source, predicate, cancellationToken);
    }

    #endregion

    #region Single/SingleOrDefault

    public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.SingleWithoutPredicate, source, cancellationToken);

    public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate, nameof(predicate));
        return ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.SingleWithPredicate, source, predicate, cancellationToken);
    }

    public static Task<TSource?> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<TSource?>>(QueryableMethods.SingleOrDefaultWithoutPredicate, source, cancellationToken);

    public static Task<TSource?> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate, nameof(predicate));
        return ExecuteAsync<TSource, Task<TSource?>>(QueryableMethods.SingleOrDefaultWithPredicate, source, predicate, cancellationToken);
    }

    #endregion

    #region Min

    public static Task<TSource> MinAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.MinWithoutSelector, source, cancellationToken);

    public static Task<TResult> MinAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<TResult>>(QueryableMethods.MinWithSelector, source, selector, cancellationToken);
    }

    #endregion

    #region Max

    public static Task<TSource> MaxAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<TSource>>(QueryableMethods.MaxWithoutSelector, source, cancellationToken);

    public static Task<TResult> MaxAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<TResult>>(QueryableMethods.MaxWithSelector, source, selector, cancellationToken);
    }

    #endregion

    #region Sum

    public static Task<decimal> SumAsync(this IQueryable<decimal> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<decimal, Task<decimal>>(QueryableMethods.GetSumWithoutSelector(typeof(decimal)), source, cancellationToken);

    public static Task<decimal?> SumAsync(this IQueryable<decimal?> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<decimal?, Task<decimal?>>(QueryableMethods.GetSumWithoutSelector(typeof(decimal?)), source, cancellationToken);

    public static Task<decimal> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<decimal>>(QueryableMethods.GetSumWithSelector(typeof(decimal)), source, selector, cancellationToken);
    }

    public static Task<decimal?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<decimal?>>(QueryableMethods.GetSumWithSelector(typeof(decimal?)), source, selector, cancellationToken);
    }

    public static Task<int> SumAsync(this IQueryable<int> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<int, Task<int>>(QueryableMethods.GetSumWithoutSelector(typeof(int)), source, cancellationToken);

    public static Task<int?> SumAsync(this IQueryable<int?> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<int?, Task<int?>>(QueryableMethods.GetSumWithoutSelector(typeof(int?)), source, cancellationToken);

    public static Task<int> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<int>>(QueryableMethods.GetSumWithSelector(typeof(int)), source, selector, cancellationToken);
    }

    public static Task<int?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<int?>>(QueryableMethods.GetSumWithSelector(typeof(int?)), source, selector, cancellationToken);
    }

    public static Task<long> SumAsync(this IQueryable<long> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<long, Task<long>>(QueryableMethods.GetSumWithoutSelector(typeof(long)), source, cancellationToken);

    public static Task<long?> SumAsync(this IQueryable<long?> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<long?, Task<long?>>(QueryableMethods.GetSumWithoutSelector(typeof(long?)), source, cancellationToken);

    public static Task<long> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<long>>(QueryableMethods.GetSumWithSelector(typeof(long)), source, selector, cancellationToken);
    }

    public static Task<long?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<long?>>(QueryableMethods.GetSumWithSelector(typeof(long?)), source, selector, cancellationToken);
    }

    public static Task<double> SumAsync(this IQueryable<double> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<double, Task<double>>(QueryableMethods.GetSumWithoutSelector(typeof(double)), source, cancellationToken);

    public static Task<double?> SumAsync(this IQueryable<double?> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<double?, Task<double?>>(QueryableMethods.GetSumWithoutSelector(typeof(double?)), source, cancellationToken);

    public static Task<double> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<double>>(QueryableMethods.GetSumWithSelector(typeof(double)), source, selector, cancellationToken);
    }

    public static Task<double?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<double?>>(QueryableMethods.GetSumWithSelector(typeof(double?)), source, selector, cancellationToken);
    }

    public static Task<float> SumAsync(this IQueryable<float> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<float, Task<float>>(QueryableMethods.GetSumWithoutSelector(typeof(float)), source, cancellationToken);

    public static Task<float?> SumAsync(this IQueryable<float?> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<float?, Task<float?>>(QueryableMethods.GetSumWithoutSelector(typeof(float?)), source, cancellationToken);

    public static Task<float> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<float>>(QueryableMethods.GetSumWithSelector(typeof(float)), source, selector, cancellationToken);
    }

    public static Task<float?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<float?>>(QueryableMethods.GetSumWithSelector(typeof(float?)), source, selector, cancellationToken);
    }

    #endregion

    #region Average

    public static Task<decimal> AverageAsync(this IQueryable<decimal> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<decimal, Task<decimal>>(QueryableMethods.GetAverageWithoutSelector(typeof(decimal)), source, cancellationToken);

    public static Task<decimal?> AverageAsync(this IQueryable<decimal?> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<decimal?, Task<decimal?>>(QueryableMethods.GetAverageWithoutSelector(typeof(decimal?)), source, cancellationToken);

    public static Task<decimal> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<decimal>>(QueryableMethods.GetAverageWithSelector(typeof(decimal)), source, selector, cancellationToken);
    }

    public static Task<decimal?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<decimal?>>(QueryableMethods.GetAverageWithSelector(typeof(decimal?)), source, selector, cancellationToken);
    }

    public static Task<double> AverageAsync(this IQueryable<int> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<int, Task<double>>(QueryableMethods.GetAverageWithoutSelector(typeof(int)), source, cancellationToken);

    public static Task<double?> AverageAsync(this IQueryable<int?> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<int?, Task<double?>>(QueryableMethods.GetAverageWithoutSelector(typeof(int?)), source, cancellationToken);

    public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<double>>(QueryableMethods.GetAverageWithSelector(typeof(int)), source, selector, cancellationToken);
    }

    public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<double?>>(QueryableMethods.GetAverageWithSelector(typeof(int?)), source, selector, cancellationToken);
    }

    public static Task<double> AverageAsync(this IQueryable<long> source,
                                            CancellationToken cancellationToken = default)
        => ExecuteAsync<long, Task<double>>(QueryableMethods.GetAverageWithoutSelector(typeof(long)), source, cancellationToken);

    public static Task<double?> AverageAsync(
        this IQueryable<long?> source,
        CancellationToken cancellationToken = default)
        => ExecuteAsync<long?, Task<double?>>(QueryableMethods.GetAverageWithoutSelector(typeof(long?)), source, cancellationToken);

    public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<double>>(QueryableMethods.GetAverageWithSelector(typeof(long)), source, selector, cancellationToken);
    }

    public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<double?>>(QueryableMethods.GetAverageWithSelector(typeof(long?)), source, selector, cancellationToken);
    }

    public static Task<double> AverageAsync(this IQueryable<double> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<double, Task<double>>(QueryableMethods.GetAverageWithoutSelector(typeof(double)), source, cancellationToken);

    public static Task<double?> AverageAsync(this IQueryable<double?> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<double?, Task<double?>>(QueryableMethods.GetAverageWithoutSelector(typeof(double?)), source, cancellationToken);

    public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<double>>(QueryableMethods.GetAverageWithSelector(typeof(double)), source, selector, cancellationToken);
    }

    public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<double?>>(QueryableMethods.GetAverageWithSelector(typeof(double?)), source, selector, cancellationToken);
    }

    public static Task<float> AverageAsync(this IQueryable<float> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<float, Task<float>>(QueryableMethods.GetAverageWithoutSelector(typeof(float)), source, cancellationToken);

    public static Task<float?> AverageAsync(this IQueryable<float?> source, CancellationToken cancellationToken = default)
        => ExecuteAsync<float?, Task<float?>>(QueryableMethods.GetAverageWithoutSelector(typeof(float?)), source, cancellationToken);

    public static Task<float> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<float>>(QueryableMethods.GetAverageWithSelector(typeof(float)), source, selector, cancellationToken);
    }

    public static Task<float?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default) {
        IsNotNull(selector, nameof(selector));
        return ExecuteAsync<TSource, Task<float?>>(QueryableMethods.GetAverageWithSelector(typeof(float?)), source, selector, cancellationToken);
    }

    #endregion

    #region Contains

    public static Task<bool> ContainsAsync<TSource>(this IQueryable<TSource> source, TSource item, CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, Task<bool>>(QueryableMethods.Contains, source, Expression.Constant(item, typeof(TSource)), cancellationToken);

    #endregion

    #region ForEach

    public static void ForEach<T>(this IQueryable source, Action<T> action) {
        IsNotNull(action, nameof(action));
        foreach (var element in source.Cast<T>())
            action(element);
    }

    public static async Task ForEachAsync<T>(this IQueryable source, Action<T> action, CancellationToken cancellationToken = default) {
        IsNotNull(action, nameof(action));
        await foreach (var element in source.Cast<T>().AsAsyncEnumerable().WithCancellation(cancellationToken))
            action(element);
    }

    #endregion

    #region Include

    internal static readonly MethodInfo IncludeMethodInfo
        = typeof(QueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(Include))
            .Single(mi => mi.GetGenericArguments().Length == 2
                       && mi.GetParameters().Any(pi => pi.Name == "navigationPropertyPath" && pi.ParameterType != typeof(string)));

    internal static readonly MethodInfo NotQuiteIncludeMethodInfo
        = typeof(QueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(NotQuiteInclude))
            .Single(mi => mi.GetGenericArguments().Length == 2
                       && mi.GetParameters().Any(pi => pi.Name == "navigationPropertyPath" && pi.ParameterType != typeof(string)));

    public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        where TEntity : class {
        IsNotNull(navigationPropertyPath, nameof(navigationPropertyPath));
        var callExpression = Expression.Call(instance: null,
                                             method: IncludeMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TProperty)),
                                             arguments: [source.Expression, Expression.Quote(navigationPropertyPath)]);
        var queryable = source.Provider is IAsyncQueryProvider
            ? source.Provider.CreateQuery<TEntity>(callExpression)
            : source;
        return new IncludableQueryable<TEntity, TProperty>(queryable);
    }

    internal static IIncludableQueryable<TEntity, TProperty> NotQuiteInclude<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        where TEntity : class {
        var callExpression = Expression.Call(instance: null,
                                             method: NotQuiteIncludeMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TProperty)),
                                             arguments: [source.Expression, Expression.Quote(navigationPropertyPath)]);
        var queryable = source.Provider is IAsyncQueryProvider
            ? source.Provider.CreateQuery<TEntity>(callExpression)
            : source;
        return new IncludableQueryable<TEntity, TProperty>(queryable);
    }

    internal static readonly MethodInfo ThenIncludeAfterEnumerableMethodInfo
        = typeof(QueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(ThenInclude))
            .Where(mi => mi.GetGenericArguments().Length == 3)
            .Single(mi => {
                var typeInfo = mi.GetParameters()[0].ParameterType.GenericTypeArguments[1];
                return typeInfo.IsGenericType
                    && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);
            });

    internal static readonly MethodInfo ThenIncludeAfterReferenceMethodInfo
        = typeof(QueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(ThenInclude))
            .Single(mi => mi.GetGenericArguments().Length == 3
                       && mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);

    public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
        where TEntity : class {
        var methodCallExpression = Expression.Call(instance: null,
                                                   method: ThenIncludeAfterEnumerableMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty)),
                                                   arguments: [source.Expression, Expression.Quote(navigationPropertyPath)]);
        var queryable = source.Provider is IAsyncQueryProvider
            ? source.Provider.CreateQuery<TEntity>(methodCallExpression)
            : source;
        return new IncludableQueryable<TEntity, TProperty>(queryable);
    }

    public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, TPreviousProperty> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
        where TEntity : class {
        var methodCallExpression = Expression.Call(instance: null,
                                                   method: ThenIncludeAfterReferenceMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty)),
                                                   arguments: [source.Expression, Expression.Quote(navigationPropertyPath)]);
        var queryable = source.Provider is IAsyncQueryProvider
            ? source.Provider.CreateQuery<TEntity>(methodCallExpression)
            : source;
        return new IncludableQueryable<TEntity, TProperty>(queryable);
    }

    private sealed class IncludableQueryable<TEntity, TProperty>(IQueryable<TEntity> queryable)
        : IIncludableQueryable<TEntity, TProperty>, IAsyncEnumerable<TEntity> {
        public Expression Expression => queryable.Expression;
        public Type ElementType => queryable.ElementType;
        public IQueryProvider Provider => queryable.Provider;

        public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => ((IAsyncEnumerable<TEntity>)queryable).GetAsyncEnumerator(cancellationToken);
        public IEnumerator<TEntity> GetEnumerator() => queryable.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal static readonly MethodInfo StringIncludeMethodInfo
        = typeof(QueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(Include))
            .Single(mi => mi.GetParameters()
                            .Any(pi => pi.Name == "navigationPropertyPath" && pi.ParameterType == typeof(string)));

    public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> source, string navigationPropertyPath)
        where TEntity : class {
        IsNotEmpty(navigationPropertyPath, nameof(navigationPropertyPath));
        var methodCallExpression = Expression.Call(instance: null,
                                                   method: StringIncludeMethodInfo.MakeGenericMethod(typeof(TEntity)),
                                                   arg0: source.Expression,
                                                   arg1: Expression.Constant(navigationPropertyPath));
        return source.Provider is IAsyncQueryProvider
            ? source.Provider.CreateQuery<TEntity>(methodCallExpression)
            : source;
    }

    #endregion

    #region Impl.

    private static TResult ExecuteAsync<TSource, TResult>(MethodInfo operatorMethodInfo,
                                                          IQueryable source,
                                                          Expression? expression,
                                                          CancellationToken cancellationToken = default) {
        if (source.Provider is not IAsyncQueryProvider provider) throw new InvalidOperationException(_providerIsNotIAsyncQueryProvider);
        if (operatorMethodInfo.IsGenericMethod) {
            operatorMethodInfo = operatorMethodInfo.GetGenericArguments().Length == 2
                ? operatorMethodInfo.MakeGenericMethod(typeof(TSource), typeof(TResult).GetGenericArguments().Single())
                : operatorMethodInfo.MakeGenericMethod(typeof(TSource));
        }

        Expression[] arguments = expression == null ? [source.Expression] : [source.Expression, expression];
        var methodCallExpression = Expression.Call(null, operatorMethodInfo, arguments);
        return provider.ExecuteAsync<TResult>(methodCallExpression, cancellationToken);
    }

    private static TResult ExecuteAsync<TSource, TResult>(MethodInfo operatorMethodInfo,
                                                          IQueryable source,
                                                          LambdaExpression expression,
                                                          CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, TResult>(operatorMethodInfo, source, Expression.Quote(expression), cancellationToken);

    private static TResult ExecuteAsync<TSource, TResult>(MethodInfo operatorMethodInfo,
                                                          IQueryable source,
                                                          CancellationToken cancellationToken = default)
        => ExecuteAsync<TSource, TResult>(operatorMethodInfo, source, (Expression?)null, cancellationToken);

    #endregion
}
