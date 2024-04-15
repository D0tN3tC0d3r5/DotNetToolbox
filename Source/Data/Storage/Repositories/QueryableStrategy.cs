namespace DotNetToolbox.Data.Repositories;

public abstract class QueryableStrategy<TItem>(IEnumerable<TItem> remote)
    : QueryableStrategy<TItem, TItem>(remote, s => s, s => s)
        where TItem : class;

public abstract class QueryableStrategy<TModel, TEntity>
    : IQueryableStrategy
    where TModel : class
    where TEntity : class {
    protected QueryableStrategy(IEnumerable<TEntity> remote,
                                Expression<Func<TModel, TEntity>> projectToEntity,
                                Expression<Func<TEntity, TModel>> projectToModel) {
        Remote = remote;
        ProjectToEntity = IsNotNull(projectToEntity);
        ConvertToEntity = ProjectToEntity.Compile();
        ProjectToModel = IsNotNull(projectToModel);
        ConvertToModel = ProjectToModel.Compile();
    }

    protected IQueryable<TModel> Local { get; } = Enumerable.Empty<TModel>().AsQueryable();
    protected IEnumerable<TEntity> Remote { get; }

    protected Expression<Func<TModel, TEntity>> ProjectToEntity { get; }
    protected Func<TModel, TEntity> ConvertToEntity { get; }

    protected Expression<Func<TEntity, TModel>> ProjectToModel { get; }
    protected Func<TEntity, TModel> ConvertToModel { get; }

    protected IQueryable<TEntity> GetQueryableRemote() {
        var mappers = new TypeMapper[] {
            new TypeMapper<TModel, TEntity>(ConvertToEntity),
            new(Local.GetType(), Remote.GetType(), Remote),
        };
        var convertedLocalExpression = Local.Expression.ReplaceExpressionType(mappers);
        var updatedRemote = Remote.AsQueryable()
                                  .Provider
                                  .Execute<IEnumerable<TEntity>>(convertedLocalExpression);
        return updatedRemote.AsQueryable();
    }
    protected Expression<TDelegate> ConvertToRemoteExpression<TDelegate>(Expression expression)
        where TDelegate : Delegate {
        var mappers = new TypeMapper[] {
            new TypeMapper<TModel, TEntity>(ConvertToEntity),
            new(Local.GetType(), Remote.GetType(), Remote),
        };
        return (Expression<TDelegate>)expression.ReplaceExpressionType(mappers);
    }

    protected TResult ConvertToRemoteAndApply<TResult>(Expression expression) {
        var updatedRemote = GetQueryableRemote();
        var result = expression.Apply<TEntity, TResult>(updatedRemote);
        return result;
    }
}
