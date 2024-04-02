namespace DotNetToolbox.Data.Repositories;

public interface ICompositeKeyEntityRepository<TRepository, TModel>
    : IReadOnlyCompositeKeyRepository<TRepository, TModel>,
      IWriteOnlyCompositeKeyEntityRepository<TRepository, TModel>
    where TRepository : ICompositeKeyEntityRepository<TRepository, TModel>
    where TModel : class, ICompositeKeyEntity<TModel>, new();
