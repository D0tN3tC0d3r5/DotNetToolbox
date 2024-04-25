namespace DotNetToolbox.Data.Repositories;

internal class RepositoryFactory
    : IRepositoryFactory {
    public IRepository<TResult> CreateRepository<TRepository, TResult>(IQueryable<TResult> result)
        where TRepository : IRepository {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }

    public IRepository<TResult> CreateRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy strategy)
        where TRepository : IRepository {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, IsNotNull(strategy))!;
    }

    public IAsyncRepository<TResult> CreateAsyncRepository<TRepository, TResult>(IQueryable<TResult> result)
        where TRepository : IAsyncRepository {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IAsyncRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result)!;
    }

    public IAsyncRepository<TResult> CreateAsyncRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy strategy)
        where TRepository : IAsyncRepository {
        var resultRepositoryType = typeof(TRepository).GetGenericTypeDefinition().MakeGenericType(typeof(TResult));
        return (IAsyncRepository<TResult>)Activator.CreateInstance(resultRepositoryType, result, IsNotNull(strategy))!;
    }
}
