namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlyRepository
    : IQueryable {
    bool HaveAny();
    int Count();
    object[] ToArray();
    object? GetFirst();
}

public interface IReadOnlyRepository<TItem>
    : IReadOnlyRepository,
      IQueryable<TItem>
    where TItem : class {
    new TItem[] ToArray();
    new TItem? GetFirst();
    IRepositoryStrategy<TItem> Strategy { get; }
}
