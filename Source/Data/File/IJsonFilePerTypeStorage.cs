
namespace DotNetToolbox.Data.File;

public interface IJsonFilePerTypeStorage<TItem, in TKey>
    : IStorage<TItem, TKey>
    where TItem : class, IEntity<TKey>
    where TKey : notnull {
    string FilePath { get; }
}
