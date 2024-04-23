namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepository<TItem>
    : InMemoryRepository<InMemoryRepository<TItem>, TItem>{

    internal InMemoryRepository(IEnumerable<TItem> data, IQueryable query)
        : base(data, query) {
    }
}

// ReSharper disable PossibleMultipleEnumeration
public class InMemoryRepository<TRepository, TItem>
    : Repository<TRepository, InMemoryRepositoryStrategy<TRepository, TItem>, TItem>
    where TRepository : Repository<TRepository, InMemoryRepositoryStrategy<TRepository, TItem>, TItem> {

    public InMemoryRepository()
        : this(new List<TItem>()) {
    }

    internal InMemoryRepository(IEnumerable data, IQueryable? query = null)
        : base(new InMemoryRepositoryStrategy<TRepository, TItem>(data, query ?? data.AsQueryable())) {
    }
}
// ReSharper enable PossibleMultipleEnumeration
