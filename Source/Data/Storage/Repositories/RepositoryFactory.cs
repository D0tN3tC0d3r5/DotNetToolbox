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

    public static IOrderedRepository<TResult> CreateOrderedRepository<TRepository, TResult>(IQueryable<TResult> result)
        where TRepository : IOrderedRepository{
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IOrderedRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }

    public static IOrderedRepository<TResult> CreateOrderedRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy strategy)
        where TRepository : IOrderedRepository{
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IOrderedRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, IsNotNull(strategy))!;
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

    public static IAsyncOrderedRepository<TResult> CreateAsyncOrderedRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy? strategy = null)
        where TRepository : IAsyncOrderedRepository{
        var repositoryType = typeof(TRepository).GetGenericTypeDefinition();
        var resultRepositoryType = repositoryType.MakeGenericType(typeof(TResult));
        return (IAsyncOrderedRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, strategy)!;
    }
}
