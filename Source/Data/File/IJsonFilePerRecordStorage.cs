
namespace DotNetToolbox.Data.File;

public interface IJsonFilePerRecordStorage<TItem, in TKey>
    : IStorage<TItem, TKey>
    where TItem : class, IEntity<TKey>
    where TKey : notnull {
    string BaseFolderPath { get; }
}
