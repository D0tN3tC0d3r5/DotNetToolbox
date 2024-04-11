namespace DotNetToolbox.Data.Repositories;

public interface ICanAddItemAsync<in TItem> {
    Task Add(TItem input, CancellationToken ct = default);
}
