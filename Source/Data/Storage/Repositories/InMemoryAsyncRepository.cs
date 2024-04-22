namespace DotNetToolbox.Data.Repositories;

public class InMemoryAsyncRepository<TItem>(IEnumerable<TItem> data)
    : InMemoryAsyncRepository<InMemoryAsyncRepository<TItem>, TItem>(data){
    public InMemoryAsyncRepository()
        : this([]) { }
}

// ReSharper disable PossibleMultipleEnumeration
public class InMemoryAsyncRepository<TRepository, TItem>(IEnumerable<TItem> data)
    : AsyncRepository<TRepository, InMemoryAsyncRepositoryStrategy<TRepository, TItem>, TItem>(data, new InMemoryAsyncRepositoryStrategy<TRepository, TItem>(data))
    where TRepository : AsyncRepository<TRepository, InMemoryAsyncRepositoryStrategy<TRepository, TItem>, TItem>{
    public InMemoryAsyncRepository()
        : this([]) { }
}
// ReSharper enable PossibleMultipleEnumeration
