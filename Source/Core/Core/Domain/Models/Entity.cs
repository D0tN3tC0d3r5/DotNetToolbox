
namespace DotNetToolbox.Domain.Models;

public abstract class Entity<TEntity, TKey>
    : IEntity<TKey>
    where TEntity : Entity<TEntity, TKey>, new()
    where TKey : notnull {
    public TKey Key { get; set; } = default!;

    public virtual Result Validate(IMap? context = null)
        => Result.Success();
}
