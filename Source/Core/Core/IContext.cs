namespace DotNetToolbox;

public interface IContext
    : IContext<object>,
      IMap;

public interface IContext<TValue>
    : IMap<TValue>,
      IDisposable;
