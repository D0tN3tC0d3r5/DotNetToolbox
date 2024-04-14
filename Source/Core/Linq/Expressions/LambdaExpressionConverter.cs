// ReSharper disable once CheckNamespace
namespace System.Linq.Expressions;

public static class LambdaExpressionConverter {
    public static LambdaExpression ConvertParameterType<TSource, TTarget>(this LambdaExpression expression, Func<TSource, TTarget>? convert = null) {
        var visitor = new LambdaExpressionConversionVisitor<TSource, TTarget>(convert);
        return (LambdaExpression)visitor.Convert(expression);
    }

    public static LambdaExpression ConvertParameterType<TTarget>(this LambdaExpression expression, Func<object, TTarget>? convert = null) {
        var visitor = new LambdaExpressionConversionVisitor(expression.Parameters[0].Type, typeof(TTarget), convert is null ? null : s => convert(s)!);
        return (LambdaExpression)visitor.Convert(expression);
    }

    public static IQueryable<TTarget> Convert<TSource, TTarget>(this IQueryable<TSource> source, IEnumerable<TTarget> target, Func<TSource, TTarget>? convert = null) {
        var convertedExpression = ConvertParameterType((LambdaExpression)source.Expression, convert);
        return (IQueryable<TTarget>)target.AsQueryable().Provider.Execute(convertedExpression)!;
    }

    public static TResult ConvertAndApply<TSource, TTarget, TResult>(this LambdaExpression expression, IEnumerable<TTarget> target, Func<TSource, TTarget>? convert = null) {
        var convertedExpression = ConvertParameterType(expression, convert);
        return convertedExpression.Apply<TTarget, TResult>(target);
    }

    public static IQueryable<TTarget> ConvertAndApply<TSource, TTarget>(this LambdaExpression expression, IEnumerable<TTarget> target, Func<TSource, TTarget>? convert = null) {
        var convertedExpression = ConvertParameterType(expression, convert);
        return convertedExpression.Apply<TTarget, IQueryable<TTarget>>(target);
    }

    public static IQueryable<TItem> Apply<TItem>(this Expression expression, IEnumerable<TItem> target)
        => target.AsQueryable().Provider.Execute<IQueryable<TItem>>(expression);

    public static TResult Apply<TItem, TResult>(this Expression expression, IEnumerable<TItem> target)
        => target.AsQueryable().Provider.Execute<TResult>(expression);
}
