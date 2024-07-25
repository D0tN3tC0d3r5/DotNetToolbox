namespace DotNetToolbox.Graph.Policies;

public class NoRetryPolicy()
    : RetryPolicy(byte.MinValue) {
    public override bool TryExecute(Action action) {
        action();
        return true;
    }
}
