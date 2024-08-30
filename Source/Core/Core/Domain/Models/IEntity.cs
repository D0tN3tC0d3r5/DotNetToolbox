namespace DotNetToolbox.Domain.Models;

public interface IEntity;

public interface IEntity<TKey>
    : IEntity,
      IValidatable
    where TKey : notnull {
    TKey Key { get; set; }
}
