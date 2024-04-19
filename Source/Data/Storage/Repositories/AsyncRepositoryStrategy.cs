namespace DotNetToolbox.Data.Repositories;

public abstract class AsyncRepositoryStrategy<TItem>
    : AsyncRepositoryUpdateStrategy<TItem>
    where TItem : class {
}
