namespace Sophia.Data;

public abstract class EntityFrameworkRepository<[DynamicallyAccessedMembers(Mapper.AccessedMembers)] TModel, [DynamicallyAccessedMembers(Mapper.AccessedMembers)] TEntity, TKey>(DataContext dataContext, DbSet<TEntity> dbSet)
    : Repository<TModel, TKey>(dataContext)
    where TModel : class, IEntity<TKey>, new()
    where TEntity : class, IEntity<TKey>, new()
    where TKey : notnull {
    protected DbSet<TEntity> Set { get; } = dbSet;

    protected Expression<Func<TEntity, TResult>> SwitchSource<TResult>(Expression<Func<TModel, TResult>> expression) {
        var visitor = new BodyConverter();
        var newBody = visitor.Visit(expression.Body);
        var newParameters = visitor.VisitAndConvert(expression.Parameters, expression.Name);
        var newExpression = Expression.Lambda<Func<TEntity, TResult>>(newBody, expression.Name, newParameters);
        return newExpression;
    }

    public override async Task<bool> HaveAny(Expression<Func<TModel, bool>>? predicate, CancellationToken ct = default) {
        if (predicate is null)
            return await Set.AnyAsync(ct);

        var newExpression = SwitchSource(predicate);
        var typedExpression = (Expression<Func<TEntity, bool>>)newExpression;
        return await Set.AnyAsync(typedExpression, ct);
    }

    public override async Task Add(TModel input, CancellationToken ct = default)
        => await Set.AddAsync(Create(input), ct).AsTask();

  protected virtual Expression UpdateExpressionBody(Expression body) {
    var visitor = new BodyConverter();
    var updatedBody = visitor.Visit(body);
    return updatedBody;
  }

  private class BodyConverter : ExpressionVisitor {
    protected override Expression VisitParameter(ParameterExpression node)
        => Expression.Parameter(typeof(TEntity), node.Name);

    protected override Expression VisitMember(MemberExpression node) {
        var expression = Visit(node.Expression);
        var newMember = typeof(TEntity).GetMember(node.Member.Name).First();
        return Expression.MakeMemberAccess(expression, newMember);
    }
  }

    protected abstract Expression<Func<TEntity, TModel>> Project { get; }
    protected abstract Action<TModel, TEntity> UpdateFrom { get; }
    protected abstract Func<TModel, TEntity> Create { get; }
}
