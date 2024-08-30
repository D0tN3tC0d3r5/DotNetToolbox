
namespace DotNetToolbox.Domain.Models;

public abstract class Entity<TEntity, TKey>
    : IEntity<TKey>
    where TEntity : Entity<TEntity, TKey>, new()
    where TKey : notnull {
    public TKey Key { get; set; } = default!;

    public virtual Result Validate(IContext? context = null) {
        var result = Result.Success();
        if (Key is null) result += new ValidationError("Key is required.", nameof(Key));
        return result;
    }
}
