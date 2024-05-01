namespace DotNetToolbox.Data.Repositories;

internal class RepositoryFactory
    : IRepositoryFactory {
    public IUpdatableValueObjectRepository<TItem> CreateRepository<TRepository, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : IUpdatableValueObjectRepository<TItem> {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TItem));
        return (IUpdatableValueObjectRepository<TItem>)Activator.CreateInstance(resultRepositoryType, data)!;
    }

    public IUpdatableValueObjectRepository<TItem> CreateRepository<TRepository, TItem>(IValueObjectRepositoryStrategy<TItem> strategy)
        where TRepository : IUpdatableValueObjectRepository<TItem> {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TItem));
        return (IUpdatableValueObjectRepository<TItem>)Activator.CreateInstance(resultRepositoryType, IsNotNull(strategy), new List<TItem>())!;
    }
}
