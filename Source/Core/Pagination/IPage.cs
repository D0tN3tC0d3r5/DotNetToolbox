namespace DotNetToolbox.Pagination;

public interface IPage<out TItem> : IBlock<TItem, uint> {
    uint TotalCount { get; init; }
}