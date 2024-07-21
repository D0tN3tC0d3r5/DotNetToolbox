namespace DotNetToolbox.Graph.Policies;

public interface IPolicy {
    void Execute(Action action);
}
