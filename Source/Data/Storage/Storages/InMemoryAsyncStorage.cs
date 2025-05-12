namespace DotNetToolbox.Data.Storages;

public class InMemoryAsyncStorage<TItem>(string id, IAsyncKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? data = null)
    : InMemoryAsyncStorage<TItem, uint>(id, keyGenerator, data)
    where TItem : IEntity<uint>, new() {
    public InMemoryAsyncStorage(IAsyncKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) { }

    public InMemoryAsyncStorage(string name, IEnumerable<TItem>? seed = null)
        : this(name, InMemoryAsyncKeyGenerator<uint>.Instance, seed) { }

    public InMemoryAsyncStorage(IEnumerable<TItem> seed)
        : this(InMemoryAsyncKeyGenerator<uint>.Instance, seed) { }
    public InMemoryAsyncStorage()
        : this([]) { }
}

public class InMemoryAsyncStorage<TItem, TKey>(string id, IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? data = null)
    : AsyncStorage<InMemoryAsyncStorage<TItem, TKey>, TItem, TKey>(id, keyGenerator, data)
    where TItem : IEntity<TKey>, new()
    where TKey : notnull {
    public InMemoryAsyncStorage(IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) { }
    public InMemoryAsyncStorage(string name, IEnumerable<TItem>? seed = null)
        : this(name, InMemoryAsyncKeyGenerator<TKey>.Instance, seed) { }
    public InMemoryAsyncStorage(IEnumerable<TItem> seed)
        : this(InMemoryAsyncKeyGenerator<TKey>.Instance, seed) { }
    public InMemoryAsyncStorage()
        : this([]) { }

    public override ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => FindAsync(x => x.Id.Equals(key), ct);

    public override async Task<Result<TItem>> CreateAsync(Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default) {
        var item = new TItem { Id = await GetNextKeyAsync(ct) };
        await setItem(item, ct);
        var result = Result.Success(item);
        result += item.Validate(validationContext);
        return result;
    }

    public override async Task<Result> AddAsync(TItem newItem, IMap? validationContext = null, CancellationToken ct = default) {
        newItem.Id = newItem.Id == null! ? await GetNextKeyAsync(ct) : newItem.Id;
        var result = Result.Success();
        result = newItem is IValidatable validatable
                     ? result + validatable.Validate(validationContext)
                     : result;
        if (result.IsSuccessful) Data.Add(newItem);
        return result;
    }

    public override Task<Result> UpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => UpdateAsync(x => x.Id.Equals(updatedItem.Id), updatedItem, validationContext, ct);

    public override Task<Result> PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => PatchAsync(x => x.Id.Equals(key), setItem, validationContext, ct);

    public override Task<Result> RemoveAsync(TKey key, CancellationToken ct = default)
        => RemoveAsync(x => x.Id.Equals(key), ct);

    public override async Task<Result> UpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default) {
        var result = Result.Success();
        await foreach (var updatedItem in updatedItems.AsAsyncEnumerable(ct))
            result += await UpdateAsync(updatedItem, validationContext, ct);
        return result;
    }

    public override async Task<Result> AddOrUpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default) {
        var result = updatedItem.Validate(validationContext);
        if (!result.IsSuccessful) return result;
        result += await RemoveAsync(updatedItem.Id, ct);
        return !result.IsSuccessful
                   ? result
                   : await AddAsync(updatedItem, validationContext, ct);
    }

    public override async Task<Result> AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default) {
        var result = Result.Success();
        await foreach (var item in updatedItems.AsAsyncEnumerable(ct)) result += await AddOrUpdateAsync(item, validationContext, ct);
        return result;
    }

    public override async Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default) {
        var result = Result.Success();
        await foreach (var key in keys.AsAsyncEnumerable(ct)) {
            var item = await FindByKeyAsync(key, ct: ct);
            if (item is null) {
                result += new Error($"Item with key {key} not found.", nameof(keys));
                continue;
            }
            setItem(item);
            result += item.Validate(validationContext);
        }
        return result;
    }
    public override async Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default) {
        var result = Result.Success();
        await foreach (var key in keys.AsAsyncEnumerable(ct)) {
            var item = await FindByKeyAsync(key, ct: ct);
            if (item is null) {
                result += new Error($"Item with key {key} not found.", nameof(keys));
                continue;
            }
            await setItem(item, ct);
            result += item.Validate(validationContext);
        }
        return result;
    }

    public override async Task<Result> RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default) {
        var result = Result.Success();
        await foreach (var key in keys.AsAsyncEnumerable(ct)) {
            var item = await FindByKeyAsync(key, ct: ct);
            if (item is null) {
                result += new Error($"Item with key {key} not found.", nameof(keys));
                continue;
            }
            Data.Remove(item);
        }
        return result;
    }

    public override async Task<Result> SeedAsync(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null, CancellationToken ct = default) {
        var result = Result.Success();
        if (!preserveContent) result += await ClearAsync(ct);
        result += await AddManyAsync(seed, validationContext, ct);
        return result;
    }

    public override Task<Result> LoadAsync(CancellationToken ct = default)
        => Task.FromResult(Result.Success());

    public override ValueTask<TItem[]> GetAllAsync(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default) {
        var query = Data.AsQueryable();
        query = ApplyFilter(query, filterBy);
        query = ApplySorting(query, orderBy);
        return query.ToArrayAsync(ct);
    }

    private static IQueryable<TItem> ApplyFilter(IQueryable<TItem> query, Expression<Func<TItem, bool>>? filterBy = null)
        => filterBy is null ? query : query.Where(filterBy);

    private static IQueryable<TItem> ApplySorting(IQueryable<TItem> query, HashSet<SortClause>? orderBy = null) {
        if (orderBy is null) return query;
        IOrderedQueryable<TItem>? orderedQuery = null;

        foreach (var clause in orderBy) {
            if (typeof(TItem).GetProperty(clause.PropertyName) is null)
                throw new ArgumentException($"Property {clause.PropertyName} not found on {typeof(TItem).Name}.", nameof(orderBy));

            var parameter = Expression.Parameter(typeof(TItem), "x");
            var property = Expression.Property(parameter, clause.PropertyName);
            var lambda = Expression.Lambda<Func<TItem, object>>(property, parameter);
            orderedQuery = orderedQuery is null
                               ? clause.Direction is SortDirection.Ascending
                                     ? query.OrderBy(lambda)
                                     : query.OrderByDescending(lambda)
                               : clause.Direction is SortDirection.Ascending
                                   ? orderedQuery.ThenBy(lambda)
                                   : orderedQuery.ThenByDescending(lambda);
        }
        return orderedQuery ?? query;
    }

    public override async ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default) {
        var count = await Data.AsQueryable().CountAsync(ct);
        var items = await Data.AsQueryable().Skip((int)(pageIndex * pageSize))
                              .Take((int)pageSize)
                              .ToArrayAsync(ct);
        return new() {
                         TotalCount = (uint)count,
                         Index = pageIndex,
                         Size = pageSize,
                         Items = items,
                     };
    }

    public override async ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default) {
        var query = Data.AsQueryable();
        if (isChunkStart is not null) {
            var isNotStart = (Expression<Func<TItem, bool>>)Expression.Lambda(Expression.Not(isChunkStart.Body), isChunkStart.Parameters);
            query = query.SkipWhile(isNotStart);
        }
        var items = await query
                         .Take((int)blockSize)
                         .ToArrayAsync(ct);
        return new() {
                         Size = blockSize,
                         Items = items,
                     };
    }

    public override ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Data.AsQueryable().FirstOrDefaultAsync(predicate, ct);

    public override async Task<Result> AddManyAsync(IEnumerable<TItem> newItems, IMap? validationContext = null, CancellationToken ct = default) {
        var result = Result.Success();
        await foreach (var item in newItems.AsAsyncEnumerable(ct)) {
            var itemResult = Result.Success();
            if (item is IValidatable validatable) itemResult += validatable.Validate(validationContext);
            if (!itemResult.IsSuccessful) {
                result += itemResult;
                continue;
            }
            Data.Add(item);
        }
        return result;
    }

    public override async Task<Result> UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default) {
        var result = await TryRemoveAsync(predicate, ct);
        return !result.IsSuccessful
                   ? result
                   : await AddAsync(updatedItem, validationContext, ct);
    }

    public override async Task<Result> AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default) {
        await RemoveAsync(predicate, ct);
        return await AddAsync(updatedItem, validationContext, ct);
    }
    public override async Task<Result> AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default) {
        await RemoveAsync(predicate, ct);
        return await AddManyAsync(updatedItems, validationContext, ct);
    }

    public override async Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default) {
        var itemToPatch = await Data.AsQueryable().FirstOrDefaultAsync(predicate, ct);
        if (itemToPatch is null) return Result.Failure(new Error("Item not found.", nameof(predicate)));
        setItem(itemToPatch);
        return itemToPatch is IValidatable validatable
                   ? validatable.Validate(validationContext)
                   : Result.Success();
    }
    public override async Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default) {
        var itemToPatch = await Data.AsQueryable().FirstOrDefaultAsync(predicate, ct);
        if (itemToPatch is null) return Result.Failure(new Error("Item not found.", nameof(predicate)));
        await setItem(itemToPatch, ct);
        return itemToPatch is IValidatable validatable
                   ? validatable.Validate(validationContext)
                   : Result.Success();
    }

    public override async Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default) {
        var itemsToPatch = Data.AsQueryable().Where(predicate).AsAsyncEnumerable(ct);
        var result = Result.Success();
        await foreach (var item in itemsToPatch) {
            setItem(item);
            if (item is IValidatable validatable)
                result += validatable.Validate(validationContext);
        }
        return result;
    }
    public override async Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default) {
        var itemsToPatch = Data.AsQueryable().Where(predicate).AsAsyncEnumerable(ct);
        var result = Result.Success();
        await foreach (var item in itemsToPatch) {
            await setItem(item, ct);
            if (item is IValidatable validatable)
                result += validatable.Validate(validationContext);
        }
        return result;
    }

    public override Task<Result> RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => TryRemoveAsync(predicate, ct);
    public override async Task<Result> RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        await foreach (var item in Data.AsQueryable().Where(predicate).AsAsyncEnumerable(ct)) Data.Remove(item);
        return Result.Success();
    }

    public override Task<Result> ClearAsync(CancellationToken ct = default) {
        Data.Clear();
        return Task.FromResult(Result.Success());
    }

    private async Task<Result> TryRemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        var itemToRemove = await Data.AsQueryable().FirstOrDefaultAsync(predicate, ct);
        if (itemToRemove is null)
            return new Error("Item not found.", nameof(predicate));
        Data.Remove(itemToRemove);
        return Result.Success();
    }
}
