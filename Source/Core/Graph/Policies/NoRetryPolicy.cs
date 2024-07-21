namespace DotNetToolbox.Graph.Policies;

public class NoRetryPolicy : Policy {
    public override void Execute(Action action)
        => action();
}
