namespace DotNetToolbox.Data.Repositories;

public abstract class RepositoryStrategy<TModel>(IEnumerable<TModel> remote)
    : RepositoryStrategy<TModel, TModel>(remote);

public abstract class RepositoryStrategy<TModel, TEntity>(IEnumerable<TEntity> remote, Expression<Func<TEntity, TModel>>? convertToModel = null, Func<TModel, TEntity>? convertToEntity = null)
    : IRepositoryStrategy<TModel> {
    protected IQueryable<TModel> Local { get; } = Enumerable.Empty<TModel>().AsQueryable();
    protected IEnumerable<TEntity> Remote { get; } = remote;
    protected Expression<Func<TEntity, TModel>>? ProjectToModel { get; } = convertToModel;
    protected Func<TEntity, TModel>? ConvertToModel { get; } = convertToModel?.Compile();
    protected Func<TModel, TEntity>? ConvertToEntity { get; } = convertToEntity;

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

    protected TResult ConvertToRemoteAndApply<TResult>(LambdaExpression expression) {
        var target = ApplyExpressionToRemote();
        return ConvertToEntity is null
            ? expression.Apply<TEntity, TResult>(target)
            : expression.ConvertAndApply<TModel, TEntity, TResult>(target, ConvertToEntity);
    }

    protected IQueryable<TEntity> ApplyExpressionToRemote() {
        var expression = ((LambdaExpression)Local.Expression).ConvertParameterType<TEntity>();
        return Remote.AsQueryable().Provider.CreateQuery<TEntity>(expression);
    }
}
