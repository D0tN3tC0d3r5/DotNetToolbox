namespace DotNetToolbox;

public interface IAwaiter {
    bool IsWaiting { get; }
    Task StartWait(CancellationToken ct);
    void StopWaiting();
}
