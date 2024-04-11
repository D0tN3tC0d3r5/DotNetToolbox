namespace DotNetToolbox.Data.Repositories;

public interface ICanPatchItem<TItem>
    where TItem : class {
    Task<TItem?> Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setModel, CancellationToken ct = default);
}