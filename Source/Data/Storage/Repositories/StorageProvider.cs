namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyStorageProvider(IQueryStrategy queryStrategy)
        : IReadOnlyStorageProvider {
    private static MethodInfo? _genericCreateQueryMethod;
    private static MethodInfo? _genericExecuteQueryMethod;
    private readonly IQueryStrategy _queryStrategy = IsNotNull(queryStrategy);

    private static MethodInfo GenericCreateQueryMethod
        => _genericCreateQueryMethod ??= typeof(ReadOnlyStorageProvider)
        .GetMethod("Create", 1, BindingFlags.Instance | BindingFlags.Public, null, [typeof(Expression)], null)!;

    private static MethodInfo GenericExecuteMethod
        => _genericExecuteQueryMethod ??= typeof(ReadOnlyStorageProvider)
        .GetMethod("ExecuteQuery", 1, BindingFlags.Instance | BindingFlags.Public, null, [typeof(Expression)], null)!;

    IQueryable IQueryProvider.CreateQuery(Expression expression)
        => Create(expression);
    public IQueryableSet Create(Expression expression)
        => (IQueryableSet)GenericCreateQueryMethod
            .MakeGenericMethod(GetSequenceType(expression.Type))
            .Invoke(this, [expression])!;

    IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        => Create<TElement>(expression);
    public IQueryableSet<TElement> Create<TElement>(Expression expression)
        => new QueryableSet<TElement>(_queryStrategy, default, expression);

    object? IQueryProvider.Execute(Expression expression)
        => (IQueryableSet?)GenericExecuteMethod
            .MakeGenericMethod(GetSequenceType(expression.Type))
            .Invoke(this, [expression]);
    TResult IQueryProvider.Execute<TResult>(Expression expression)
        => ExecuteQuery<TResult>(expression);
    public TResult ExecuteQuery<TResult>(Expression expression)
        => _queryStrategy.ExecuteQuery<TResult>(expression);

    TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        => ExecuteQueryAsync<TResult>(expression, cancellationToken);
    public TResult ExecuteQueryAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        => _queryStrategy.ExecuteQueryAsync<TResult>(expression, cancellationToken);

    private static Type GetSequenceType(Type type)
        => type.GetElementType()
        ?? throw new ArgumentException($"The type {type.Name} does not represent a sequence");
}

public class StorageProvider(IQueryStrategy queryStrategy, IUpdateStrategy? updateStrategy = null)
        : ReadOnlyStorageProvider(queryStrategy),
          IStorageProvider {
    private readonly IUpdateStrategy _updateStrategy = IsNotNull(updateStrategy);
    public TResult ExecuteCommand<TResult>()
        => _updateStrategy.ExecuteCommand<TResult>();
    public TResult ExecuteCommandAsync<TResult>(CancellationToken cancellationToken = default)
        => _updateStrategy.ExecuteCommandAsync<TResult>(cancellationToken);
}
