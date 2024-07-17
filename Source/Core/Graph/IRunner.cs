namespace DotNetToolbox.Graph;

public interface IRunner {
    public string Id { get; }
    public bool IsRunning { get; }
    public DateTimeOffset? Start { get; }
    public DateTimeOffset? End { get; }
    public TimeSpan? ElapsedTime { get; }
    public bool HasStarted { get; }
    public bool HasStopped { get; }

    Context Run(Context? context = null);
}
