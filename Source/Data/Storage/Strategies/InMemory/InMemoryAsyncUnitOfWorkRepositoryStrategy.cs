namespace DotNetToolbox.Data.Strategies.InMemory;

public class InMemoryAsyncUnitOfWorkRepositoryStrategy<TItem>
    : InMemoryAsyncRepositoryStrategy<TItem>, IAsyncUnitOfWorkRepositoryStrategy<TItem> {
    public InMemoryAsyncUnitOfWorkRepositoryStrategy() {
    }

    public InMemoryAsyncUnitOfWorkRepositoryStrategy(IEnumerable<TItem> data)
        : base(data) {
    }

    public void SaveChanges() { }
    public Task SaveChangesAsync(CancellationToken ct = default)
        => Task.CompletedTask;
}
