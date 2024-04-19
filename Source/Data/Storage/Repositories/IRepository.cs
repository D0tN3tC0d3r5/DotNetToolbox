namespace DotNetToolbox.Data.Repositories;

public interface IRepository<TItem>
    : IReadOnlyRepository<TItem>
    where TItem : class {
    void Add(TItem newItem);
    void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem);
    void Remove(Expression<Func<TItem, bool>> predicate);
}
