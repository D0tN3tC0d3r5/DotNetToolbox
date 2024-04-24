namespace DotNetToolbox.Data.Repositories;

internal static class RepositoryFactory {
    public static IRepository<TResult> CreateRepository<TRepository, TResult>(IQueryable<TResult> result)
        where TRepository : IRepository{
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }

    public static IRepository<TResult> CreateRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy strategy)
        where TRepository : IRepository{
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, IsNotNull(strategy))!;
    }

    public static IAsyncRepository<TResult> CreateAsyncRepository<TRepository, TResult>(IQueryable<TResult> result)
        where TRepository : IAsyncRepository{
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IAsyncRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }

    public static IAsyncRepository<TResult> CreateAsyncRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy strategy)
        where TRepository : IAsyncRepository{
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IAsyncRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, IsNotNull(strategy))!;
    }
}
