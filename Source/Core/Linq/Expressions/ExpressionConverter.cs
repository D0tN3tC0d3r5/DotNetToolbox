// ReSharper disable once CheckNamespace
namespace System.Linq.Expressions;

public static class ExpressionConverter {
    public static Expression ReplaceExpressionType(this Expression expression, params TypeMapper[] mappers) {
        var originalType = expression is LambdaExpression lambda
            ? lambda.Parameters[0].Type
            : expression.Type;
        var visitor = new ExpressionConversionVisitor(mappers);
        return visitor.Convert(expression);
    }

    public static TResult Apply<TItem, TResult>(this Expression expression, IEnumerable<TItem> target)
        => target.AsQueryable().Provider.Execute<TResult>(expression);
}
