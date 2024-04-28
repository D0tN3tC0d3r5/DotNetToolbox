namespace DotNetToolbox.Data.Repositories;

public interface IRepository;

public interface IRepository<TItem>
    : IQueryable<TItem>
    , IRepository {
    void Seed(IEnumerable<TItem> seed);

    void Add(TItem newItem);
    void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem);
    void Remove(Expression<Func<TItem, bool>> predicate);
}
