namespace Sophia.Data;

public abstract class EntityFrameworkRepository<TDomainModel, TEntity>(DataContext dataContext, DbContext dbContext)
    : Repository<TDomainModel>(dataContext)
    where TDomainModel : class, new()
    where TEntity : class, new() {
    protected DbSet<TEntity> DbSet { get; } = dbContext.Set<TEntity>();

    public sealed override IQueryProvider Provider
        => DbSet.Select(Project).Provider;
    public sealed override IAsyncEnumerator<TDomainModel> GetAsyncEnumerator(CancellationToken ct = default)
        => DbSet.Select(Project).AsAsyncEnumerable().GetAsyncEnumerator(ct);
    public sealed override IEnumerator<TDomainModel> GetEnumerator()
        => DbSet.Select(Project).GetEnumerator();
    public sealed override Expression Expression
        => DbSet.Select(Project).Expression;

    public sealed override Task Add(TDomainModel input, CancellationToken cancellationToken = default)
        => DbSet.AddAsync(CreateFrom(input), cancellationToken).AsTask();

    public sealed override Task Update(TDomainModel input, CancellationToken cancellationToken = default) {
        DbSet.Update(CreateFrom(input));
        return Task.CompletedTask;
    }

    public sealed override Task Remove(TDomainModel input, CancellationToken cancellationToken = default) {
        DbSet.Remove(CreateFrom(input));
        return Task.CompletedTask;
    }

    protected abstract Expression<Func<TEntity, bool>> Translate(Expression<Func<TDomainModel, bool>> predicate);
    protected abstract Expression<Func<TEntity, TDomainModel>> Project { get; }
    protected abstract Action<TDomainModel, TEntity> UpdateFrom { get; }
    protected virtual TEntity CreateFrom(TDomainModel input) {
        var entity = new TEntity();
        UpdateFrom(input, entity);
        return entity;
    }
}
