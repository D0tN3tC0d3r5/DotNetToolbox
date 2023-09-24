namespace DotNetToolbox.Collections;

public abstract record PagedCollection<TCollection, TItem> : HasEmpty<TCollection>
    where TCollection : PagedCollection<TCollection, TItem>, new()
{
    public virtual uint TotalCount { get; init; }
    public virtual IReadOnlyList<TItem> Items { get; init; } = Array.Empty<TItem>();

    public virtual uint PageSize { get; init; } = PaginationSettings.DefaultPageSize;
    public virtual uint PageIndex { get; init; }

    protected TNewCollection Map<TNewCollection, TNewItem>(Func<TItem, TNewItem> map)
        where TNewCollection : PagedCollection<TNewCollection, TNewItem>, new()
        => new()
        {
            TotalCount = TotalCount,
            Items = Items.Select(map).ToArray(),
            PageIndex = PageIndex,
            PageSize = PageSize,
        };
}

public sealed record PagedCollection<TItem> : PagedCollection<PagedCollection<TItem>, TItem>
{
    public PagedCollection<TNewItem> Map<TNewItem>(Func<TItem, TNewItem> map)
        => Map<PagedCollection<TNewItem>, TNewItem>(map);
}
