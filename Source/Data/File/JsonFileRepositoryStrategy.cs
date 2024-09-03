using System.Linq.Expressions;

namespace DotNetToolbox.Data.File;

public class JsonFileRepositoryStrategy<TRepository, TItem, TKey>
    : RepositoryStrategy<JsonFileRepositoryStrategy<TRepository, TItem, TKey>, TRepository, TItem, TKey>
    where TRepository : class, IQueryableRepository<TItem>
    where TItem : class, IEntity<TKey>
    where TKey : notnull {
    private const string _defaultBaseFolder = "data";
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public JsonFileRepositoryStrategy(string name, IConfigurationRoot configuration)
        : base() {
        var baseFolder = configuration.GetValue<string>("Data:BaseFolder")
                      ?? configuration.GetValue<string>($"Data:{name}:BaseFolder")
                      ?? _defaultBaseFolder;
        _filePath = Path.Combine(baseFolder, $"{name}.json");
        EnsureFileExistis(baseFolder);
    }
    private void EnsureFileExistis(string baseFolder) {
        if (Path.Exists(_filePath)) return;
        Directory.CreateDirectory(baseFolder);
        Save();
    }

    public override Result Load() {
        if (Repository.Data.Count != 0) return Result.Success();
        var json = System.IO.File.ReadAllText(_filePath);
        var items = JsonSerializer.Deserialize<TItem[]>(json, _jsonOptions) ?? [];
        Repository.Data.AddRange(items);
        LoadLastUsedKey();
        return Result.Success();
    }

    protected override Result<TKey?> LoadLastUsedKey() {
        LastUsedKey = Repository.Data.Count != 0
            ? Repository.Data.Max(item => item.Key)
            : default;
        return Result.Success(LastUsedKey);
    }

    public override TItem[] GetAll() => [.. Repository.Data];

    public override TItem? FindByKey(TKey key)
        => Repository.Data.Find(item => item.Key.Equals(key));

    public override TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Repository.Data.AsQueryable().FirstOrDefault(predicate);

    private void Save() {
        var json = JsonSerializer.Serialize(Repository?.Data ?? [], _jsonOptions);
        System.IO.File.WriteAllText(_filePath, json);
    }

    public override Result<TItem> Create(Action<TItem>? setItem = null, IContext? validationContext = null) {
        var item = InstanceFactory.Create<TItem>();
        if (TryGetNextKey(out var next)) item.Key = next;
        setItem?.Invoke(item);
        var result = Result.Success(item);
        result += item.Validate(validationContext);
        return result;
    }

    public override Result Add(TItem newItem, IContext? validationContext = null) {
        if (TryGetNextKey(out var next)) newItem.Key = next;
        var result = newItem.Validate(validationContext);
        if (!result.IsSuccess) return result;
        Repository.Data.Add(newItem);
        Save();
        return result;
    }

    public override Result Update(TItem updatedItem, IContext? validationContext = null) {
        var index = Repository.Data.FindIndex(item => item.Key.Equals(updatedItem.Key));
        if (index == -1) return new ValidationError($"Item '{updatedItem.Key}' not found", nameof(updatedItem));
        var result = updatedItem.Validate(validationContext);
        if (!result.IsSuccess) return result;
        Repository.Data[index] = updatedItem;
        Save();
        return result;
    }

    public override Result Remove(TKey key) {
        var index = Repository.Data.FindIndex(item => item.Key.Equals(key));
        if (index == -1) return new ValidationError($"Item '{key}' not found", nameof(key));
        Repository.Data.RemoveAt(index);
        Save();
        return Result.Success();
    }
}
