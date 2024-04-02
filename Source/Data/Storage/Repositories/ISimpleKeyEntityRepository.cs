namespace DotNetToolbox.Data.Repositories;

public interface ISimpleKeyEntityRepository<TRepository, TModel, in TKey>
    : IReadOnlySimpleKeyRepository<TRepository,TModel, TKey>,
      IWriteOnlySimpleKeyEntityRepository<TRepository, TModel, TKey>
    where TRepository : ISimpleKeyEntityRepository<TRepository, TModel, TKey>
    where TModel : class, ISimpleKeyEntity<TModel, TKey>, new()
    where TKey : notnull;
