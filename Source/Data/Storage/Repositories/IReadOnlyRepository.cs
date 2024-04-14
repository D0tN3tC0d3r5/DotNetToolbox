namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlyRepository
    : IQueryable {
    bool HaveAny();
    int Count();
}

public interface IReadOnlyRepository<TItem>
    : IReadOnlyRepository,
      IQueryable<TItem> {
    TItem[] ToArray();
    TItem? GetFirst();
    IRepositoryStrategy<TItem> Strategy { get; }
}
