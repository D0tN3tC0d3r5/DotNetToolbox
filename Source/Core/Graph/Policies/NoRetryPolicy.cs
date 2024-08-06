namespace DotNetToolbox.Graph.Policies;

public class NoRetryPolicy()
    : RetryPolicy<NoRetryPolicy>(byte.MinValue);
