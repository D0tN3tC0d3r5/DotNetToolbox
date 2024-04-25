namespace DotNetToolbox.Data.Repositories;

public class InMemoryAsyncRepository<TItem>
    : InMemoryAsyncRepository<InMemoryAsyncRepository<TItem>, TItem>;

// ReSharper disable PossibleMultipleEnumeration
public class InMemoryAsyncRepository<TRepository, TItem>()
    : AsyncRepository<TRepository, InMemoryAsyncRepositoryStrategy<TItem>, TItem>([])
    where TRepository : AsyncRepository<TRepository, InMemoryAsyncRepositoryStrategy<TItem>, TItem>{
}
// ReSharper enable PossibleMultipleEnumeration
