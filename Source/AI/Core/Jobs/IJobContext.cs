namespace DotNetToolbox.AI.Jobs;

public interface IJobContext
    : IContext {
    Context<Asset> Assets { get; init; }
    IHttpConnection Connection { get; init; }
    Context<string> Memory { get; init; }
    Model Model { get; init; }
    Persona Persona { get; init; }
    Task Task { get; init; }
    Context<Tool> Tools { get; init; }
    UserProfile UserProfile { get; init; }
    World World { get; init; }
}
