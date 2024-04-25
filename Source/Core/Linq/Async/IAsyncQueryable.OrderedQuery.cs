namespace DotNetToolbox.Linq.Async;

//[SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "Does not apply.")]
//public interface IOrderedAsyncQueryable
//    : IAsyncQueryable;

public interface IOrderedAsyncQueryable<out TItem>
    : IAsyncQueryable<TItem> {
}
