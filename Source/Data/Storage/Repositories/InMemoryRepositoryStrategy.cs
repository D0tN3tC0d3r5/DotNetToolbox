namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategy<TModel>(IEnumerable<TModel> remote)
    : InMemoryRepositoryStrategy<TModel, TModel>(remote) {
    public override void Add(TModel newItem)
        => AddEntity(newItem);

    public override void Update(Expression<Func<TModel, bool>> predicate, TModel updatedItem)
        => UpdateEntity(predicate, updatedItem);
}

public class InMemoryRepositoryStrategy<TModel, TEntity>(IEnumerable<TEntity> remote, Expression<Func<TEntity, TModel>>? convertToModel = null, Func<TModel, TEntity>? convertToEntity = null)
    : RepositoryStrategy<TModel, TEntity>(remote, convertToModel, convertToEntity) {
    public override TModel[] ToArray()
        => [.. ApplyExpressionToRemote().Select(ProjectToModel!)];

    public override TModel? GetFirst() {
        var entity = ApplyExpressionToRemote().FirstOrDefault();
        return entity is null
            ? default
            : ConvertToModel!(entity);
    }

    public override bool HaveAny()
        => ApplyExpressionToRemote().Any();

    public override void Add(TModel newItem)
        => AddEntity(ConvertToEntity!(newItem));

    protected void AddEntity(TEntity newEntity) {
        var collection = Remote as ICollection<TEntity>
            ?? throw new NotSupportedException($"Remote is not a collection. Found '{Remote.GetType().Name}'.");
        collection.Add(newEntity);
    }

    public override void Update(Expression<Func<TModel, bool>> predicate, TModel updatedItem)
        => UpdateEntity(predicate, ConvertToEntity!(updatedItem));

    protected void UpdateEntity(Expression<Func<TModel, bool>> predicate, TEntity updatedItem) {
        var collection = Remote as ICollection<TEntity>
            ?? throw new NotSupportedException($"Remote is not a collection. Found '{Remote.GetType().Name}'.");
        var itemToRemove = ConvertToRemoteAndApply<IQueryable<TEntity>>(predicate).FirstOrDefault();
        if (itemToRemove is null)
            return;
        if (!collection.Remove(itemToRemove))
            return;
        collection.Add(updatedItem);
    }

    public override void Remove(Expression<Func<TModel, bool>> predicate) {
        var collection = Remote as ICollection<TEntity>
            ?? throw new NotSupportedException($"Remote is not a collection. Found '{Remote.GetType().Name}'.");
        var itemToRemove = ConvertToRemoteAndApply<IQueryable<TEntity>>(predicate).FirstOrDefault();
        if (itemToRemove is null)
            return;
        collection.Remove(itemToRemove);
    }
}
