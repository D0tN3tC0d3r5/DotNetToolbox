namespace DotNetToolbox.Data.Strategies.InMemory;

public class InMemoryUnitOfWorkRepositoryStrategy<TItem>
    : InMemoryRepositoryStrategy<TItem>
    , IUnitOfWorkRepositoryStrategy<TItem> {
    public InMemoryUnitOfWorkRepositoryStrategy() {
    }

    public InMemoryUnitOfWorkRepositoryStrategy(IEnumerable<TItem> data)
        : base(data) {
    }

    public void SaveChanges() { }
}
