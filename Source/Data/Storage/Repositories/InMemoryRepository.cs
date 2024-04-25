namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepository<TItem>
    : InMemoryRepository<InMemoryRepository<TItem>, TItem>;

// ReSharper disable PossibleMultipleEnumeration
public class InMemoryRepository<TRepository, TItem>()
    : Repository<TRepository, InMemoryRepositoryStrategy<TItem>, TItem>([])
    where TRepository : Repository<TRepository, InMemoryRepositoryStrategy<TItem>, TItem>;
// ReSharper enable PossibleMultipleEnumeration
