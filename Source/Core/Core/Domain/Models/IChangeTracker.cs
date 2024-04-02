namespace DotNetToolbox.Domain.Models;

public interface IChangeTracker<TTracker>
    : ITrackChange
    where TTracker : IChangeTracker<TTracker>;
