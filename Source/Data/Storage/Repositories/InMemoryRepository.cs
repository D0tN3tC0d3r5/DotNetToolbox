namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepository<TItem>
    : InMemoryRepository<InMemoryRepository<TItem>, TItem>;

// ReSharper disable PossibleMultipleEnumeration
public class InMemoryRepository<TRepository, TItem>
    : Repository<TRepository, InMemoryRepositoryStrategy<TRepository, TItem>, TItem>
    where TRepository : Repository<TRepository, InMemoryRepositoryStrategy<TRepository, TItem>, TItem> {

    public InMemoryRepository()
        : base(new InMemoryRepositoryStrategy<TRepository, TItem>()) {
    }
}
// ReSharper enable PossibleMultipleEnumeration
