namespace DotNetToolbox.Data.Strategies.ValueObject;

public interface IValueObjectRepositoryStrategy
    : IRepositoryStrategy
    , IRepository;

public interface IValueObjectRepositoryStrategy<TItem>
    : IRepositoryStrategy
    , IValueObjectRepository<TItem>;
