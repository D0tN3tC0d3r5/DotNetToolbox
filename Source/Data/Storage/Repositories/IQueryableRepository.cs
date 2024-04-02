namespace DotNetToolbox.Data.Repositories;

public interface IQueryableRepository<TRepository, out TModel>
    : IQueryable<TModel>,
      IAsyncEnumerable<TModel>
    where TRepository : IQueryableRepository<TRepository, TModel>
    where TModel : class {
}
