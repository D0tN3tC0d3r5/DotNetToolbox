namespace DotNetToolbox.Graph.PathBuilder;

public interface IPathBuilder {
    IIfBuilder If(Func<Map, bool> predicate);
    ISwitchBuilder<TKey> Switch<TKey>(Func<Map, TKey> select);
    IPathBuilder Do(Action<Map> execute);
    void End();
}
