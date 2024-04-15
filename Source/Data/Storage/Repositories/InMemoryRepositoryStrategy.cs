namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategy<TItem>(IEnumerable<TItem> remote)
    : InMemoryRepositoryStrategy<TItem, TItem>(remote, s => s, s => s)
    where TItem : class;

public class InMemoryRepositoryStrategy<TModel, TEntity>(IEnumerable<TEntity> remote,
                                                         Expression<Func<TModel, TEntity>> projectToEntity,
                                                         Expression<Func<TEntity, TModel>> projectToModel)
    : RepositoryStrategy<TModel, TEntity>(remote, projectToEntity, projectToModel)
    where TModel : class
    where TEntity : class {
    public override bool HaveAny()
        => Remote.Any();

    public override int Count()
        => Remote.Count();

    public override TModel[] ToArray()
        => [.. Remote.AsQueryable().Select(ProjectToModel)];

    public override TModel? GetFirst() {
        var entity = Remote.FirstOrDefault();
        return entity is null
            ? default
            : ConvertToModel(entity);
    }

    public override void Add(TModel newItem) {
        var collection = IsOfType<ICollection<TEntity>>(Remote);
        var newEntity = ConvertToEntity(newItem);
        collection.Add(newEntity);
    }

    public override void Update(Expression<Func<TModel, bool>> predicate, TModel updatedItem) {
        var collection = IsOfType<IList<TEntity>>(Remote);
        var convertedPredicate = ConvertToRemoteExpression<Func<TEntity, bool>>(predicate);
        var itemToUpdate = Remote.AsQueryable().FirstOrDefault(convertedPredicate);
        if (itemToUpdate is null) return;
        var index = collection.IndexOf(itemToUpdate);
        collection[index] = ConvertToEntity(updatedItem);
    }

    public override void Remove(Expression<Func<TModel, bool>> predicate) {
        var collection = IsOfType<ICollection<TEntity>>(Remote);
        var convertedPredicate = ConvertToRemoteExpression<Func<TEntity, bool>>(predicate);
        var itemToRemove = Remote.AsQueryable().FirstOrDefault(convertedPredicate);
        if (itemToRemove is null) return;
        collection.Remove(itemToRemove);
    }
}
