namespace DotNetToolbox.Domain.Models;

public interface ITrackState : ITrackState<string>;

public interface ITrackState<out TState>
    : ITrackChange {
    TState? State { get; }
}
