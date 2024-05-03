
using DotNetToolbox.Data.Strategies.Key;

namespace DotNetToolbox.Data.InMemory;

public class InMemoryEntityRepositoryStrategy<TItem, TKey, TKeyHandler>
    : EntityRepositoryStrategy<TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {

    private readonly IValueObjectRepositoryStrategy<TItem> _keylessStrategy;

    public InMemoryEntityRepositoryStrategy(string name, IValueObjectRepositoryStrategy<TItem>? keylessStrategy = null) {
        Name = name;
        _keylessStrategy = keylessStrategy ?? new InMemoryValueObjectRepositoryStrategy<TItem>();
    }

    #region Blocking

    public override void Seed(IEnumerable<TItem> seed)
        => _keylessStrategy.Seed(seed);
    public override void Load()
        => _keylessStrategy.Load();

    public override TItem[] GetAll()
        => _keylessStrategy.GetAll();
    public override Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = 20)
        => _keylessStrategy.GetPage(pageIndex, pageSize);
    public override Block<TItem> GetBlock(Expression<Func<TItem, bool>> isNotStart, uint blockSize = 20)
        => _keylessStrategy.GetBlock(isNotStart, blockSize);

    public override TItem? Find(Expression<Func<TItem, bool>> predicate)
        => _keylessStrategy.Find(predicate);
    public override TItem? FindByKey(TKey key)
        => Find(x => KeyHandler.Equals(x.Key, key));

    public override TItem Create(Action<TItem> setItem) {
        var item = _keylessStrategy.Create(setItem);
        item.Key = KeyHandler.GetNext(Name, item.Key);
        return item;
    }

    public override void Add(TItem newItem) {
        newItem.Key = KeyHandler.GetNext(Name, newItem.Key);
        _keylessStrategy.Add(newItem);
    }

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => _keylessStrategy.Update(predicate, updatedItem);
    public override void Update(TItem updatedItem)
        => Update(x => KeyHandler.Equals(x.Key, updatedItem.Key), updatedItem);

    public override void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem)
        => _keylessStrategy.Patch(predicate, setItem);
    public override void Patch(TKey key, Action<TItem> setItem)
        => Patch(x => KeyHandler.Equals(x.Key, key), setItem);

    public override void Remove(Expression<Func<TItem, bool>> predicate)
        => _keylessStrategy.Remove(predicate);
    public override void Remove(TKey key)
        => Remove(x => KeyHandler.Equals(x.Key, key));

    #endregion

    #region Async

    public override Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default)
        => _keylessStrategy.SeedAsync(seed, ct);
    public override Task LoadAsync(CancellationToken ct = default)
        => _keylessStrategy.LoadAsync(ct);

    public override ValueTask<TItem[]> GetAllAsync(CancellationToken ct = default)
        => _keylessStrategy.GetAllAsync(ct);
    public override ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = 20, CancellationToken ct = default)
        => _keylessStrategy.GetPageAsync(pageIndex, pageSize, ct);
    public override ValueTask<Block<TItem>> GetBlockAsync(Expression<Func<TItem, bool>> findStart, uint blockSize = 20, CancellationToken ct = default)
        => _keylessStrategy.GetBlockAsync(findStart, blockSize, ct);

    public override ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _keylessStrategy.FindAsync(predicate, ct);
    public override ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => FindAsync(x => KeyHandler.Equals(x.Key, key), ct);

    public override async Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) {
        var item = await _keylessStrategy.CreateAsync(setItem, ct);
        item.Key = await KeyHandler.GetNextAsync(Name, item.Key, ct);
        return item;
    }

    public override async Task AddAsync(TItem newItem, CancellationToken ct = default) {
        newItem.Key = await KeyHandler.GetNextAsync(Name, newItem.Key, ct);
        await _keylessStrategy.AddAsync(newItem, ct);
    }

    public override Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => _keylessStrategy.UpdateAsync(predicate, updatedItem, ct);
    public override Task UpdateAsync(TItem updatedItem, CancellationToken ct = default)
        => UpdateAsync(x => KeyHandler.Equals(x.Key, updatedItem.Key), updatedItem, ct);

    public override Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => _keylessStrategy.PatchAsync(predicate, setItem, ct);
    public override Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => PatchAsync(x => KeyHandler.Equals(x.Key, key), setItem, ct);

    public override Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _keylessStrategy.RemoveAsync(predicate, ct);
    public override Task RemoveAsync(TKey key, CancellationToken ct = default)
        => RemoveAsync(x => KeyHandler.Equals(x.Key, key), ct);

    #endregion
}
