namespace DotNetToolbox.Data.Repositories;

public class StorageProvider(IStorageCompiler compiler)
    : IStorageProvider {
    private static MethodInfo? _genericCreateQueryMethod;
    private static MethodInfo? _genericExecuteMethod;
    private static MethodInfo? _genericExecuteAsyncMethod;

    private static MethodInfo GenericCreateQueryMethod
        => _genericCreateQueryMethod ??= typeof(StorageProvider)
        .GetMethod("Create", 1, BindingFlags.Instance | BindingFlags.Public, null, [typeof(Expression)], null)!;

    private static MethodInfo GenericExecuteMethod
        => _genericExecuteMethod ??= typeof(StorageProvider)
        .GetMethod("Execute", 1, BindingFlags.Instance | BindingFlags.Public, null, [typeof(Expression)], null)!;

    private static MethodInfo GenericExecuteAsyncMethod
        => _genericExecuteAsyncMethod ??= typeof(StorageProvider)
        .GetMethod("ExecuteAsync", 1, BindingFlags.Instance | BindingFlags.Public, null, [typeof(Expression)], null)!;

    IQueryable IQueryProvider.CreateQuery(Expression expression)
        => Create(expression);
    public IStorage Create(Expression expression)
        => (IStorage)GenericCreateQueryMethod
            .MakeGenericMethod(GetSequenceType(expression.Type))
            .Invoke(this, [expression])!;

    IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        => Create<TElement>(expression);
    public IStorage<TElement> Create<TElement>(Expression expression)
        => new Storage<TElement>(this, expression);

    public object? Execute(Expression expression)
        => (IStorage?)GenericExecuteMethod
            .MakeGenericMethod(GetSequenceType(expression.Type))
            .Invoke(this, [expression]);

    public TResult Execute<TResult>(Expression expression)
        => compiler.Execute<TResult>(expression);

    public object? ExecuteAsync(Expression expression, CancellationToken cancellationToken = default)
        => (IStorage?)GenericExecuteAsyncMethod
            .MakeGenericMethod(GetSequenceType(expression.Type))
            .Invoke(this, [expression, cancellationToken]);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        => compiler.ExecuteAsync<TResult>(expression, cancellationToken);

    private static Type GetSequenceType(Type type)
        => type.GetElementType()
        ?? throw new ArgumentException($"The type {type.Name} does not represent a sequence");
}
