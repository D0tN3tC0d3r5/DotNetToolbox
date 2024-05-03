using DotNetToolbox.Data.Strategies.Key;

namespace DotNetToolbox.Data.Strategies.Entity;

public interface IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>
    : IValueObjectRepositoryStrategy<TItem>
    , IEntityRepository<TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {

    TKeyHandler KeyHandler { get; set; }
};
