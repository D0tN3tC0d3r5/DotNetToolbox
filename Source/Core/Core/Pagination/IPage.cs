namespace DotNetToolbox.Pagination;

public interface IPage<out TItem> {
    uint TotalCount { get; }
    IReadOnlyList<TItem> Items { get; }
    uint Index { get; }
    uint Size { get; }
}
