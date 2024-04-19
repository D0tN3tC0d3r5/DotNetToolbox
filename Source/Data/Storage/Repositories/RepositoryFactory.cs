namespace DotNetToolbox.Data.Repositories;

public static class RepositoryFactory {
    public static IRepository<TResult> CreateRepository<TSourceRepository, TResult>(IQueryable<TResult> result)
        where TSourceRepository : class, IQueryableRepository
        where TResult : class {
        var resultRepositoryType = typeof(TSourceRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }

    public static IOrderedRepository<TResult> CreateOrderedRepository<TSourceRepository, TResult>(IQueryable<TResult> result)
        where TSourceRepository : class, IOrderedQueryableRepository
        where TResult : class {
        var resultRepositoryType = typeof(TSourceRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IOrderedRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }

    public static IAsyncRepository<TResult> CreateAsyncRepository<TSourceRepository, TResult>(IQueryable<TResult> result)
        where TSourceRepository : class, IAsyncQueryableRepository
        where TResult : class {
        var resultRepositoryType = typeof(TSourceRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IAsyncRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }

    public static IAsyncOrderedRepository<TResult> CreateAsyncOrderedRepository<TOrderedRepository, TResult>(IQueryable<TResult> result)
        where TOrderedRepository : class, IAsyncOrderedQueryableRepository
        where TResult : class {
        var resultRepositoryType = typeof(TOrderedRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IAsyncOrderedRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }
}
