
namespace DotNetToolbox.Data.Repositories;

internal interface IRepositoryFactory {
    IRepository<TItem> CreateRepository<TRepository, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem>;
    IRepository<TItem> CreateRepository<TRepository, TItem>(IRepositoryStrategy strategy, IEnumerable<TItem>? data = null)
        where TRepository : IRepository<TItem>;
    IAsyncRepository<TItem> CreateAsyncRepository<TRepository, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : IAsyncRepository<TItem>;
    IAsyncRepository<TItem> CreateAsyncRepository<TRepository, TItem>(IAsyncRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        where TRepository : IAsyncRepository<TItem>;
}
