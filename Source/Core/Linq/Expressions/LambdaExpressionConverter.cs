namespace System.Linq.Expressions;

public static class LambdaExpressionConverter {
    public static Expression Convert<TTarget>(LambdaExpression expression, Func<object?, TTarget?>? convert = null) {
        var visitor = new LambdaExpressionConversionVisitor(expression.Parameters[0].Type, typeof(TTarget), convert is null ? null : s => convert(s));
        return visitor.Convert(expression);
    }

    public static IQueryable Apply(LambdaExpression expression, IQueryable source, Func<object?, object?>? convert = null) {
        //var sourceType = source.GetType();
        //var visitor = new LambdaExpressionConversionVisitor(expression.Parameters[0].Type, sourceType, convert);
        //var newExpression = visitor.Translate<LambdaExpression>(expression);
        var result = source.Provider.Execute(expression)!;
        return (IQueryable)result;
    }
}
