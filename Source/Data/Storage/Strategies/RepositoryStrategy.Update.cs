namespace DotNetToolbox.Data.Strategies;

public abstract partial class RepositoryStrategy<TItem> {
    public virtual void Add(TItem newItem) => throw new NotImplementedException();

    public virtual void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) => throw new NotImplementedException();

    public virtual void Remove(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();
}
