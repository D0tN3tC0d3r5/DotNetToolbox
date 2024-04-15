namespace DotNetToolbox.Data.Repositories;

public interface IRepository
    : IQueryable {
    new IRepositoryStrategy Provider { get; }
}

public interface IRepository<TItem>
    : IQueryable<TItem>
    , IRepository
    where TItem : class {
    new IRepositoryStrategy<TItem> Provider { get; }
    TItem[] ToArray();
    TItem? GetFirst();
    void Add(TItem newItem);
    void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem);
    void Remove(Expression<Func<TItem, bool>> predicate);
}
