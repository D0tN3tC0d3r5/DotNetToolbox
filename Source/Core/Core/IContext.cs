namespace DotNetToolbox;

public interface IContext
    : IMap,
      IDisposable {
    IServiceProvider Services { get; }
}
