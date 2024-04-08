namespace DotNetToolbox.Data.Repositories;

public interface ISimpleKeyRepository<TModel, in TKey>
    : IReadOnlySimpleKeyRepository<TModel, TKey>,
      IWriteOnlySimpleKeyRepository<TModel, TKey>
    where TModel : class, ISimpleKeyEntity<TModel, TKey>, new()
    where TKey : notnull;
