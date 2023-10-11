namespace System.Pagination;

public interface IBlock<out TItem> : IBlock<TItem, TItem> {
}

public interface IBlock<out TItem, out TOffset> {
    uint Size { get; }
    TOffset Offset { get; }
    IReadOnlyList<TItem> Items { get; }
}