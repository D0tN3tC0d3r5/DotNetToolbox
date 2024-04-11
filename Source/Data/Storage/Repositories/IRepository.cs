namespace DotNetToolbox.Data.Repositories;

public interface IRepository<TItem, out TStrategy>
    : IItemSet<TItem, TStrategy>
    where TStrategy : IRepositoryStrategy<TStrategy> {
    TItem[] GetList();
    int Count();
    int CountWhere(Expression<Func<TItem, bool>> predicate);
    bool HaveAny();
    bool HaveAnyWhere(Expression<Func<TItem, bool>> predicate);
    TItem? FindFirst();
    TItem? FindFirstWhere(Expression<Func<TItem, bool>> predicate);
}
