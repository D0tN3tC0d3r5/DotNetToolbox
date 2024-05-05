namespace DotNetToolbox.Data.Repositories;

internal interface IRepositoryFactory {
    IRepository<TItem> CreateRepository<TItem>(IEnumerable<TItem>? data = null);
    IRepository<TItem> CreateRepository<TItem>(string name, IEnumerable<TItem>? data = null);
    IRepository<TItem> CreateRepository<TItem, TStrategy>(IEnumerable<TItem>? data = null)
        where TStrategy : class, IRepositoryStrategy<TItem>, new();
    IRepository<TItem> CreateRepository<TItem, TStrategy>(string name, IEnumerable<TItem>? data = null)
        where TStrategy : class, IRepositoryStrategy<TItem>, new();
}
