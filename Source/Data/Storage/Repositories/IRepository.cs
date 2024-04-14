namespace DotNetToolbox.Data.Repositories;

public interface IRepository
    : IReadOnlyRepository;

public interface IRepository<TItem>
    : IReadOnlyRepository<TItem> {
    void Add(TItem newItem);
    void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem);
    void Remove(Expression<Func<TItem, bool>> predicate);
}
