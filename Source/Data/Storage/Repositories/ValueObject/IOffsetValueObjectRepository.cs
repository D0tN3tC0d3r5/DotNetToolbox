namespace DotNetToolbox.Data.Repositories.ValueObject;

public interface IOffsetValueObjectRepository<TItem, TOffsetMarker>
    : IValueObjectRepository<TItem>
    , IOffsetQueryableValueObjectRepository<TItem, TOffsetMarker> {
}
