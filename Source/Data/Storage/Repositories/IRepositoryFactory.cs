
namespace DotNetToolbox.Data.Repositories;

internal interface IRepositoryFactory {
    IAsyncRepository<TResult> CreateAsyncRepository<TRepository, TResult>(IQueryable<TResult> result) where TRepository : IAsyncRepository;
    IAsyncRepository<TResult> CreateAsyncRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy strategy) where TRepository : IAsyncRepository;
    IRepository<TResult> CreateRepository<TRepository, TResult>(IQueryable<TResult> result) where TRepository : IRepository;
    IRepository<TResult> CreateRepository<TRepository, TResult>(IQueryable<TResult> result, IRepositoryStrategy strategy) where TRepository : IRepository;
}