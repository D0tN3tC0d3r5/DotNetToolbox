namespace DotNetToolbox.Data.Repositories;

public class ModelAsyncQueryProvider(IQueryCompiler queryCompiler)
    : IAsyncQueryProvider {
    private static MethodInfo? _genericCreateQueryMethod;
    private static MethodInfo? _genericExecuteAsyncMethod;
    private static MethodInfo? _genericExecuteMethod;
    private readonly IQueryCompiler _queryCompiler = queryCompiler;

    private static MethodInfo GenericCreateQueryMethod
        => _genericCreateQueryMethod ??= typeof(ModelAsyncQueryProvider)
        .GetMethod("CreateQuery", 1, BindingFlags.Instance | BindingFlags.Public, null, [typeof(Expression)], null)!;

    private MethodInfo GenericExecuteMethod
        => _genericExecuteMethod ??= typeof(ModelAsyncQueryProvider)
        .GetMethod("Execute", 1, BindingFlags.Instance | BindingFlags.Public, null, [typeof(Expression)], null)!;

    private MethodInfo GenericExecuteAsyncMethod
        => _genericExecuteAsyncMethod ??= typeof(ModelAsyncQueryProvider)
        .GetMethod("ExecuteAsync", 1, BindingFlags.Instance | BindingFlags.Public, null, [typeof(Expression), typeof(CancellationToken)], null)!;

    public IQueryable CreateQuery(Expression expression)
        => (IQueryable)GenericCreateQueryMethod
            .MakeGenericMethod(GetSequenceType(expression.Type))
            .Invoke(this, [expression])!;

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => new QueryableRepository<TElement>(this, expression);

    public object? Execute(Expression expression)
        => (IQueryable)GenericExecuteMethod
            .MakeGenericMethod(GetSequenceType(expression.Type))
            .Invoke(this, [expression])!;

    public TResult Execute<TResult>(Expression expression)
        => _queryCompiler.Execute<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        => _queryCompiler.ExecuteAsync<TResult>(expression, cancellationToken);

    private static Type GetSequenceType(Type type)
        => type.GetElementType()
        ?? throw new ArgumentException($"The type {type.Name} does not represent a sequence");
}
