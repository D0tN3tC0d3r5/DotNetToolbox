namespace DotNetToolbox.Data.Repositories;

public interface ICanCreateItem<out TItem>
    where TItem : class, new() {
    TItem Create(Action<TItem> setItem);
}
