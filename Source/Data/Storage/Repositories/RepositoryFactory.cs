namespace DotNetToolbox.Data.Repositories;

internal class RepositoryFactory
    : IRepositoryFactory {
    public IRepository<TItem> CreateRepository<TRepository, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem> {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TItem));
        return (IRepository<TItem>)Activator.CreateInstance(resultRepositoryType, data)!;
    }

    public IRepository<TItem> CreateRepository<TRepository, TItem>(IRepositoryStrategy<TItem> strategy)
        where TRepository : IRepository<TItem> {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TItem));
        return (IRepository<TItem>)Activator.CreateInstance(resultRepositoryType, IsNotNull(strategy), new List<TItem>())!;
    }
}
