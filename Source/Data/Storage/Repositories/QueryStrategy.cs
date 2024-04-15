namespace DotNetToolbox.Data.Repositories;

public abstract class QueryStrategy<TItem>(IEnumerable<TItem> remote)
    : QueryStrategy<TItem, TItem>(remote, s => s, s => s)
        where TItem : class;

public abstract class QueryStrategy<TModel, TEntity>
    : IQueryStrategy
    where TModel : class
    where TEntity : class {
    protected QueryStrategy(IEnumerable<TEntity> remote,
                                Expression<Func<TModel, TEntity>> projectToEntity,
                                Expression<Func<TEntity, TModel>> projectToModel) {
        Remote = remote;
        ProjectToEntity = IsNotNull(projectToEntity);
        ConvertToEntity = ProjectToEntity.Compile();
        ProjectToModel = IsNotNull(projectToModel);
        ConvertToModel = ProjectToModel.Compile();
    }

    IQueryable IQueryProvider.CreateQuery(Expression expression) => throw new NotImplementedException();

    IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression) => throw new NotImplementedException();

    object? IQueryProvider.Execute(Expression expression) => throw new NotImplementedException();

    TResult IQueryProvider.Execute<TResult>(Expression expression) => throw new NotImplementedException();

    IRepository IQueryStrategy.CreateRepository(Expression expression)
        => CreateRepository(expression);

    public IRepository<TModel> CreateRepository(Expression expression)
        => CreateRepository<TModel>(expression);

    public IRepository<TResult> CreateRepository<TResult>(Expression expression)
        where TResult : class {
        var local = expression.Apply<TModel, IEnumerable<TResult>>(Local);
        var mappers = new TypeMapper[] {
                new TypeMapper<TModel, TEntity>(ConvertToEntity),
                new(Local.GetType(), Remote.GetType(), Remote),
            };
        var remoteExpression = expression.ReplaceExpressionType(mappers);
        Remote = remoteExpression.Apply<TEntity, IEnumerable<TEntity>>(Remote);
        var result = new Repository<TResult>(local, this);
        return result;
    }

    protected IEnumerable<TModel> Local { get; } = Enumerable.Empty<TModel>();
    protected IEnumerable<TEntity> Remote { get; private set; }

    protected Expression<Func<TModel, TEntity>> ProjectToEntity { get; }
    protected Func<TModel, TEntity> ConvertToEntity { get; }

    protected Expression<Func<TEntity, TModel>> ProjectToModel { get; }
    protected Func<TEntity, TModel> ConvertToModel { get; }

    //protected IQueryable<TEntity> GetQueryableRemote() {
    //    var mappers = new TypeMapper[] {
    //        new TypeMapper<TModel, TEntity>(ConvertToEntity),
    //        new(Local.GetType(), Remote.GetType(), Remote),
    //    };
    //    var convertedLocalExpression = Local.AsQueryable().Expression.ReplaceExpressionType(mappers);
    //    var updatedRemote = Remote.AsQueryable()
    //                              .Provider
    //                              .Execute<IEnumerable<TEntity>>(convertedLocalExpression);
    //    return updatedRemote.AsQueryable();
    //}
    protected Expression<TDelegate> ConvertToRemoteExpression<TDelegate>(Expression expression)
        where TDelegate : Delegate {
        var mappers = new TypeMapper[] {
            new TypeMapper<TModel, TEntity>(ConvertToEntity),
            new(Local.GetType(), Remote.GetType(), Remote),
        };
        return (Expression<TDelegate>)expression.ReplaceExpressionType(mappers);
    }

    //protected TResult ConvertToRemoteAndApply<TResult>(Expression expression) {
    //    //var updatedRemote = GetQueryableRemote();
    //    var result = expression.Apply<TEntity, TResult>(Remote);
    //    return result;
    //}
}
