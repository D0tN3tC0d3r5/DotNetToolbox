namespace DotNetToolbox.Data.Repositories;

public interface ICanCreateItem<TItem>
    where TItem : class, new() {
    Task<TItem> Create(Action<TItem> setItem, CancellationToken ct = default);
}