namespace DotNetToolbox.Data.Strategies.ValueObject;

public interface IValueObjectRepositoryStrategy<TItem>
    : IRepositoryStrategy<TItem>
    , IValueObjectRepository<TItem>
    , IPagedQueryableRepository<TItem>
    , IOffsetQueryableRepository<TItem>;
