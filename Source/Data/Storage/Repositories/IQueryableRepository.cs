namespace DotNetToolbox.Data.Repositories;

public interface IQueryableRepository<out TModel>
    : IOrderedQueryable<TModel>,
      IAsyncEnumerable<TModel> {
}
