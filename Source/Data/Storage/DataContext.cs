namespace DotNetToolbox.Data;

public abstract class DataContext {
    public abstract Task<int> SaveChanges(CancellationToken ct = default);
    public virtual Task EnsureIsUpToDate(CancellationToken ct = default) => Seed(ct);
    protected virtual Task Seed(CancellationToken ct = default) => Task.CompletedTask;
}
