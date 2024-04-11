namespace DotNetToolbox.Domain.Models;

public interface IEntity {
    [NotNull]
    object Id { get; }
}

public interface IEntity<TKey>
    : IEntity
    where TKey : notnull {
    object IEntity.Id => Id;
    new TKey Id { get; set; }
}
