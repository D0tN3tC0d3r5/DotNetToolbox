namespace DotNetToolbox.Data.Repositories;

public class AddOnlyRepository<TItem>
    : ReadOnlyRepository<TItem>,
      ICanAddItem<TItem> {
    public AddOnlyRepository() {
    }
    public AddOnlyRepository(Expression expression)
        : base(expression) {
    }
    public AddOnlyRepository(IEnumerable<TItem> data)
        : base(data) {
    }
    public virtual void Add(TItem item)
        => Strategy.ExecuteAction("Add", input: item);
}