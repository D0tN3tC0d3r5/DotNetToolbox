namespace DotNetToolbox.Graph.Policies;

public class NoRetryPolicy()
    : RetryPolicy(byte.MinValue) {
    protected override bool TryExecute(Action action) {
        action();
        return true;
    }
}
