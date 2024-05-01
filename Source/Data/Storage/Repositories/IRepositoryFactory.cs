namespace DotNetToolbox.Data.Repositories;

internal interface IRepositoryFactory {
    IUpdatableValueObjectRepository<TItem> CreateRepository<TRepository, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : IUpdatableValueObjectRepository<TItem>;
    IUpdatableValueObjectRepository<TItem> CreateRepository<TRepository, TItem>(IValueObjectRepositoryStrategy<TItem> strategy)
        where TRepository : IUpdatableValueObjectRepository<TItem>;
}
