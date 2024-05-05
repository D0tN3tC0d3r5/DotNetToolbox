namespace DotNetToolbox.Domain.Models;

public interface IEntity;

public interface IEntity<TKey>
    : IEntity
    where TKey : notnull {
    TKey Key { get; set; }
}
