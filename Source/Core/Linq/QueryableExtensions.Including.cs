// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class QueryableExtensions {
    internal static readonly MethodInfo IncludingMethodInfo
        = typeof(QueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(Including))
            .Single(mi => mi.GetGenericArguments().Length == 2
                       && mi.GetParameters().Any(pi => pi.Name == "navigationPropertyPath" && pi.ParameterType != typeof(string)));

    public static IIncludableQueryable<TEntity, TProperty> Including<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        where TEntity : class {
        IsNotNull(navigationPropertyPath, nameof(navigationPropertyPath));
        var callExpression = Expression.Call(instance: null,
                                             method: IncludingMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TProperty)),
                                             arguments: [source.Expression, Expression.Quote(navigationPropertyPath)]);
        var queryable = source.Provider.CreateQuery<TEntity>(callExpression);
        return new IncludableQueryable<TEntity, TProperty>(queryable);
    }

    internal static readonly MethodInfo WithAfterEnumerableMethodInfo
        = typeof(QueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(With))
            .Where(mi => mi.GetGenericArguments().Length == 3)
            .Single(mi => {
                var typeInfo = mi.GetParameters()[0].ParameterType.GenericTypeArguments[1];
                return typeInfo.IsGenericType
                    && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);
            });

    internal static readonly MethodInfo WithAfterReferenceMethodInfo
        = typeof(QueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(With))
            .Single(mi => mi.GetGenericArguments().Length == 3
                       && mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);

    public static IIncludableQueryable<TEntity, TProperty> With<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
        where TEntity : class {
        var methodCallExpression = Expression.Call(instance: null,
                                                   method: WithAfterEnumerableMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty)),
                                                   arguments: [source.Expression, Expression.Quote(navigationPropertyPath)]);
        var queryable = source.Provider.CreateQuery<TEntity>(methodCallExpression);
        return new IncludableQueryable<TEntity, TProperty>(queryable);
    }

    public static IIncludableQueryable<TEntity, TProperty> With<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, TPreviousProperty> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
        where TEntity : class {
        var methodCallExpression = Expression.Call(instance: null,
                                                   method: WithAfterReferenceMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty)),
                                                   arguments: [source.Expression, Expression.Quote(navigationPropertyPath)]);
        var queryable = source.Provider.CreateQuery<TEntity>(methodCallExpression);
        return new IncludableQueryable<TEntity, TProperty>(queryable);
    }

    private sealed class IncludableQueryable<TEntity, TProperty>(IQueryable<TEntity> queryable)
        : IIncludableQueryable<TEntity, TProperty>, IAsyncEnumerable<TEntity> {
        public Expression Expression => queryable.Expression;
        public Type ElementType => queryable.ElementType;
        public IQueryProvider Provider => queryable.Provider;

        public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => ((IAsyncEnumerable<TEntity>)queryable).GetAsyncEnumerator(cancellationToken);
        public IEnumerator<TEntity> GetEnumerator() => queryable.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal static readonly MethodInfo StringIncludeMethodInfo
        = typeof(QueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(Including))
            .Single(mi => mi.GetParameters()
                            .Any(pi => pi.Name == "navigationPropertyPath" && pi.ParameterType == typeof(string)));

    public static IQueryable<TEntity> Including<TEntity>(this IQueryable<TEntity> source, string navigationPropertyPath)
        where TEntity : class {
        IsNotEmpty(navigationPropertyPath, nameof(navigationPropertyPath));
        var methodCallExpression = Expression.Call(instance: null,
                                                   method: StringIncludeMethodInfo.MakeGenericMethod(typeof(TEntity)),
                                                   arg0: source.Expression,
                                                   arg1: Expression.Constant(navigationPropertyPath));
        return source.Provider.CreateQuery<TEntity>(methodCallExpression);
    }
}
