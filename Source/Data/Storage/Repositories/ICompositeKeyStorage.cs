namespace DotNetToolbox.Data.Repositories;

public interface ICompositeKeyStorage<TModel>
    : IReadOnlyCompositeKeyStorage<TModel>,
      IWriteOnlyCompositeKeyRepository<TModel>
    where TModel : class, ICompositeKeyEntity<TModel>, new();
