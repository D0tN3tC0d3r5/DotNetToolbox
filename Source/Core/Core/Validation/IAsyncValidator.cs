namespace DotNetToolbox.Validation;

public interface IAsyncValidator<in TValue>
    : IAsyncValidator<TValue, IMap>;

public interface IAsyncValidator<in TValue, in TContext>
    where TContext : class {
    Task<bool> IsValidAsync(TValue value, TContext? context = null, CancellationToken token = default);
    Task<bool> IsValidAsync(TValue value, CancellationToken token = default) => IsValidAsync(value, null, token);
}
