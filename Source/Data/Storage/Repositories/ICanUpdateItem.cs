namespace DotNetToolbox.Data.Repositories;

public interface ICanUpdateItem<in TItem> {
    Task Update(TItem input, CancellationToken ct = default);
}
