namespace DotNetToolbox.Data.Repositories;

public class AddOrCreateOnlyRepository<TItem>
    : ReadOnlyRepository<TItem>,
      ICanAddItem<TItem>,
      ICanCreateItem<TItem>
    where TItem : class, new() {
    public AddOrCreateOnlyRepository() {
    }
    public AddOrCreateOnlyRepository(Expression expression)
        : base(expression) {
    }
    public AddOrCreateOnlyRepository(IEnumerable<TItem> data)
        : base(data) {
    }
    public virtual void Add(TItem item)
        => Strategy.ExecuteAction("Add", input: item);
    public virtual TItem Create(Action<TItem> set)
        => Strategy.ExecuteFunction<TItem>("Create", default!, set);
}