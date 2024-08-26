namespace DotNetToolbox.Data.File;

public class JsonFileRepositoryStrategy<TItem, TKey>
    : RepositoryStrategy<JsonFileRepositoryStrategy<TItem, TKey>, TItem, TKey>
    where TItem : class, IEntity<TKey>
    where TKey : notnull {
    private readonly string _filePath = ".";
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public override void Load() {
        if (!System.IO.File.Exists(_filePath)) return;
        var json = System.IO.File.ReadAllText(_filePath);
        var items = JsonSerializer.Deserialize<TItem[]>(json, _jsonOptions) ?? [];
        AddMany(items);
    }

    private void Save() {
        var json = JsonSerializer.Serialize(Repository.ToArray(), _jsonOptions);
        System.IO.File.WriteAllText(_filePath, json);
    }

    public override void Add(TItem newItem) {
        base.Add(newItem);
        Save();
    }

    public override void Update(TItem updatedItem) {
        base.Update(updatedItem);
        Save();
    }

    public override void Remove(TKey key) {
        base.Remove(key);
        Save();
    }

    // Implement other methods as needed, ensuring to call Save() after modifications
}
