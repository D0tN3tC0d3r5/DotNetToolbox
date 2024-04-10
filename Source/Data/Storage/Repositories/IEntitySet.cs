namespace DotNetToolbox.Data.Repositories;

#pragma warning disable CA1010
public interface IEntitySet
    : IQueryable {
    new IEntitySetQueryHandler Provider { get; }
}
#pragma warning restore CA1010

public interface IEntitySet<out TModel>
    : IQueryable<TModel>,
      IAsyncEnumerable<TModel>,
      IEntitySet;

public interface IOrderedEntitySet<out TModel>
    : IOrderedQueryable<TModel>,
      IEntitySet<TModel>;
