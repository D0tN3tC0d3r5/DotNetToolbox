namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategy<TItem>(IEnumerable<TItem> remote)
    : InMemoryRepositoryStrategy<TItem, TItem>(remote, s => s, s => s)
    where TItem : class, new();

public class InMemoryRepositoryStrategy<TModel, TEntity>(IEnumerable<TEntity> remote,
                                                         Expression<Func<TModel, TEntity>> projectToEntity,
                                                         Expression<Func<TEntity, TModel>> projectToModel)
    : RepositoryStrategy<TModel, TEntity>(remote, projectToEntity, projectToModel)
    where TModel : class, new()
    where TEntity : class, new() {
    public override bool HaveAny()
        => GetQueryableRemote().Any();

    public override int Count()
        => GetQueryableRemote().Count();

    public override TModel[] ToArray()
        => [.. GetQueryableRemote().Select(ProjectToModel!)];

    public override TModel? GetFirst() {
        var entity = GetQueryableRemote().FirstOrDefault();
        return entity is null
            ? default
            : ConvertToModel(entity);
    }

    public override void Add(TModel newItem) {
        var collection = IsOfType<ICollection<TEntity>>(Remote);
        var newEntity = ConvertToEntity!(newItem);
        collection.Add(newEntity);
    }

    public override void Update(Expression<Func<TModel, bool>> predicate, TModel updatedItem) {
        Remove(predicate);
        Add(updatedItem);
    }

    public override void Remove(Expression<Func<TModel, bool>> predicate) {
        var collection = IsOfType<ICollection<TEntity>>(Remote);
        var itemToRemove = ConvertToRemoteAndApply<IQueryable<TEntity>>(predicate).FirstOrDefault();
        if (itemToRemove is null)
            return;
        collection.Remove(itemToRemove);
    }
}
