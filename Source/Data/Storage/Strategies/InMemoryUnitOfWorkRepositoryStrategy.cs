namespace DotNetToolbox.Data.Strategies;

public class InMemoryUnitOfWorkRepositoryStrategy<TItem>(IEnumerable<TItem>? data = null)
    : InMemoryRepositoryStrategy<TItem>(data)
    , IUnitOfWorkRepositoryStrategy<TItem> {
    public void SaveChanges() { }
}
