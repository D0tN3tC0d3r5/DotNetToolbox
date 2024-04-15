namespace DotNetToolbox.Data.Repositories;

public interface IRepositoryStrategy<TItem>
    : IQueryableStrategy
      where TItem : class {
    bool HaveAny();
    int Count();
    TItem[] ToArray();
    TItem? GetFirst();
    void Add(TItem newItem);
    void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem);
    void Remove(Expression<Func<TItem, bool>> predicate);
}
