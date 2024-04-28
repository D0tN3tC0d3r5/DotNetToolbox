namespace DotNetToolbox.Data.Strategies;

public class InMemoryAsyncUnitOfWorkRepositoryStrategy<TItem>(IEnumerable<TItem>? data = null)
    : InMemoryAsyncRepositoryStrategy<TItem>(data), IAsyncUnitOfWorkRepositoryStrategy<TItem> {
    public Task SaveChangesAsync(CancellationToken ct = default)
        => Task.CompletedTask;
}
