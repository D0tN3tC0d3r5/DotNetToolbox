namespace Sophia.Data;

internal sealed class LambdaExpressionConversionVisitor<TModel, TEntity>(Func<TModel, TEntity>? convertToEntity = null)
        : ExpressionVisitor
        where TModel : class
        where TEntity : class, new() {
    private bool _isProcessingBody;
    private ParameterExpression[] _parameters = [];

    public TExpression Translate<TExpression>(Expression node)
        where TExpression : Expression
        => (TExpression)Visit(node);

    protected override Expression VisitLambda<T>(Expression<T> node) {
        _parameters = node.Parameters.ToArray<ParameterExpression>(p => (ParameterExpression)VisitParameter(p));
        _isProcessingBody = true;
        var body = Visit(node.Body);
        return Expression.Lambda(body, _parameters);
    }

    protected override Expression VisitParameter(ParameterExpression node)
        => node.Type != typeof(TModel)
               ? base.VisitParameter(node)
               : !_isProcessingBody
                   ? Expression.Parameter(typeof(TEntity), node.Name)
                   : _parameters.First(i => i.Name == node.Name);

    protected override Expression VisitMember(MemberExpression node) {
        if (node.Member.DeclaringType != typeof(TModel))
            return base.VisitMember(node);
        var newMember = typeof(TEntity).GetMember(node.Member.Name).FirstOrDefault()
            ?? throw new InvalidOperationException($"No member with the name {node.Member.Name} exists on type {typeof(TEntity).Name}.");
        var newExpression = Visit(node.Expression);
        return Expression.MakeMemberAccess(newExpression, newMember);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node) {
        var method = node.Method;
        var arguments = node.Arguments.Select(Visit).ToList();

        if (method.IsGenericMethod) {
            var genericArguments = method.GetGenericArguments();
            var transformedGenericArguments = genericArguments.Select(t => t == typeof(TModel) ? typeof(TEntity) : t).ToArray();
            method = method.GetGenericMethodDefinition().MakeGenericMethod(transformedGenericArguments);
        }

        var objectMember = Visit(node.Object);
        return Expression.Call(objectMember, method, arguments!);
    }

    protected override Expression VisitBinary(BinaryExpression node) {
        var left = Visit(node.Left);
        var right = Visit(node.Right);

        var conversion = VisitAndConvert(node.Conversion, nameof(VisitBinary));

        return Expression.MakeBinary(node.NodeType, left, right, node.IsLiftedToNull, node.Method, conversion);
    }

    protected override Expression VisitUnary(UnaryExpression node) {
        var operand = Visit(node.Operand);
        return Expression.MakeUnary(node.NodeType, operand, node.Type, node.Method);
    }

    protected override Expression VisitConditional(ConditionalExpression node) {
        var test = Visit(node.Test);
        var ifTrue = Visit(node.IfTrue);
        var ifFalse = Visit(node.IfFalse);
        return Expression.Condition(test, ifTrue, ifFalse);
    }

    protected override Expression VisitConstant(ConstantExpression node) {
        if (node.Value == null || node.Value.GetType() != typeof(TModel)) return base.VisitConstant(node);
        convertToEntity = IsNotNull(convertToEntity);
        var convertedValue = convertToEntity((TModel)node.Value);
        return Expression.Constant(convertedValue, typeof(TEntity));
    }

    protected override Expression VisitMemberInit(MemberInitExpression node) {
        var newExpression = (NewExpression)VisitNew(node.NewExpression);
        var bindings = node.Bindings.Select(VisitMemberBinding).ToList();
        return Expression.MemberInit(newExpression, bindings);
    }

    protected override Expression VisitNewArray(NewArrayExpression node) {
        var expressions = node.Expressions.Select(Visit).ToList();
        var elementType = node.Type.GetElementType();
        return node.NodeType == ExpressionType.NewArrayInit
            ? Expression.NewArrayInit(elementType!, expressions!)
            : (Expression)Expression.NewArrayBounds(elementType!, expressions!);
    }
}
