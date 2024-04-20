namespace DotNetToolbox.Data.Repositories;

public interface IOrderedRepository<TItem>
    : IOrderedQueryableRepository<TItem>, IRepository<TItem>
    where TItem : class {
}
