namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepository<TItem>
    : InMemoryRepository<InMemoryRepository<TItem>, TItem>{

    internal InMemoryRepository(List<TItem> data, Expression expression)
        : base(data, expression) {
    }
}

// ReSharper disable PossibleMultipleEnumeration
public class InMemoryRepository<TRepository, TItem>
    : Repository<TRepository, InMemoryRepositoryStrategy<TRepository, TItem>, TItem>
    where TRepository : Repository<TRepository, InMemoryRepositoryStrategy<TRepository, TItem>, TItem> {

    public InMemoryRepository()
        : this(new InMemoryRepositoryStrategy<TRepository, TItem>()) {
    }

    internal InMemoryRepository(List<TItem> data, Expression expression) {
        Strategy = new InMemoryRepositoryStrategy<TRepository, TItem>(data, expression);
    }
}
// ReSharper enable PossibleMultipleEnumeration
