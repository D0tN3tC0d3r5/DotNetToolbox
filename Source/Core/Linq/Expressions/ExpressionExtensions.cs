// ReSharper disable once CheckNamespace
namespace System.Linq.Expressions;

public static class ExpressionExtensions {
    public static Expression ReplaceExpressionType(this Expression expression, params TypeMapper[] mappers) {
        var visitor = new ExpressionConversionVisitor(mappers);
        return visitor.Visit(expression);
    }

    public static TResult Apply<TItem, TResult>(this Expression expression, IEnumerable<TItem> target) {
        var  query = target.AsQueryable();
        var originalType = expression switch {
            LambdaExpression lambda => lambda.Parameters[0].Type,
            MethodCallExpression method => method.Arguments[0].Type,
            _ => expression.Type,
        };
        var mapper = new TypeMapper(originalType, query.GetType(), _ => query);
        var newExpression = expression.ReplaceExpressionType(mapper);
        return query.AsQueryable().Provider.Execute<TResult>(newExpression);
    }

    public static object? Apply<TItem>(this Expression expression, IEnumerable<TItem> target)
        => (object?)Apply<TItem, object>(expression, target);
}
