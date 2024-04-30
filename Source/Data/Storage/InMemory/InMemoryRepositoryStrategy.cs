namespace DotNetToolbox.Data.InMemory;

public class InMemoryRepositoryStrategy<TItem>
    : RepositoryStrategy<TItem> {
    #region Blocking

    public override void Add(TItem newItem)
        => OriginalData.Add(newItem);

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) {
        if (!TryRemove(predicate))
            return;
        Add(updatedItem);
    }

    public override void Remove(Expression<Func<TItem, bool>> predicate)
        => TryRemove(predicate);

    private bool TryRemove(Expression<Func<TItem, bool>> predicate) {
        var itemToRemove = Query.FirstOrDefault(predicate);
        if (itemToRemove is null)
            return false;
        OriginalData.Remove(itemToRemove);
        return true;
    }

    #endregion

    #region Async

    public override Task AddAsync(TItem newItem, CancellationToken ct = default) {
        OriginalData.Add(newItem);
        return Task.CompletedTask;
    }

    public override async Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) {
        if (!await TryRemoveAsync(predicate, ct))
            return;
        await AddAsync(updatedItem, ct);
    }

    public override Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => TryRemoveAsync(predicate, ct);

    private async Task<bool> TryRemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        var itemToRemove = await AsyncQuery.FirstOrDefaultAsync(predicate.Compile(), ct);
        if (itemToRemove is null)
            return false;
        OriginalData.Remove(itemToRemove);
        return true;
    }

    #endregion
}
