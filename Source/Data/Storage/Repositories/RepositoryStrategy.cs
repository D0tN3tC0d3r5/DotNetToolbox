namespace DotNetToolbox.Data.Repositories;

public abstract class RepositoryStrategy<TItem>
    : RepositoryUpdateStrategy<TItem>
    where TItem : class {
}
