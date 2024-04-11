namespace DotNetToolbox.Data.Repositories;

public interface ICanAddOrUpdateItem<in TItem> :
    ICanAddItem<TItem>,
    ICanUpdateItem<TItem> {
    Task AddOrUpdate(TItem input, CancellationToken ct = default);
}