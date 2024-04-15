namespace DotNetToolbox.Data.Repositories;

public abstract class RepositoryStrategy<TItem>(IEnumerable<TItem> remote)
    : RepositoryStrategy<TItem, TItem>(remote, s => s, s => s)
    where TItem : class;

public abstract class RepositoryStrategy<TModel, TEntity>(IEnumerable<TEntity> remote,
                                                          Expression<Func<TModel, TEntity>> projectToEntity,
                                                          Expression<Func<TEntity, TModel>> projectToModel)
    : QueryStrategy<TModel, TEntity>(remote, projectToEntity, projectToModel),
      IRepositoryStrategy<TModel>
    where TModel : class
    where TEntity : class {
    public virtual bool HaveAny()
        => throw new NotImplementedException(nameof(HaveAny));
    public virtual int Count()
        => throw new NotImplementedException(nameof(HaveAny));
    public virtual TModel[] ToArray()
        => throw new NotImplementedException(nameof(ToArray));
    public virtual TModel? GetFirst()
        => throw new NotImplementedException(nameof(GetFirst));
    public virtual void Add(TModel newItem)
        => throw new NotImplementedException(nameof(Add));
    public virtual void Update(Expression<Func<TModel, bool>> predicate, TModel updatedItem)
        => throw new NotImplementedException(nameof(Update));
    public virtual void Remove(Expression<Func<TModel, bool>> predicate)
        => throw new NotImplementedException(nameof(Remove));
}
