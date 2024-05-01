namespace DotNetToolbox.Data.Repositories.ValueObject;

public interface IQueryableValueObjectRepository<out TItem>
    : IQueryableRepository
    , IAsyncQueryable<TItem>;
