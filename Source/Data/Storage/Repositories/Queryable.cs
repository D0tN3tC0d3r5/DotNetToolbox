namespace DotNetToolbox.Data.Repositories;

public abstract class Queryable<TItem>
    : IQueryable<TItem> {
    private readonly Expression _expression;
    internal IQueryable<TItem> Data { get; }

    protected Queryable(IEnumerable<TItem>? data, Expression? expression) {
        _expression = expression ?? Expression.Constant(this);
        if (expression is not null && expression.Type.GetElementType() != typeof(TItem))
            throw new ArgumentException($"Expression element type must be of type {typeof(TItem).Name}.", nameof(expression));
        Data = (data ?? []).AsQueryable().Provider.CreateQuery<TItem>(_expression);
    }

    public IQueryable Create(Expression expression) {
        var repoType = Type.MakeGenericSignatureType(GetType(), expression.Type);
        return (IQueryable)Activator.CreateInstance(repoType, Data, expression)!;
    }

    public Type ElementType { get; } = typeof(TItem);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TItem> GetEnumerator()
        => Data.GetEnumerator();

    Expression IQueryable.Expression => _expression;
    IQueryProvider IQueryable.Provider => Provider;
    protected IQueryProvider Provider => Data.AsQueryable().Provider;
}
