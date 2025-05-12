namespace DotNetToolbox.Domain.Models;

public interface IEntity;

public interface IEntity<TKey>
    : IEntity
    , IValidatable {
    TKey Id { get; set; }
}
