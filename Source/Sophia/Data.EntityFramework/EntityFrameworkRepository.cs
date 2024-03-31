namespace Sophia.Data;

public abstract class EntityFrameworkRepository<TModel, TModelKey, TEntity, TEntityKey>(DataContext dataContext, DbContext dbContext)
    : Repository<TModel, TModelKey>(dataContext)
    where TModel : class, IEntity<TModelKey>, new()
    where TModelKey: notnull
    where TEntity : class, IEntity<TEntityKey>, new()
    where TEntityKey : notnull {
    protected DbSet<TEntity> DbSet { get; } = dbContext.Set<TEntity>();

    protected Expression<Func<TEntity, TResult>> SwitchSource<TResult>(Expression<Func<TModel, TResult>> expression) {
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var body = Expression.Invoke(expression, Expression.Property(parameter, "model"));
        return Expression.Lambda<Func<TEntity, TResult>>(body, parameter);
    }

    public override Task<bool> HasAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => DbSet.AnyAsync(SwitchSource(predicate), ct);
}
