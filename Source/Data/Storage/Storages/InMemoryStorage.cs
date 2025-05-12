namespace DotNetToolbox.Data.Storages;

public class InMemoryStorage<TItem>(string id, IKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
    : InMemoryStorage<TItem, uint>(id, keyGenerator, seed)
    where TItem : IEntity<uint>, new() {
    public InMemoryStorage(IKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) { }
    public InMemoryStorage(string name, IEnumerable<TItem>? seed = null)
        : this(name, InMemoryKeyGenerator<uint>.Instance, seed) { }
    public InMemoryStorage(IEnumerable<TItem> seed)
        : this(InMemoryKeyGenerator<uint>.Instance, seed) { }
    public InMemoryStorage()
        : this([]) { }
}

public class InMemoryStorage<TItem, TKey>(string id, IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
    : Storage<InMemoryStorage<TItem, TKey>, TItem, TKey>(id, keyGenerator, seed)
    where TItem : IEntity<TKey>, new()
    where TKey : notnull {
    public InMemoryStorage(IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) { }
    public InMemoryStorage(string name, IEnumerable<TItem>? seed = null)
        : this(name, InMemoryKeyGenerator<TKey>.Instance, seed) { }
    public InMemoryStorage(IEnumerable<TItem> seed)
        : this(InMemoryKeyGenerator<TKey>.Instance, seed) { }
    public InMemoryStorage()
        : this([]) { }

    public override TItem? FindByKey(TKey key)
        => Find(x => x.Id.Equals(key));

    public override Result<TItem> Create(Action<TItem>? setItem = null, IMap? validationContext = null) {
        var item = new TItem { Id = GetNextKey() };
        setItem?.Invoke(item);
        var result = Result.Success(item);
        result += Add(item, validationContext);
        return result;
    }

    public override Result Add(TItem newItem, IMap? validationContext = null) {
        newItem.Id = newItem.Id == null! ? GetNextKey() : newItem.Id;
        var result = Result.Success();
        result = newItem is IValidatable validatable
                     ? result + validatable.Validate(validationContext)
                     : result;
        if (result.IsSuccessful) Data.Add(newItem);
        return result;
    }

    public override Result Update(TItem updatedItem, IMap? validationContext = null)
        => Update(x => x.Id.Equals(updatedItem.Id), updatedItem, validationContext);

    public override Result Patch(TKey key, Action<TItem> setItem, IMap? validationContext = null)
        => Patch(x => x.Id.Equals(key), setItem, validationContext);

    public override Result Remove(TKey key)
        => Remove(x => x.Id.Equals(key));

    public override Result UpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null) {
        var result = Result.Success();
        foreach (var updatedItem in updatedItems)
            result += Update(updatedItem, validationContext);
        return result;
    }

    public override Result AddOrUpdate(TItem updatedItem, IMap? validationContext = null) {
        var result = updatedItem.Validate(validationContext);
        if (!result.IsSuccessful) return result;
        result += Remove(updatedItem.Id);
        return !result.IsSuccessful
                   ? result
                   : Add(updatedItem, validationContext);
    }

    public override Result AddOrUpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null) {
        var result = Result.Success();
        foreach (var item in updatedItems)
            result += AddOrUpdate(item, validationContext);
        return result;
    }

    public override Result PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null) {
        var result = Result.Success();
        foreach (var key in keys) {
            var item = FindByKey(key);
            if (item is null) {
                result += new Error($"Item with key {key} not found.", nameof(keys));
                continue;
            }
            setItem(item);
            result += item.Validate(validationContext);
        }
        return result;
    }

    public override Result RemoveMany(IEnumerable<TKey> keys) {
        var result = Result.Success();
        foreach (var key in keys)
            result += Remove(key);
        return result;
    }

    public override Result Seed(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null) {
        var result = Result.Success();
        if (!preserveContent) result += Clear();
        result += AddMany(seed, validationContext);
        return result;
    }

    public override Result Load() => Result.Success();

    public override TItem[] GetAll(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null)
        => [.. Data];

    public override Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null) {
        var count = Data.Count;
        var items = Data.Skip((int)(pageIndex * pageSize))
                   .Take((int)pageSize)
                   .ToArray();
        return new() {
            TotalCount = (uint)count,
            Index = pageIndex,
            Size = pageSize,
            Items = items,
        };
    }

    public override Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null) {
        var query = Data.AsQueryable();
        if (isChunkStart is not null) {
            var isNotStart = (Expression<Func<TItem, bool>>)Expression.Lambda(Expression.Not(isChunkStart.Body), isChunkStart.Parameters);
            query = query.SkipWhile(isNotStart);
        }
        var items = query
                   .Take((int)blockSize)
                   .ToArray();
        return new() {
            Size = blockSize,
            Items = items,
        };
    }

    public override TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Data.AsQueryable().FirstOrDefault(predicate);
    public override Result AddMany(IEnumerable<TItem> newItems, IMap? validationContext = null) {
        var result = Result.Success();
        var validItems = new List<TItem>();
        foreach (var newItem in newItems) {
            var itemResult = Result.Success();
            if (newItem is IValidatable validatable) itemResult += validatable.Validate(validationContext);
            if (!itemResult.IsSuccessful) {
                result += itemResult;
                continue;
            }
            validItems.Add(newItem);
        }

        Data.AddRange(validItems);
        return result;
    }

    public override Result Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null) {
        var result = TryRemove(predicate);
        return !result.IsSuccessful
            ? result
            : Add(updatedItem, validationContext);
    }

    public override Result UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null) {
        var result = TryRemove(predicate);
        return !result.IsSuccessful
                   ? result
                   : AddMany(updatedItems, validationContext);
    }

    public override Result AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null) {
        Remove(predicate);
        return Add(updatedItem, validationContext);
    }
    public override Result AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items, IMap? validationContext = null) {
        Remove(predicate);
        return AddMany(items, validationContext);
    }

    public override Result Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null) {
        var itemToPatch = Data.AsQueryable().FirstOrDefault(predicate);
        if (itemToPatch is null) return Result.Failure(new Error("Item not found.", nameof(predicate)));
        setItem(itemToPatch);
        return itemToPatch is IValidatable validatable
                   ? validatable.Validate(validationContext)
                   : Result.Success();
    }
    public override Result PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null) {
        var itemsToPatch = Data.AsQueryable().Where(predicate);
        var result = Result.Success();
        foreach (var item in itemsToPatch) {
            setItem(item);
            if (item is IValidatable validatable)
                result += validatable.Validate(validationContext);
        }
        return result;
    }

    public override Result Remove(Expression<Func<TItem, bool>> predicate)
        => TryRemove(predicate);
    public override Result RemoveMany(Expression<Func<TItem, bool>> predicate) {
        var itemsToRemove = Data.AsQueryable().Where(predicate);
        foreach (var item in itemsToRemove.ToArray()) Data.Remove(item);
        return Result.Success();
    }

    public override Result Clear() {
        Data.Clear();
        return Result.Success();
    }

    private Result TryRemove(Expression<Func<TItem, bool>> predicate) {
        var itemToRemove = Data.AsQueryable().FirstOrDefault(predicate);
        if (itemToRemove is null)
            return new Error("Item not found.", nameof(predicate));
        Data.Remove(itemToRemove);
        return Result.Success();
    }
}
