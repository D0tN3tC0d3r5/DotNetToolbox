namespace DotNetToolbox.Data.Repositories.ValueObject;

public interface IPagedValueObjectRepository<TItem>
    : IValueObjectRepository<TItem>
    , IPagedQueryableValueObjectRepository<TItem> {
}
