
namespace DotNetToolbox.Data.File;

public interface IJsonFilePerRecordStorage<TItem, TKey>
    : IStorage<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    string BaseFolderPath { get; }
}
