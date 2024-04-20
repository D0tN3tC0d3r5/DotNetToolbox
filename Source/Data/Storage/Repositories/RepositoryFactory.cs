namespace DotNetToolbox.Data.Repositories;

public static class RepositoryFactory {
    public static IRepository<TResult> CreateRepository<TSourceRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy? strategy = null)
        where TSourceRepository : IQueryableRepository
        where TResult : class {
        var resultRepositoryType = typeof(TSourceRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, strategy)!;
    }

    public static IOrderedRepository<TResult> CreateOrderedRepository<TSourceRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy? strategy = null)
        where TSourceRepository : IOrderedQueryableRepository
        where TResult : class {
        var resultRepositoryType = typeof(TSourceRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IOrderedRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, strategy)!;
    }

    public static IAsyncRepository<TResult> CreateAsyncRepository<TSourceRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy? strategy = null)
        where TSourceRepository : IAsyncQueryableRepository
        where TResult : class {
        var resultRepositoryType = typeof(TSourceRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IAsyncRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, strategy)!;
    }

    public static IAsyncOrderedRepository<TResult> CreateAsyncOrderedRepository<TOrderedRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy? strategy = null)
        where TOrderedRepository : IAsyncOrderedQueryableRepository
        where TResult : class {
        var resultRepositoryType = typeof(TOrderedRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IAsyncOrderedRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, strategy)!;
    }
}
