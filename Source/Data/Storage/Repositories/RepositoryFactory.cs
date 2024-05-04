namespace DotNetToolbox.Data.Repositories;

internal class RepositoryFactory
    : IRepositoryFactory {
    public IRepository<TItem> CreateRepository<TRepository, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem> {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TItem));
        return (IRepository<TItem>)Activator.CreateInstance(resultRepositoryType, data)!;
    }
    public IRepository<TItem> CreateRepository<TRepository, TItem>(string name, IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem> {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TItem));
        return (IRepository<TItem>)Activator.CreateInstance(resultRepositoryType, name, data)!;
    }
    public IRepository<TItem> CreateRepository<TRepository, TItem>(IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem> {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TItem));
        return (IRepository<TItem>)Activator.CreateInstance(resultRepositoryType, IsNotNull(strategy), data)!;
    }
    public IRepository<TItem> CreateRepository<TRepository, TItem>(string name, IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem> {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TItem));
        return (IRepository<TItem>)Activator.CreateInstance(resultRepositoryType, name, IsNotNull(strategy), data)!;
    }
}
