//namespace DotNetToolbox.Data.Repositories;

//public class EntitySetQueryHandler(IRepositoryStrategy strategy)
//    : IEntitySetQueryHandler {
//    //private static MethodInfo? _genericCreateQueryMethod;
//    //private static MethodInfo? _genericExecuteQueryMethod;
//    //private static MethodInfo GenericCreateQueryMethod
//    //    => _genericCreateQueryMethod ??= typeof(EntitySetQueryHandler)
//    //          .GetMethod("Create", 1, BindingFlags.Instance | BindingFlags.Public, null, [typeof(Expression)], null)!;
//    //private static MethodInfo GenericExecuteMethod
//    //    => _genericExecuteQueryMethod ??= typeof(EntitySetQueryHandler)
//    //          .GetMethod("ExecuteQuery", 1, BindingFlags.Instance | BindingFlags.Public, null, [typeof(Expression)], null)!;

//    protected IRepositoryStrategy Strategy { get; } = IsNotNull(strategy);

//    IQueryable IQueryProvider.CreateQuery(Expression expression)
//        => Create(expression);
//    public IEntitySet Create(Expression expression)
//        => (IEntitySet)GenericCreateQueryMethod
//                      .MakeGenericMethod(GetSequenceType(expression.Type))
//                      .Invoke(this, [expression])!;

//    IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
//        => Create<TElement>(expression);
//    public IEntitySet<TElement> Create<TElement>(Expression expression)
//        => new EntitySet<TElement>(expression, default, Strategy);

//    object? IQueryProvider.Execute(Expression expression)
//        => (IEntitySet?)GenericExecuteMethod
//                       .MakeGenericMethod(GetSequenceType(expression.Type))
//                       .Invoke(this, [expression]);
//    TResult IQueryProvider.Execute<TResult>(Expression expression)
//        => ExecuteAsync<TResult>(expression);
//    TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
//        => ExecuteAsync<TResult>(expression, cancellationToken);
//    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
//        => Strategy.ExecuteAsync<TResult>(expression, cancellationToken);
//    public Task<TResult> ExecuteAsync<TResult>(string command, Expression expression, CancellationToken cancellationToken = default)
//        => Strategy.ExecuteAsync<TResult>(command, expression, cancellationToken);

//    private static Type GetSequenceType(Type type)
//        => type.GetElementType()
//        ?? throw new ArgumentException($"The type {type.Name} does not represent a sequence");
//}
