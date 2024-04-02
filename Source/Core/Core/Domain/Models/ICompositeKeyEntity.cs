namespace DotNetToolbox.Domain.Models;

public interface ICompositeKeyEntity
    : IEntity {
    object GetKey();
    object?[] GetKeys();
}

public interface ICompositeKeyEntity<TEntity>
    : IEntity<TEntity>,
      ICompositeKeyEntity
    where TEntity : ICompositeKeyEntity<TEntity>;
