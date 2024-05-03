namespace DotNetToolbox.Data.Repositories.ValueObject;

public class OffsetValueObjectRepository<TItem>
    : OffsetValueObjectRepository<IValueObjectRepositoryStrategy<TItem>, TItem> {
    public OffsetValueObjectRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryValueObjectRepositoryStrategy<TItem>(), data) { }
    public OffsetValueObjectRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base((IValueObjectRepositoryStrategy<TItem>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public OffsetValueObjectRepository(IValueObjectRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    public OffsetValueObjectRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, new InMemoryValueObjectRepositoryStrategy<TItem>(), data) { }
    public OffsetValueObjectRepository(string name, IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base(name, (IValueObjectRepositoryStrategy<TItem>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public OffsetValueObjectRepository(string name, IValueObjectRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }
}

public abstract class OffsetValueObjectRepository<TStrategy, TItem>
    : ValueObjectRepository<TItem>
    , IOffsetQueryableRepository<TItem>
    where TStrategy : class, IValueObjectRepositoryStrategy<TItem> {
    protected OffsetValueObjectRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    protected OffsetValueObjectRepository(string name, TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }

    #region Blocking

    public Block<TItem> GetBlock(Expression<Func<TItem, bool>> isNotStart, uint blockSize = DefaultBlockSize)
        => Strategy.GetBlock(isNotStart, blockSize);

    #endregion

    #region Async

    public ValueTask<Block<TItem>> GetBlockAsync(Expression<Func<TItem, bool>> findStart, uint blockSize = DefaultBlockSize, CancellationToken ct = default)
        => Strategy.GetBlockAsync(findStart, blockSize, ct);

    #endregion
}
