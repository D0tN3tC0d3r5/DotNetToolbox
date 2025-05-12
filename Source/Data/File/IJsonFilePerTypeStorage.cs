
namespace DotNetToolbox.Data.File;

public interface IJsonFilePerTypeStorage<TItem, TKey>
    : IStorage<TItem, TKey>
    where TItem : class, IEntity<TKey>
    where TKey : notnull {
    string FilePath { get; }
}
