namespace DotNetToolbox.Data.File;

public class JsonFileRepositoryStrategy<TRepository, TItem, TKey, TSequencer>
    : RepositoryStrategy<JsonFileRepositoryStrategy<TRepository, TItem, TKey, TSequencer>, TRepository, TItem, TKey>
    where TRepository : class, IQueryableRepository<TItem>
    where TItem : class, IEntity<TKey>
    where TKey : notnull
    where TSequencer : class, ISequencer<TKey> {
    private const string _defaultBaseFolder = "data";
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
    private readonly List<TItem> _data = [];

    public JsonFileRepositoryStrategy(string name, IConfigurationRoot configuration, Lazy<TRepository> repository)
        : base(repository) {
        var baseFolder = configuration.GetValue<string>("Data:BaseFolder")
                      ?? configuration.GetValue<string>($"Data:{name}:BaseFolder")
                      ?? _defaultBaseFolder;
        _filePath = Path.Combine(baseFolder, $"{name}.json");
        EnsureFileExistis(baseFolder);
        Load();
    }
    private void EnsureFileExistis(string baseFolder) {
        if (Path.Exists(_filePath)) return;
        Directory.CreateDirectory(baseFolder);
        Save();
    }

    public override Result Load() {
        var json = System.IO.File.ReadAllText(_filePath);
        var items = JsonSerializer.Deserialize<TItem[]>(json, _jsonOptions) ?? [];
        _data.Clear();
        _data.AddRange(items);
        LoadLastUsedKey();
        return Result.Success();
    }

    protected override Result<TKey?> LoadLastUsedKey() {
        LastUsedKey = _data.Count != 0
            ? _data.Max(item => item.Key)
            : default;
        return Result.Success(LastUsedKey);
    }

    public override Result<TItem> Create(Action<TItem> setItem, IContext? validationContext = null) {
        var item = InstanceFactory.Create<TItem>();
        setItem(item);
        return Result.Success(item);
    }

    public override TItem[] GetAll() => [.. _data];

    public override TItem? FindByKey(TKey key)
        => _data.Find(item => item.Key.Equals(key));

    private void Save() {
        var json = JsonSerializer.Serialize(_data, _jsonOptions);
        System.IO.File.WriteAllText(_filePath, json);
    }

    public override Result Add(TItem newItem, IContext? validationContext = null) {
        newItem.Key = GetNextKey();
        var result = newItem.Validate(validationContext);
        if (!result.IsSuccess) return result;
        _data.Add(newItem);
        Save();
        return result;
    }

    public override Result Update(TItem updatedItem, IContext? validationContext = null) {
        var index = _data.FindIndex(item => item.Key.Equals(updatedItem.Key));
        if (index == -1) return new ValidationError($"Item '{updatedItem.Key}' not found", nameof(updatedItem));
        var result = updatedItem.Validate(validationContext);
        if (!result.IsSuccess) return result;
        _data[index] = updatedItem;
        Save();
        return result;
    }

    public override Result Remove(TKey key) {
        var index = _data.FindIndex(item => item.Key.Equals(key));
        if (index == -1) return new ValidationError($"Item '{key}' not found", nameof(key));
        _data.RemoveAt(index);
        Save();
        return Result.Success();
    }
}
