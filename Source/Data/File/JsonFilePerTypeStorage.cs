namespace DotNetToolbox.Data.File;

public abstract class JsonFilePerTypeStorage<TItem, TKey>
    : Storage<JsonFilePerTypeStorage<TItem, TKey>, TItem, TKey>
    where TItem : class, IEntity<TKey>
    where TKey : notnull {
    private const string _defaultBaseFolder = "data";
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    protected JsonFilePerTypeStorage(string name, IConfiguration configuration, IList<TItem>? data = null)
        : base(data) {
        var baseFolder = configuration.GetValue<string>("Data:BaseFolder")
                      ?? configuration.GetValue<string>($"Data:{name}:BaseFolder")
                      ?? _defaultBaseFolder;
        _filePath = Path.Combine(baseFolder, $"{name}.json");
        EnsureFileExists(baseFolder);
    }
    private void EnsureFileExists(string baseFolder) {
        if (Path.Exists(_filePath)) return;
        Directory.CreateDirectory(baseFolder);
        Save();
    }

    public override Result Load() {
        if (Data.Count != 0) return Result.Success();
        var json = System.IO.File.ReadAllText(_filePath);
        var items = JsonSerializer.Deserialize<TItem[]>(json, _jsonOptions) ?? [];
        foreach (var item in items) Data.Add(item);
        LoadLastUsedKey();
        return Result.Success();
    }

    protected override Result<TKey?> LoadLastUsedKey() {
        LastUsedKey = Data.Count != 0
            ? Data.Max(item => item.Key)
            : default;
        return Result.Success(LastUsedKey);
    }

    public override TItem[] GetAll(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null) {
        var query = Data.AsQueryable();
        query = ApplyFilter(query, filterBy);
        query = ApplySorting(query, orderBy);
        return [.. query];
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

    public override TItem? FindByKey(TKey key)
        => Data.FirstOrDefault(item => item.Key.Equals(key));

    public override TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Data.AsQueryable().FirstOrDefault(predicate);

    private void Save() {
        var json = JsonSerializer.Serialize(Data, _jsonOptions);
        System.IO.File.WriteAllText(_filePath, json);
    }

    public override Result<TItem> Create(Action<TItem>? setItem = null, IMap? validationContext = null) {
        var item = InstanceFactory.Create<TItem>();
        if (TryGetNextKey(out var next)) item.Key = next;
        setItem?.Invoke(item);
        var result = Result.Success(item);
        result += item.Validate(validationContext);
        return result;
    }

    public override Result Add(TItem newItem, IMap? validationContext = null) {
        if (TryGetNextKey(out var next)) newItem.Key = next;
        var result = newItem.Validate(validationContext);
        if (!result.IsSuccess) return result;
        Data.Add(newItem);
        Save();
        return result;
    }

    public override Result Update(TItem updatedItem, IMap? validationContext = null) {
        var entry = Data.Index().FirstOrDefault(i => i.Item.Key.Equals(updatedItem.Key));
        if (entry.Item is null) return new ValidationError($"Item '{updatedItem.Key}' not found", nameof(updatedItem));
        var result = updatedItem.Validate(validationContext);
        if (!result.IsSuccess) return result;
        Data[entry.Index] = updatedItem;
        Save();
        return result;
    }

    public override Result Remove(TKey key) {
        var entry = Data.Index().FirstOrDefault(i => i.Item.Key.Equals(key));
        if (entry.Item is null) return new ValidationError($"Item '{key}' not found", nameof(key));
        Data.RemoveAt(entry.Index);
        Save();
        return Result.Success();
    }
}
