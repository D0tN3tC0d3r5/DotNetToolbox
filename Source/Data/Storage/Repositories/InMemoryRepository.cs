namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepository<TItem>(IEnumerable<TItem> data)
    : InMemoryRepository<InMemoryRepository<TItem>, TItem>(data)
    where TItem : class {
    public InMemoryRepository() : this([]) { }
}

// ReSharper disable PossibleMultipleEnumeration
public class InMemoryRepository<TRepository, TItem>(IEnumerable<TItem> data)
    : Repository<TRepository, InMemoryRepositoryStrategy<TRepository, TItem>, TItem>(data, new InMemoryRepositoryStrategy<TRepository, TItem>(data))
    where TRepository : Repository<TRepository, InMemoryRepositoryStrategy<TRepository, TItem>, TItem>
    where TItem : class {
    public InMemoryRepository() : this([]) { }
}
// ReSharper enable PossibleMultipleEnumeration
