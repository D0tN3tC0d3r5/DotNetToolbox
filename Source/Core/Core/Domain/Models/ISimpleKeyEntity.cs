namespace DotNetToolbox.Domain.Models;

public interface ISimpleKeyEntity<TKey>
    : IEntity {
    TKey Id { get; set; }
}

public interface ISimpleKeyEntity<TEntity, TKey>
    : IEntity<TEntity>,
      ISimpleKeyEntity<TKey>
    where TEntity : ISimpleKeyEntity<TEntity, TKey>;
