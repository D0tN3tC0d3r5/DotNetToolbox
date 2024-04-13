namespace DotNetToolbox.Data.Repositories;

public class CreateOnlyRepository<TItem>
    : ReadOnlyRepository<TItem>,
      ICanCreateItem<TItem>
    where TItem : class, new() {
    public CreateOnlyRepository() {
    }
    public CreateOnlyRepository(Expression expression)
        : base(expression) {
    }
    public CreateOnlyRepository(IEnumerable<TItem> data)
        : base(data) {
    }
    public virtual TItem Create(Action<TItem> set)
        => Strategy.ExecuteFunction<TItem>("Create", default!, set);
}