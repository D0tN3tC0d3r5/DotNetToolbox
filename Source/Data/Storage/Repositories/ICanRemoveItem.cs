namespace DotNetToolbox.Data.Repositories;

public interface ICanRemoveItem<TItem> {
    Task Remove(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
}