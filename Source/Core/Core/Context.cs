namespace DotNetToolbox;

public class Context(IServiceProvider services, IDictionary<string, object>? source = null)
    : Map(source),
      IContext {

    public IServiceProvider Services { get; } = services;
}
