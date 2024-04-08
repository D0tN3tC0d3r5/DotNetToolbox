namespace DotNetToolbox.Data.Repositories;

public interface ICompositeKeyRepository<TModel>
    : IReadOnlyCompositeKeyRepository<TModel>,
      IWriteOnlyCompositeKeyRepository<TModel>
    where TModel : class, ICompositeKeyEntity<TModel>, new();
