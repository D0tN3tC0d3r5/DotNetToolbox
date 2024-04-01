namespace Sophia.Data;

public interface IRepository<[DynamicallyAccessedMembers(IEntity.AccessedMembers)]TModel, in TKey>
    : IReadOnlyRepository<TModel, TKey>,
      IWriteOnlyRepository<TModel, TKey>
    where TModel : class
    where TKey : notnull;
