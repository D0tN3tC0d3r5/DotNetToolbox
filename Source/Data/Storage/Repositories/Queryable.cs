namespace DotNetToolbox.Data.Repositories;

public abstract class Queryable<TItem>
    : IQueryable<TItem> {
    private readonly Expression _expression;
    internal IQueryable<TItem> Data { get; }

    protected Queryable(IEnumerable<TItem> data, Expression? expression) {
        var source = data.AsQueryable();
        if (expression is not null && expression.Type.GetElementType() != typeof(TItem))
            throw new ArgumentException($"Expression element type must be of type {typeof(TItem).Name}.", nameof(expression));
        _expression = expression ?? Expression.Constant(source);
        Data = source.Provider.CreateQuery<TItem>(_expression);
    }

    public IQueryable Create(LambdaExpression expression) {
        try {
            return LambdaExpressionConverter.Apply(expression, Data);
        } catch (Exception ex) {
            throw new InvalidOperationException("Failed to create query.", ex);
        }
    }

    public Type ElementType { get; } = typeof(TItem);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TItem> GetEnumerator()
        => Data.GetEnumerator();

    Expression IQueryable.Expression => _expression;
    IQueryProvider IQueryable.Provider => Provider;
    protected IQueryProvider Provider => Data.AsQueryable().Provider;
}

internal class ExpressionTreeParameterReplacer : ExpressionVisitor {
    private readonly Type _newParameterType;

    private ExpressionTreeParameterReplacer(Type newParameterType) {
        _newParameterType = newParameterType;
    }

    internal static Expression CopyAndReplace(Expression expression, Type newType) {
        var visitor = new ExpressionTreeParameterReplacer(newType);
        return visitor.Visit(expression);
    }

    protected override Expression VisitParameter(ParameterExpression node)
        => node.Type == _newParameterType ? Expression.Parameter(_newParameterType, node.Name) : node;
}
