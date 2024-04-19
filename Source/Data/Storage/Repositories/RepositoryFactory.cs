namespace DotNetToolbox.Data.Repositories;

public static class RepositoryFactory {
    //public static TRepository CreateRepository<TSource, TRepository, TResult>(IQueryable<TSource> source, RepositoryQueryStrategy strategy, Expression expression, Func<TSource, TResult> convert)
    //    where TRepository : class, IQueryableRepository<TResult>
    //    where TResult : class {
    //    var resultType = source.GetType().GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
    //    var result = (IQueryable<TResult>)Activator.CreateInstance(resultType)!;
    //    var local = expression.Apply<TSource, IQueryable<TResult>>(source);
    //    var mappers = new TypeMapper[] {
    //                                       new TypeMapper<TSource, TResult>(convert),
    //                                       new(source.GetType(), typeof(IQueryable<TResult>), result),
    //                                   };
    //    var remoteExpression = expression.ReplaceExpressionType(mappers);
    //    result = remoteExpression.Apply<TResult, IQueryable<TResult>>(result);
    //    var repository = InstanceFactory.Create<TRepository>(local, strategy);
    //    return repository;
    //}

    public static Repository<TResult> CreateRepository<TRepository, TResult>(IQueryable<TResult> result)
        where TRepository : class, IQueryableRepository
        where TResult : class {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (Repository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }

    public static AsyncRepository<TResult> CreateAsyncRepository<TRepository, TResult>(IQueryable<TResult> result)
        where TRepository : class, IAsyncQueryableRepository
        where TResult : class {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (AsyncRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }
}
