namespace DotNetToolbox.Data.Repositories;

public abstract class Queryable<TItem>
    : IQueryable<TItem> {
    private readonly Expression _expression;
    internal IEnumerable<TItem> Data { get; }
    protected Queryable(IEnumerable<TItem>? data, Expression? expression) {
        Data = data ?? [];
        if (expression is not null && expression.Type.GetElementType() != typeof(TItem))
            throw new ArgumentException($"Expression element type must be of type {typeof(TItem).Name}.", nameof(expression));
        _expression = expression ?? Expression.Constant(this);
    }

    public Type ElementType { get; } = typeof(TItem);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TItem> GetEnumerator()
        => Data.GetEnumerator();

    Expression IQueryable.Expression => _expression;
    IQueryProvider IQueryable.Provider => Data.AsQueryable().Provider;
}
