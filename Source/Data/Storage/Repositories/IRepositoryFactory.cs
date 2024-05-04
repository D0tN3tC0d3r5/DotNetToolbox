namespace DotNetToolbox.Data.Repositories;

internal interface IRepositoryFactory {
    IRepository<TItem> CreateRepository<TRepository, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem>;
    IRepository<TItem> CreateRepository<TRepository, TItem>(string name, IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem>;
    IRepository<TItem> CreateRepository<TRepository, TItem>(IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem>;
    IRepository<TItem> CreateRepository<TRepository, TItem>(string name, IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem>;
}
