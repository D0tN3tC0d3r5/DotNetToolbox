namespace DotNetToolbox.Data.Repositories;

public partial interface IRepository<TItem> {
    void Add(TItem newItem);
    void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem);
    void Remove(Expression<Func<TItem, bool>> predicate);
}
