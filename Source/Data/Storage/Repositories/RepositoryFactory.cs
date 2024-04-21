namespace DotNetToolbox.Data.Repositories;

internal static class RepositoryFactory {
    public static IRepository<TResult> CreateRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy? strategy = null)
        where TRepository : IRepository
        where TResult : class {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, strategy)!;
    }

    public static IOrderedRepository<TResult> CreateOrderedRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy? strategy = null)
        where TRepository : IOrderedRepository
        where TResult : class {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IOrderedRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, strategy)!;
    }

    public static IAsyncRepository<TResult> CreateAsyncRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy? strategy = null)
        where TRepository : IAsyncRepository
        where TResult : class {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IAsyncRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, strategy)!;
    }

    public static IAsyncOrderedRepository<TResult> CreateAsyncOrderedRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy? strategy = null)
        where TRepository : IAsyncOrderedRepository
        where TResult : class {
        var repositoryType = typeof(TRepository).GetGenericTypeDefinition();
        var resultRepositoryType = repositoryType.MakeGenericType(typeof(TResult));
        return (IAsyncOrderedRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, strategy)!;
    }
}
