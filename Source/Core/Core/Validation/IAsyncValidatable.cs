namespace DotNetToolbox.Validation;

public interface IAsyncValidatable
    : IAsyncValidatable<IMap>;

public interface IAsyncValidatable<in TContext>
    where TContext : class {
    Task<Result> ValidateAsync(TContext? context = null, CancellationToken token = default);
    Task<Result> ValidateAsync(CancellationToken token = default) => ValidateAsync(null, token);
}
