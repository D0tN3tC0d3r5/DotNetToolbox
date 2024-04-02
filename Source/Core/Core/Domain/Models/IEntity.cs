namespace DotNetToolbox.Domain.Models;

public interface IEntity;

public interface IEntity<TEntity>
    : IEntity
    where TEntity : IEntity<TEntity>;
