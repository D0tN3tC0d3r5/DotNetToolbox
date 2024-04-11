namespace DotNetToolbox.Data.Repositories;

public interface ICanAddItem<in TItem> {
    void Add(TItem input);
}
