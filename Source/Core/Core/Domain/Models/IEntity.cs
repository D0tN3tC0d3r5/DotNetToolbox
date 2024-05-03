namespace DotNetToolbox.Domain.Models;

public interface IEntity {
    [NotNull]
    object Key { get; }
}

public interface IEntity<TKey>
    : IEntity
    where TKey : notnull {
    new TKey Key { get; set; }
}
