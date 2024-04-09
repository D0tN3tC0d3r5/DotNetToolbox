namespace DotNetToolbox.Data.Repositories;

public interface ISimpleKeyStorage<TModel, in TKey>
    : IReadOnlySimpleKeyStorage<TModel, TKey>,
      IWriteOnlySimpleKeyRepository<TModel, TKey>
    where TModel : class, ISimpleKeyEntity<TModel, TKey>, new()
    where TKey : notnull;
