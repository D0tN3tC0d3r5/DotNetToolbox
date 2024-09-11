using DotNetToolbox.AI.Personas;

namespace DotNetToolbox.AI.Jobs;

public interface IJobContext
    : IMap {
    Map<Asset> Assets { get; init; }
    IAgent Agent { get; init; }
    Map<string> Memory { get; init; }
    Model Model { get; init; }
    Persona Persona { get; init; }
    Task Task { get; init; }
    Map<Tool> Tools { get; init; }
    UserProfile UserProfile { get; init; }
    World World { get; init; }
}
