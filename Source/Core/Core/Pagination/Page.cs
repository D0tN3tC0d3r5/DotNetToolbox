using static DotNetToolbox.Pagination.PaginationSettings;

namespace DotNetToolbox.Pagination;

[method: SetsRequiredMembers]
public class Page<TItem>(IReadOnlyList<TItem> items, uint index = 0, uint size = DefaultPageSize, uint totalCount = 0)
    : IPage<TItem> {
    [SetsRequiredMembers]
    public Page() : this([]) {
    }

    public uint TotalCount { get; init; } = totalCount;
    public required IReadOnlyList<TItem> Items { get; init; } = IsNotNull(items);
    public uint Index { get; init; } = index;
    public uint Size { get; init; } = size;
}
