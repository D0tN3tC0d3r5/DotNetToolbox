namespace DotNetToolbox.Data.Repositories.ValueObject;

public interface IValueObjectRepository<TItem>
    : IRepository
    , IQueryableValueObjectRepository<TItem>
    , IUpdatableValueObjectRepository<TItem>;
