namespace DotNetToolbox.Data.Repositories;

public abstract class AsyncRepositoryStrategy<TModel>(IEnumerable<TModel> remote)
    : AsyncRepositoryStrategy<TModel, TModel>(remote) {
}

public abstract class AsyncRepositoryStrategy<TModel, TEntity>(IEnumerable<TEntity> remote,
                                                               Expression<Func<TEntity, TModel>>? convertToModel = null,
                                                               Func<TModel, TEntity>? convertToEntity = null)
    : IAsyncRepositoryStrategy<TModel> {
    protected IQueryable<TModel> Local { get; } = Enumerable.Empty<TModel>().AsQueryable();
    protected IEnumerable<TEntity> Remote { get; } = remote;
    protected Expression<Func<TEntity, TModel>>? ProjectToModel { get; } = convertToModel;
    protected Func<TEntity, TModel>? ConvertToModel { get; } = convertToModel?.Compile();
    protected Func<TModel, TEntity>? ConvertToEntity { get; } = convertToEntity;

    public virtual Task<bool> HaveAny(CancellationToken ct = default)
        => throw new NotImplementedException(nameof(HaveAny));
    public virtual Task<int> Count(CancellationToken ct = default)
        => throw new NotImplementedException(nameof(HaveAny));
    public virtual Task<TModel[]> ToArray(CancellationToken ct = default)
        => throw new NotImplementedException(nameof(ToArray));
    public virtual Task<TModel?> GetFirst(CancellationToken ct = default)
        => throw new NotImplementedException(nameof(GetFirst));
    public virtual Task Add(TModel newItem, CancellationToken ct = default)
        => throw new NotImplementedException(nameof(Add));
    public virtual Task Update(Expression<Func<TModel, bool>> predicate, TModel updatedItem, CancellationToken ct = default)
        => throw new NotImplementedException(nameof(Update));
    public virtual Task Remove(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
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
