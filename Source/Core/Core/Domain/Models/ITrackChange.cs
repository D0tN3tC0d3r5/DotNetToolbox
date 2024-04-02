namespace DotNetToolbox.Domain.Models;

public interface ITrackChange {
    DateTimeOffset DateTime { get; }
    ChangeType Type { get; }
}