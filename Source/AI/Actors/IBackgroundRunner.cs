namespace DotNetToolbox.AI.Actors;

public interface IBackgroundRunner {
    void Run(CancellationToken ct);
}
