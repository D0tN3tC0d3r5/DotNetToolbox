
namespace DotNetToolbox.Data.Repositories;

internal interface IRepositoryFactory {
    IRepository<TItem> CreateRepository<TRepository, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem>;
    IRepository<TItem> CreateRepository<TRepository, TItem>(IRepositoryStrategy<TItem> strategy)
        where TRepository : IRepository<TItem>;
}
