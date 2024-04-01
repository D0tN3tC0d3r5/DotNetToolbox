namespace Sophia.Data;

public abstract class EntityFrameworkRepository<[DynamicallyAccessedMembers(Mapper.AccessedMembers)] TModel, [DynamicallyAccessedMembers(Mapper.AccessedMembers)] TEntity, TKey>(DataContext dataContext, DbSet<TEntity> dbSet)
    : Repository<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TEntity : class, IEntity<TKey>, new()
    where TKey : notnull {
    protected DbSet<TEntity> Set { get; } = dbSet;

    protected abstract Expression<Func<TEntity, TModel>> ProjectTo { get; }
    protected abstract Action<TModel, TEntity> UpdateFrom { get; }
    protected abstract Func<TModel, TEntity> CreateFrom { get; }

    [return: NotNullIfNotNull("expression")]
    protected Expression? SwitchSource(Expression? expression) {
        if (expression == null)
            return null;
        var visitor = new ExpressionConverter(CreateFrom);
        return visitor.Visit(expression);
    }

    public override async Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default) {
        var newExpression = SwitchSource(Expression);
        return await dbSet
            .AsQueryable().Provider
            .CreateQuery<TEntity>(newExpression)
            .Select(ProjectTo)
            .ToArrayAsync(ct);
    }

    protected override Task<bool> ExecuteHaveAny(Expression<Func<TModel, bool>>? predicate, CancellationToken ct = default) {
        var newPredicate = SwitchSource(predicate);
        return newPredicate is null
            ? Set.AnyAsync(ct)
            : Set.AnyAsync((Expression<Func<TEntity, bool>>)newPredicate, ct);
    }

    public override async Task Add(TModel input, CancellationToken ct = default) {
        var entity = CreateFrom(input);
        await Set.AddAsync(entity, ct).AsTask();
    }

    private sealed class ExpressionConverter(Func<TModel, TEntity> createFrom)
        : ExpressionVisitor {
        protected override Expression VisitParameter(ParameterExpression node)
            => node.Type == typeof(TModel)
            ? Expression.Parameter(typeof(TEntity), node.Name)
            : base.VisitParameter(node);

        protected override Expression VisitMember(MemberExpression node) {
            if (node.Member.DeclaringType != typeof(TModel)) return base.VisitMember(node);
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
        protected override Expression VisitConstant(ConstantExpression node) {
            if (node.Value != null && node.Value.GetType() == typeof(TModel)) {
                var convertedValue = createFrom((TModel)node.Value);
                return Expression.Constant(convertedValue, typeof(TEntity));
            }
            return base.VisitConstant(node);
        }
        protected override Expression VisitMemberInit(MemberInitExpression node) {
            var newExpression = (NewExpression)VisitNew(node.NewExpression);
            var bindings = node.Bindings.Select(VisitMemberBinding).ToList();
            return Expression.MemberInit(newExpression, bindings);
        }
        protected override Expression VisitNewArray(NewArrayExpression node) {
            var expressions = node.Expressions.Select(Visit).ToList();
            return node.NodeType == ExpressionType.NewArrayInit
                ? Expression.NewArrayInit(node.Type.GetElementType(), expressions)
                : (Expression)Expression.NewArrayBounds(node.Type.GetElementType(), expressions);
        }
        protected override Expression VisitLambda<T>(Expression<T> node) {
            var body = Visit(node.Body);
            var parameters = node.Parameters.Select(p => (ParameterExpression)VisitParameter(p)).ToList();
            return Expression.Lambda(body, parameters);
        }
    }
}
