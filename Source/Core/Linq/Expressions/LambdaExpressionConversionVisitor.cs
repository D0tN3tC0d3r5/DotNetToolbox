// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Expressions;

public class LambdaExpressionConversionVisitor<TSource, TTarget>(Func<TSource?, TTarget?>? convert = null)
    : LambdaExpressionConversionVisitor(new LambdaConversionTypeMapper<TSource, TTarget>(convert));

public class LambdaExpressionConversionVisitor(params ILambdaConversionTypeMapper[] mappers)
        : ExpressionVisitor {
    private bool _isProcessingBody;
    private ParameterExpression[] _parameters = [];

    public LambdaExpressionConversionVisitor(Type sourceType, Type targetType, Func<object?, object?>? convert = null)
        : this (new LambdaConversionTypeMapper(sourceType, targetType, convert)) {
    }

    public Expression Convert(Expression expression)
        => Visit(expression);

    public TExpression Translate<TExpression>(Expression expression)
        where TExpression : Expression
        => (TExpression)Convert(expression);

    protected override Expression VisitLambda<TDelegate>(Expression<TDelegate> node) {
        _parameters = node.Parameters.ToArray<ParameterExpression>(p => (ParameterExpression)VisitParameter(p));
        _isProcessingBody = true;
        var body = Visit(node.Body);
        return Expression.Lambda(body, _parameters);
    }

    protected override Expression VisitParameter(ParameterExpression node) {
        var typeMapping = GetTypeMapping(node.Type);
        return typeMapping == null
                   ? base.VisitParameter(node)
                   : !_isProcessingBody
                       ? Expression.Parameter(typeMapping.TargetType, node.Name)
                       : _parameters.First(i => i.Name == node.Name);
    }

    protected override Expression VisitConstant(ConstantExpression node) {
        var typeMapping = GetTypeMapping(node.Value?.GetType());
        if (typeMapping == null || typeMapping.SourceType == typeMapping.TargetType) return base.VisitConstant(node);
        if (typeMapping.Convert == null) throw new NotSupportedException($"Cannot convert a value from '{typeMapping.SourceType.Name}' to '{typeMapping.TargetType.Name}'.");
        var convertedValue = typeMapping.Convert(node.Value);
        return Expression.Constant(convertedValue, typeMapping.TargetType);
    }

    protected override Expression VisitMember(MemberExpression node) {
        var typeMapping = GetTypeMapping(node.Member.DeclaringType);
        if (typeMapping == null) return base.VisitMember(node);
        var newMember = typeMapping.TargetType.GetMember(node.Member.Name).FirstOrDefault()
            ?? throw new InvalidOperationException($"No member with the name {node.Member.Name} exists on type {typeMapping.TargetType.Name}.");
        var newExpression = Visit(node.Expression);
        return Expression.MakeMemberAccess(newExpression, newMember);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node) {
        var method = node.Method;
        var arguments = node.Arguments.ToList<Expression>(Visit);

        if (method.IsGenericMethod) {
            var genericArguments = method.GetGenericArguments();
            var transformedGenericArguments = genericArguments.ToArray<Type>(t => GetTypeMapping(t)?.TargetType ?? t);
            method = method.GetGenericMethodDefinition().MakeGenericMethod(transformedGenericArguments);
        }

        var objectMember = Visit(node.Object);
        return Expression.Call(objectMember, method, arguments);
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

    private ILambdaConversionTypeMapper? GetTypeMapping(Type? sourceType)
        => mappers.FirstOrDefault(m => m.SourceType == sourceType);
}
